using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public float damage;

    // Start is called before the first frame update
    void Start()
    {
        // (Optional) Initialize any variables here
    }

    // Update is called once per frame
    void Update()
    {
        // (Optional) Logic every frame
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // If the colliding object is tagged "Player", deal damage
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().health -= damage;
        }
    }
}
