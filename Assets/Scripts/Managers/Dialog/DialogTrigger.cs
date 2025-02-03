using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogSegment[] DialogSegments;

    public void TriggerDialog()
    {
        // DialogManager dialogManager = GameObject.Find("DialogBox").GetComponent<DialogManager>();
        DialogManager dialogManager = FindObjectOfType<DialogManager>(true);
        Debug.Log(dialogManager);
        dialogManager?.StartDialog(DialogSegments);
    }
}
