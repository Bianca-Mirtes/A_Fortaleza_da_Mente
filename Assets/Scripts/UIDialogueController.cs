using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueController : MonoBehaviour
{
    private Image spriteTalker;
    private TextMeshProUGUI nameTalker;
    private TextMeshProUGUI messageTalker;

    private void Awake()
    {
        DialogueController.NewTalker += NewTalker;
        DialogueController.ShowMessage += ShowMessage;
        DialogueController.ResetDialogue += ResetDialogue;
        DialogueController.StateDialogueController += StateDialogueController;
    }

    private void OnDestroy()
    {
        DialogueController.NewTalker -= NewTalker;
        DialogueController.ShowMessage -= ShowMessage;
        DialogueController.ResetDialogue -= ResetDialogue;
        DialogueController.StateDialogueController -= StateDialogueController;
    }

    void Start()
    {
        spriteTalker = transform.GetChild(2).GetComponent<Image>();
        nameTalker = transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        messageTalker = transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        gameObject.SetActive(false);
    }

    public void NewTalker(Dialogue talkerInformations)
    {
        spriteTalker.sprite = talkerInformations.character.spritePerson;
        nameTalker.text = talkerInformations.character.name;
    }

    public void ResetDialogue()
    {
        messageTalker.text = string.Empty;
    }

    public void ShowMessage(string message)
    {
        messageTalker.text = message;
    }

    public void StateDialogueController(bool state)
    {
        gameObject.SetActive(state);
    }

}
