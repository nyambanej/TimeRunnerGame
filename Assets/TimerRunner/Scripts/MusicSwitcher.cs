using UnityEngine;

public class MusicSwitcher : MonoBehaviour
{
    public AudioClip level1And2Music;
    public AudioClip level3Music;

    private AudioSource audioSource;
    private float timer = 0f;
    private bool hasSwitched = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = level1And2Music;
        audioSource.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!hasSwitched && timer >= 180f) // After 180 seconds
        {
            audioSource.clip = level3Music;
            audioSource.Play();
            hasSwitched = true;
        }
    }
}