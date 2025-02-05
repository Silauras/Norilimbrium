using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System.Threading;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueSpeakerName;
    [SerializeField] private GameObject dialogueHistoryContent;
    [SerializeField] private Animator portraitAnimator;
    [Header("Prefabs")]
    [SerializeField] private GameObject dialogueReplicaPrefab;
    [SerializeField] private GameObject dialogueChoicePanelPrefab;
    [SerializeField] private GameObject dialogueChoiceButtonPrefab;
    [SerializeField] private GameObject dialogueContinueIndicatorPrefab;
    [SerializeField] private GameObject dialogueExitIndicatorPrefab;
    [Header("Dialogue settings")]
    [SerializeField] private float textSpeed;

    private Story currentStory;
    private GameObject currentChoicePanel;
    private GameObject currentIndicator;
    public bool dialogueIsPlaying { get; private set; }

    private const string SPEAKER_TAG = "Speaker";
    private const string PORTRAIT_TAG = "Portrait";

    private const string TRUE_TAG_VALUE = "true";
    private const string FALSE_TAG_VALUE = "false";

    private void Awake()
    {
        if (instance is not null)
        {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
    }

    private void Update() 
    {
        if (!dialogueIsPlaying)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && currentStory.currentChoices.Count == 0)
        {
            ContinueStory();
        }
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);

        dialogueSpeakerName.text = "???";

        ContinueStory();
    }

    private IEnumerator ExitDialogue()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        foreach (Transform child in dialogueHistoryContent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            SpeakerMeta speaker = HandleTags(currentStory.currentTags);

            portraitAnimator.Play(speaker.Portrait);
            dialogueSpeakerName.text = speaker.Name;

            DisplayReplica(speaker, text);

            DisplayChoicesOrIndicator();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private void ContinueStoryAfterChoice()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();
            SpeakerMeta speaker = HandleTags(currentStory.currentTags);

            DisplayReplica(speaker, text);

            ContinueStory();
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private void DisplayReplica(SpeakerMeta speaker, string text)
    {
        GameObject replica = Instantiate(dialogueReplicaPrefab);
        replica.transform.SetParent(dialogueHistoryContent.transform, false);
        TextMeshProUGUI replicaText = replica.GetComponent<TextMeshProUGUI>();

        replicaText.text = $"{speaker.Name} — {text}";
    }

    private void DisplayChoicesOrIndicator()
    {
        if (currentChoicePanel is not null)
        {
            Destroy(currentChoicePanel);
            currentChoicePanel = null;
        }

        if (currentIndicator is not null)
        {
            Destroy(currentIndicator);
            currentIndicator = null;
        }

        List<Choice> choicesToDisplay = currentStory.currentChoices;

        if (choicesToDisplay.Count == 0)
        {
            if (!currentStory.canContinue)
            {
                currentIndicator = Instantiate(dialogueExitIndicatorPrefab);
                currentIndicator.transform.SetParent(dialogueHistoryContent.transform, false);
            }
            else
            {
                currentIndicator = Instantiate(dialogueContinueIndicatorPrefab);
                currentIndicator.transform.SetParent(dialogueHistoryContent.transform, false);
            }
            return;
        }

        currentChoicePanel = Instantiate(dialogueChoicePanelPrefab);
        currentChoicePanel.transform.SetParent(dialogueHistoryContent.transform, false);

        int index = 0;
        foreach (Choice choiceToDisplay in choicesToDisplay)
        {
            int currentIndex = index;

            GameObject newChoice = Instantiate(dialogueChoiceButtonPrefab);
            newChoice.GetComponent<Button>().onClick.AddListener(() => MakeChoice(currentIndex));
            newChoice.transform.SetParent(currentChoicePanel.transform, false);
            newChoice.GetComponentInChildren<TextMeshProUGUI>().text = choiceToDisplay.text;

            index++;
        }

        StartCoroutine(SelectFirstChoice());
    }

    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStoryAfterChoice();
    }

    private IEnumerator SelectFirstChoice()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        Button[] choiceList = currentChoicePanel.GetComponentsInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(choiceList[0].gameObject);
    }

    // returns speaker name
    private SpeakerMeta HandleTags(List<string> tags)
    {
        SpeakerMeta speaker = new SpeakerMeta();

        foreach (string tag in tags)
        {
            string[] splitTag = tag.Split(':');

            if (splitTag.Length > 2)
            {
                Debug.LogWarning($"Tag could not be appropriately parsed: {tag}");
            }

            if (splitTag.Length == 1)
            {
                splitTag = splitTag.Append(TRUE_TAG_VALUE).ToArray();
            }

            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey)
            {
                case SPEAKER_TAG:
                    speaker.Name = tagValue;
                    break;
                case PORTRAIT_TAG:
                    speaker.Portrait = tagValue;
                    break;
                default:
                    Debug.LogWarning($"Tag came in but is not currently being handled: {tag}");
                    break;
            }
        }

        return speaker;
    }
}
