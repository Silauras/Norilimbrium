using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogBox : MonoBehaviour
{
    public DialogSegment[] DialogSegments;
    [Space]
    public Image DialogSpeakerPortrait;
    public Image DialogSkipIndicator;
    [Space]
    public TextMeshProUGUI DialogSpeakerName;
    public TextMeshProUGUI DialogText;
    [Space]
    public float TextSpeed;

    private int DialogIndex;
    private bool CanContinue;

    // Start
    void Start()
    {
        SetStyle(DialogSegments[0].Speaker);
        StartCoroutine(PlayDialog(DialogSegments[0].Dialog));
    }

    // Update
    void Update() 
    {
        DialogSkipIndicator.enabled = true;
        if (Input.GetKeyDown(KeyCode.Space) && CanContinue)
        {
            DialogIndex++;
            if (DialogIndex == DialogSegments.Length) {
                gameObject.SetActive(false);
                return;
            }

            SetStyle(DialogSegments[DialogIndex].Speaker);
            StartCoroutine(PlayDialog(DialogSegments[DialogIndex].Dialog));
        }
    }


    void SetStyle(DialogSpeaker speaker) 
    {
        if (speaker.Portrait is null) 
        {
            DialogSpeakerPortrait.color = new Color(0, 0, 0, 0);
        } 
        else 
        {
            DialogSpeakerPortrait.sprite = speaker.Portrait;
            DialogSpeakerPortrait.color = Color.white;
        }

        DialogSpeakerName.color = speaker.NameColor;
        DialogSpeakerName.SetText(speaker.Name);
    }

    IEnumerator PlayDialog(string dialog) 
    {
        CanContinue = false;
        DialogText.SetText(string.Empty);

        for (int i = 0; i < dialog.Length; i++) 
        {
            DialogText.text += dialog[i];
            yield return new WaitForSeconds(1f / TextSpeed);
        }

        CanContinue = true;
    }
}

[System.Serializable]
public class DialogSegment
{
    public string Dialog;
    public DialogSpeaker Speaker;
}
