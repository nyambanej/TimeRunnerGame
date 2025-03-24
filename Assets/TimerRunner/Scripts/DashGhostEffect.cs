namespace IndieMarc.Platformer
{
    using UnityEngine;
    using System.Collections;

    [RequireComponent(typeof(SpriteRenderer))]
    public class DashGhostEffect : MonoBehaviour
    {
        [Header("Ghost Settings")]
        public float spawnInterval = 0.05f;   // How often to spawn a ghost
        public float ghostLifetime = 0.3f;   // How long each ghost remains visible
        public Color ghostColor = new Color(1f, 1f, 1f, 0.5f); // Starting color with some transparency

        private SpriteRenderer playerSprite;
        private bool isDashing = false;

        private void Awake()
        {
            playerSprite = GetComponent<SpriteRenderer>();
        }

        public void StartGhosting()
        {
            // Called at start of dash
            isDashing = true;
            StartCoroutine(SpawnGhosts());
        }

        public void StopGhosting()
        {
            // Called at end of dash
            isDashing = false;
        }

        private IEnumerator SpawnGhosts()
        {
            while (isDashing)
            {
                // Create a GameObject that copies our sprite
                GameObject ghost = new GameObject("GhostSprite");
                SpriteRenderer sr = ghost.AddComponent<SpriteRenderer>();

                // Match sorting layer & order so it renders behind or in front as needed
                sr.sortingLayerID = playerSprite.sortingLayerID;
                sr.sortingOrder = playerSprite.sortingOrder - 1; // behind the player
                sr.sprite = playerSprite.sprite;
                sr.flipX = playerSprite.flipX;
                sr.transform.position = transform.position;
                sr.transform.localScale = transform.localScale;
                sr.transform.rotation = transform.rotation;
                sr.color = ghostColor;

                // Fade out & destroy
                StartCoroutine(FadeOutAndDestroy(sr, ghostLifetime));

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private IEnumerator FadeOutAndDestroy(SpriteRenderer sr, float lifetime)
        {
            float timer = 0f;
            Color initialColor = sr.color;

            while (timer < lifetime)
            {
                timer += Time.deltaTime;
                float alpha = 1f - (timer / lifetime);
                sr.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha * initialColor.a);
                yield return null;
            }

            Destroy(sr.gameObject);
        }
    }

}
