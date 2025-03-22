using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeBody : MonoBehaviour
{

    public bool isRewinding = false;

    List<Vector2> positions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        positions = new List<Vector2>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            StopRewind();
        }
    }

    void FixedUpdate()
    {
        if (isRewinding)
            Rewind();
        else
            Record();
    }

    void Rewind()
    {
        transform.position = positions[0];
        positions.RemoveAt(0);
    }

    void Record()
    {
        positions.Insert(0, transform.position);
    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {
        isRewinding = false;
    }
}
