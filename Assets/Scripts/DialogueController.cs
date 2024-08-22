using System.Collections;
using UnityEngine;

public class DialogueController: MonoBehaviour
{
    public static DialogueController instance;
    private DialogueContainer dialogueContainer;
    private bool endCurrentTalk = true;
    private bool buttonWasClicked = false;

    public static event System.Action<Dialogue> NewTalker;
    public static event System.Action ResetDialogue;
    public static event System.Action<string> ShowMessage;
    public static event System.Action<bool> StateDialogueController;

    private void Awake()
    {
        instance = this;
    }
    public void StartConversation(DialogueContainer container)
    {
        dialogueContainer = container;
        StartCoroutine(InitDialogue());
        StateDialogueController?.Invoke(true);
    }

    public IEnumerator InitDialogue()
    {
        foreach(var character in dialogueContainer.dialogues)
        {
            ResetDialogue?.Invoke();
            NewTalker?.Invoke(character);
            StartCoroutine(ShowDialogue(character.messages));

           yield return new WaitUntil(() => endCurrentTalk);
        }
        StateDialogueController?.Invoke(false);
    }

    public IEnumerator ShowDialogue(string[] messages)
    {
        endCurrentTalk = false;
        foreach (var message in messages)
        {
            ShowMessage?.Invoke(message);
            buttonWasClicked = false;
            yield return new WaitUntil(() => buttonWasClicked);
        }

        endCurrentTalk = true;
    }

    public void ButtonWasClicked()
    {
        buttonWasClicked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
