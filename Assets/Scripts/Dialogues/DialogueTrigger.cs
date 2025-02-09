using Ink.Runtime;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    //private AudioClip audioClips;

    private void Awake()
    {
        //var story = new Story(inkJSON.text);
        //story
    }

    public void TriggerDialogue()
    {
        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(inkJSON);
    }
}
