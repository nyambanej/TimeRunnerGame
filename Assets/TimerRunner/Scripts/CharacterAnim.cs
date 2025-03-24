using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IndieMarc.Platformer
{
    [RequireComponent(typeof(PlayerCharacter))]
    [RequireComponent(typeof(Animator))]
    public class CharacterAnim : MonoBehaviour
    {
        private PlayerCharacter character;
        private Animator animator;

        void Awake()
        {
            character = GetComponent<PlayerCharacter>();
            animator = GetComponent<Animator>();

            // Optional: Setup for future
            // character.onJump += OnJump;
            // character.onCrouch += OnCrouch;
        }

        void Update()
        {
            // Only set parameters that actually exist in your Animator
            float speed = Mathf.Abs(character.move_max);
            animator.SetFloat("Speed", speed);

            // For later:
            // animator.SetBool("Jumping", character.IsJumping());
            // animator.SetBool("InAir", !character.IsGrounded());
            // animator.SetBool("Crouching", character.IsCrouching());
            // animator.SetBool("Hold", character_item?.GetHeldItem() != null);
        }

        // For future use
        // void OnCrouch()
        // {
        //     animator.SetTrigger("Crouch");
        // }

        // void OnJump()
        // {
        //     animator.SetTrigger("Jump");
        // }
    }
}
