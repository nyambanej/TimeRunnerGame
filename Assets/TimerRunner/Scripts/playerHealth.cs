using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    private bool isDead;

    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);

        // Check if player falls off the screen 
        if (transform.position.y < -10 && !isDead)
        {
            isDead = true;
            Debug.Log("Player fell and died");
            gameManager.gameOver();
            Destroy(gameObject);
        }

        // Existing health check
        if (health <= 0 && !isDead)
        {
            isDead = true;
            gameManager.gameOver();
            Destroy(gameObject);
        }
    }
}


