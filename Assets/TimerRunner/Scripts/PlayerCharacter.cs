using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace IndieMarc.Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class PlayerCharacter : MonoBehaviour
    {
        public int player_id;

        [Header("Stats")]
        public float max_hp = 100f;

        [Header("Movement")]
        // Since the character runs in place, we don't move horizontally by default.
        public float move_max = 0f;  // Not used except for reference

        [Header("Jump")]
        public bool can_jump = true;
        public bool double_jump = true;
        public float jump_strength = 10f;
        public float gravity = 20f;
        public LayerMask ground_layer;
        public float ground_raycast_dist = 0.1f;

        [Header("Crouch")]
        public bool can_crouch = true;
        public float crouch_coll_percent = 0.5f;

        [Header("Fall Below Level")]
        public bool reset_when_fall = true;
        public float fall_pos_y = -5f;
        public float fall_damage_percent = 0.25f;

        [Header("Dash")]
        [Tooltip("Whether the player can dash at all.")]
        public bool can_dash = true;
        [Tooltip("Horizontal dash speed.")]
        public float dash_power = 15f;
        [Tooltip("Time (in seconds) that the dash remains active.")]
        public float dash_time = 0.3f;
        [Tooltip("Time (in seconds) after dash before you can dash again.")]
        public float dash_cooldown = 1f;

        // ADDED/CHANGED:
        [Header("Magnet Pull Back (Horizontal Only)")]
        [Tooltip("The speed used to pull player back horizontally to dash start.")]
        public float magnetPullSpeed = 5f;

        [Header("Dash Effect (Optional)")]
        [Tooltip("Assign a TrailRenderer or ParticleSystem for dash visuals. Leave empty if not needed.")]
        [SerializeField] private TrailRenderer dashEffect;
        [SerializeField] private DashGhostEffect ghostEffect;

        public UnityAction onDeath;
        public UnityAction onHit;
        public UnityAction onJump;
        public UnityAction onLand;
        public UnityAction onCrouch;

        private Rigidbody2D rigid;
        private CapsuleCollider2D capsule_coll;
        private Vector2 coll_start_h;
        private Vector2 coll_start_off;
        private Vector3 start_scale;
        private Vector3 last_ground_pos;
        private Vector3 average_ground_pos;

        // Track vertical velocity ourselves.
        private float verticalVelocity = 0f;

        private Vector2 move_input;
        private bool jump_press;
        private bool jump_hold;

        private float hp;
        private bool is_dead = false;
        private bool is_grounded = false;
        private bool is_crouch = false;
        private bool is_jumping = false;
        private bool hasDoubleJumped = false;
        private bool disable_controls = false;
        private Animator animator;

        // --- DASH STATE ---
        private bool isDashing = false;   // Are we in the middle of a dash?
        private bool dashReady = true;    // Can we dash (cooldown expired)?
        private float dashHorizontalVelocity = 0f;

        // ADDED/CHANGED:
        // We'll remember the player's X at dash start, and let them return to it afterward
        private float dashStartX = 0f;
        // True if we are currently being pulled back horizontally
        private bool isReturning = false;

        private static Dictionary<int, PlayerCharacter> character_list = new Dictionary<int, PlayerCharacter>();

        void Awake()
        {
            character_list[player_id] = this;
            rigid = GetComponent<Rigidbody2D>();
            capsule_coll = GetComponent<CapsuleCollider2D>();
            coll_start_h = capsule_coll.size;
            coll_start_off = capsule_coll.offset;
            start_scale = transform.localScale;
            average_ground_pos = transform.position;
            last_ground_pos = transform.position;
            animator = GetComponent<Animator>();
            hp = max_hp;

            if (dashEffect)
                dashEffect.emitting = false;
        }

        void OnDestroy()
        {
            character_list.Remove(player_id);
        }

        void Update()
        {
            if (is_dead)
                return;

            if (frozen)
            {
                move_input = Vector2.zero;
                jump_press = false;
                jump_hold = false;
            }
            else
            {
                PlayerControls controls = PlayerControls.Get(player_id);
                move_input = !disable_controls ? controls.GetMove() : Vector2.zero;
                jump_press = !disable_controls ? controls.GetJumpDown() : false;
                jump_hold = !disable_controls ? controls.GetJumpHold() : false;
            }


            if (jump_press)
                Jump();

            // Dash input
            if (can_dash && dashReady && !isDashing && !isReturning && Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartCoroutine(DoDash());
            }

            /*
            if (transform.position.y < fall_pos_y && !is_grounded && verticalVelocity < -20f)
            {
                Teleport(last_ground_pos);
            }*/
        }

        void FixedUpdate()
        {
            if (is_dead)
                return;

            if (frozen)
            {
                rigid.linearVelocity = Vector2.zero;
                return;
            }


            // Grounded check
            bool wasGrounded = is_grounded;
            is_grounded = DetectGrounded();

            if (!wasGrounded && is_grounded)
            {
                is_jumping = false;
                hasDoubleJumped = false;
                last_ground_pos = transform.position;
                onLand?.Invoke();
            }

            // Gravity & vertical velocity
            if (!is_grounded)
            {
                verticalVelocity -= gravity * Time.fixedDeltaTime;
            }
            else if (!is_jumping)
            {
                verticalVelocity = 0f;
            }

            // Decide final horizontal velocity
            float horizontalVelocity = 0f;

            // If we are dashing, use dash velocity
            if (isDashing)
            {
                horizontalVelocity = dashHorizontalVelocity;
            }
            // ADDED/CHANGED: If we are returning, compute velocity that moves us back toward dashStartX
            else if (isReturning)
            {
                float currentX = transform.position.x;
                float dx = dashStartX - currentX;

                // If close enough, stop returning
                if (Mathf.Abs(dx) < 0.05f)
                {
                    // Snap to dashStartX precisely
                    Vector3 snapPos = transform.position;
                    snapPos.x = dashStartX;
                    transform.position = snapPos;

                    isReturning = false;
                    horizontalVelocity = 0f;
                }
                else
                {
                    // Move horizontally to reduce dx
                    // This will not affect vertical movement
                    float pullDir = Mathf.Sign(dx);
                    horizontalVelocity = magnetPullSpeed * pullDir;
                }
            }
            // Otherwise, stand still horizontally
            else
            {
                horizontalVelocity = 0f;
            }

            // Apply velocity to rigidbody
            rigid.linearVelocity = new Vector2(horizontalVelocity, verticalVelocity);
        }

        public void Jump()
        {
            if (!can_jump || is_crouch)
                return;

            if (is_grounded)
            {
                verticalVelocity = jump_strength;
                is_jumping = true;
                hasDoubleJumped = false;
                onJump?.Invoke();
            }
            else if (double_jump && !hasDoubleJumped)
            {
                verticalVelocity = jump_strength;
                is_jumping = true;
                hasDoubleJumped = true;
                onJump?.Invoke();
            }
        }

        private IEnumerator DoDash()
        {
            dashReady = false;
            // Store where we started so we can return there
            dashStartX = transform.position.x;

            isDashing = true;
            if (ghostEffect)
                ghostEffect.StartGhosting();

            float dashDir = Mathf.Sign(transform.localScale.x);
            dashHorizontalVelocity = dash_power * dashDir;

            float timer = 0f;
            while (timer < dash_time)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Dash ends
            isDashing = false;
            dashHorizontalVelocity = 0f;
            if (ghostEffect)
                ghostEffect.StopGhosting();

            // Start returning
            isReturning = true;

            // Wait for dash cooldown
            float cooldown = 0f;
            while (cooldown < dash_cooldown)
            {
                cooldown += Time.deltaTime;
                yield return null;
            }

            dashReady = true;
        }

        private bool DetectGrounded()
        {
            Vector2 origin = (Vector2)transform.position + capsule_coll.offset;
            float rayLength = (capsule_coll.size.y * 0.5f * transform.localScale.y) + ground_raycast_dist;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, rayLength, ground_layer);
            Debug.DrawRay(origin, Vector2.down * rayLength, hit.collider ? Color.green : Color.red);

            return (hit.collider != null);
        }

        public void Teleport(Vector3 pos)
        {
            transform.position = pos;
            verticalVelocity = 0f;
            is_jumping = false;
        }

        private bool frozen = false;

        public void Freeze(bool state)
        {
            frozen = state;
            if ((frozen) || (animator != null))
            {
                rigid.linearVelocity = Vector2.zero;
                verticalVelocity = 0f;
                animator.speed = state ? 0f : 1f;
            }
        }


        public Vector2 GetMove()
        {
            return rigid.linearVelocity;
        }

        public Vector2 GetFacing()
        {
            return Vector2.right * Mathf.Sign(transform.localScale.x);
        }

        public static PlayerCharacter Get(int player_id)
        {
            foreach (PlayerCharacter character in GetAll())
            {
                if (character.player_id == player_id)
                    return character;
            }
            return null;
        }

        public static PlayerCharacter[] GetAll()
        {
            PlayerCharacter[] list = new PlayerCharacter[character_list.Count];
            character_list.Values.CopyTo(list, 0);
            return list;
        }
    }
}
