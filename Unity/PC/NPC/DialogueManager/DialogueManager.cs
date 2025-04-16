using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI DialogueText;
    public TextMeshProUGUI NextDialogueText;
    public int CurrentDialogue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowText(string name, string dialogue)
    {
        NameText.text = name;
        DialogueText.text = dialogue;
        NextDialogueText.gameObject.SetActive(true);
        CurrentDialogue += 1;
    }

    public void HideText()
    {
        NameText.text = " ";
        DialogueText.text = " ";
        NextDialogueText.gameObject.SetActive(false);
        CurrentDialogue = 0;
    }
}
