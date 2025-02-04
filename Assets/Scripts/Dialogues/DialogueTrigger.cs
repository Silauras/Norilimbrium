using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueSegment[] DialogueSegments;

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>(true);
        dialogueManager.StartDialogue(DialogueSegments);
    }
}
