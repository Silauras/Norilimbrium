using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogManager : MonoBehaviour
{
    [Space]
    public Image DialogSpeakerPortrait;
    public Image DialogSkipIndicator;
    [Space]
    public TextMeshProUGUI DialogSpeakerName;
    public TextMeshProUGUI DialogText; 
    [Space]
    public float TextSpeed;

    private DialogSegment[] _dialogSegments;
    private int _dialogIndex;
    private bool _canContinue;

    // Start
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update
    void Update() 
    {
        if (Input.GetMouseButtonDown(0) && _canContinue)
        {
            _dialogIndex++;
            if (_dialogIndex == _dialogSegments.Length) {
                _dialogSegments = null;
                gameObject.SetActive(false);
                return;
            }

            SetStyle(_dialogSegments[_dialogIndex].Speaker);
            StartCoroutine(PlayDialog(_dialogSegments[_dialogIndex].Dialog));
        }
    }

    public void StartDialog(DialogSegment[] dialogSegments) {
        if (dialogSegments.Length == 0)
        {
            return;
        }

        // Set
        _dialogSegments = dialogSegments;

        // Prepare
        _dialogIndex = 0;
        SetStyle(_dialogSegments[0].Speaker);
        gameObject.SetActive(true);

        // Start
        StartCoroutine(PlayDialog(_dialogSegments[0].Dialog));

        // Debug
        Debug.Log("Starting dialog with init: " + _dialogSegments[0].Speaker.Name);
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
        _canContinue = false;
        DialogSkipIndicator.enabled = false;
        DialogText.SetText(string.Empty);

        for (int i = 0; i < dialog.Length; i++) 
        {
            DialogText.text += dialog[i];
            yield return new WaitForSeconds(1f / TextSpeed);
        }

        _canContinue = true;
        DialogSkipIndicator.enabled = true;
    }
}
