using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public Image healthBar;

    private bool isDead = false;
    public GameManagerScript gameManager;

    private void Start()
    {
        health = maxHealth;
        UpdateHealthBar();
    }

    private void Update()
    {
        // Check if player fell off the map
        if (transform.position.y < -10 && !isDead)
        {
            Die("Player fell and died.");
        }

        // Just in case: check for health reaching 0
        if (health <= 0 && !isDead)
        {
            Die("Player is dead.");
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
        UpdateHealthBar();

        if (health <= 0)
        {
            Die("Player is dead.");
        }
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = Mathf.Clamp01(health / maxHealth);
        }
    }

    private void Die(string reason)
    {
        isDead = true;
        Debug.Log(reason);

        if (gameManager != null)
        {
            gameManager.gameOver();
        }
        else
        {
            Debug.LogWarning("GameManager not assigned.");
        }

        Destroy(gameObject);
    }
}
