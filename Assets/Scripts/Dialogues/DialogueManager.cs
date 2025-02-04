using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Space]
    public Image DialogueSpeakerPortrait;
    public Image DialogueSkipIndicator;
    [Space]
    public TextMeshProUGUI DialogueSpeakerName;
    public TextMeshProUGUI DialogueText; 
    [Space]
    public float TextSpeed;

    private DialogueSegment[] dialogueSegments;
    private int dialogueIndex;
    private bool canContinue;

    // Start
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update
    void Update() 
    {
        if (Input.GetMouseButtonDown(0) && canContinue)
        {
            dialogueIndex++;
            if (dialogueIndex == dialogueSegments.Length) {
                dialogueSegments = null;
                gameObject.SetActive(false);
                return;
            }

            SetStyle(dialogueSegments[dialogueIndex].Speaker);
            StartCoroutine(PlayDialogue(dialogueSegments[dialogueIndex].Dialogue));
        }
    }

    public void StartDialogue(DialogueSegment[] newDialogueSegments) {
        if (newDialogueSegments.Length == 0)
        {
            return;
        }

        // Set
        dialogueSegments = newDialogueSegments;

        // Prepare
        dialogueIndex = 0;
        SetStyle(dialogueSegments[0].Speaker);
        gameObject.SetActive(true);

        // Start
        StartCoroutine(PlayDialogue(dialogueSegments[0].Dialogue));

        // Debug
        Debug.Log("Starting dialog with init: " + dialogueSegments[0].Speaker.Name);
    }

    void SetStyle(DialogueSpeaker speaker) 
    {
        if (speaker.Portrait is null) 
        {
            DialogueSpeakerPortrait.color = new Color(0, 0, 0, 0);
        } 
        else 
        {
            DialogueSpeakerPortrait.sprite = speaker.Portrait;
            DialogueSpeakerPortrait.color = Color.white;
        }

        DialogueSpeakerName.color = speaker.NameColor;
        DialogueSpeakerName.SetText(speaker.Name);
    }

    IEnumerator PlayDialogue(string dialogue) 
    {
        canContinue = false;
        DialogueSkipIndicator.enabled = false;
        DialogueText.SetText(string.Empty);

        for (int i = 0; i < dialogue.Length; i++) 
        {
            DialogueText.text += dialogue[i];
            yield return new WaitForSeconds(1f / TextSpeed);
        }

        canContinue = true;
        DialogueSkipIndicator.enabled = true;
    }
}
