using Ink.Runtime;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private string preferedLanguage;

    private TextAsset localisationInkJSON;

    private const string CONVERSATION_FILE_PATH_PATTERN = @"^Assets/Dialogues/[a-zA-Z\-]+/([a-zA-Z\/\-._ ]+)$";
    private const string CONVERSATION_FILE_PATH_TEMPLATE = @"Assets/Dialogues/{0}/{1}";

    private void Awake()
    {
        if (preferedLanguage is not null)
        {
            string selectedAssetPath = AssetDatabase.GetAssetPath(inkJSON);

            var match = Regex.Match(selectedAssetPath, CONVERSATION_FILE_PATH_PATTERN);
            if (match.Success)
            {
                string realtiveFilePath = match.Groups[1].Value;
                string localizationAssetPath = string.Format(CONVERSATION_FILE_PATH_TEMPLATE, preferedLanguage, realtiveFilePath);
                localisationInkJSON = AssetDatabase.LoadAssetAtPath<TextAsset>(localizationAssetPath);
            }
        }
    }

    public void TriggerDialogue()
    {

        DialogueManager dialogueManager = FindFirstObjectByType<DialogueManager>(FindObjectsInactive.Include);
        dialogueManager.StartDialogue(localisationInkJSON is null ? inkJSON : localisationInkJSON);
    }
}
