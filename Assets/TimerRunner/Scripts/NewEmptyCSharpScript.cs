using UnityEngine;
using System.Collections.Generic;

public class Quit : MonoBehaviour
{
    void Start()
    {
        // Optional: Leave this empty or add logic here
    }

    void Update()
    {
        // Optional: Leave this empty or add logic here
    }

    // This function is for the Quit button (exits the game)
    public void QuitGame()
    {
        Application.Quit();  // This works when you build the game
    }
}

