using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Message : MonoBehaviour
{
    public TMP_InputField Sender;
    public TMP_InputField SendersMessage;

    public TextMeshProUGUI SenderTextMessageBox;
    public TextMeshProUGUI MessageTextMessageBox;

    public Animator anim;
    public void OnClick()
    {
        if(Sender.text == string.Empty)
        {
            SenderTextMessageBox.text = "Sender Input Field Text Is Null";
        }
        else
        {
            SenderTextMessageBox.text = Sender.text;
        }

        if (SendersMessage.text == string.Empty)
        {
            MessageTextMessageBox.text = "Sender Message Input Field Text Is Null";
        }
        else
        {
            MessageTextMessageBox.text = SendersMessage.text;
        }

        anim.SetBool("Show", true);
        StartCoroutine(HideMessage());
    }

    public IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(2);
        anim.SetBool("Show", false);
        yield return null;
    }
}
