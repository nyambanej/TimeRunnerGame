using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialDialogue : MonoBehaviour
{
    public GameObject panel;             // The dialogue panel
    public Image npcPortrait;            // NPC image (already assigned)
    public TMP_Text dialogueText;        // Text field for the dialogue
    public string[] dialogueLines;       // Array of messages

    private int currentIndex = 0;

    void Start()
    {
        if (!PlayerPrefs.HasKey("HasPlayed"))
        {
            panel.SetActive(true);
            Time.timeScale = 0f;
            ShowCurrentLine();
        }
        else
        {
            panel.SetActive(false);
        }
    }

    void Update()
    {
        // Still allow space/click even when time is paused
        if (panel.activeSelf && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            AdvanceDialogue();
        }
    }

    void ShowCurrentLine()
    {
        if (currentIndex < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentIndex];
        }
    }

    void AdvanceDialogue()
    {
        currentIndex++;
        if (currentIndex >= dialogueLines.Length)
        {
            panel.SetActive(false);
            Time.timeScale = 1f; 
            PlayerPrefs.SetInt("HasPlayed", 1);
        }
        else
        {
            ShowCurrentLine();
        }
    }
}
