using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Popup : MonoBehaviour
{
    public float Timer;
    public TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Popups(string txt)
    {
        StartCoroutine(PopupFunction(txt));
    }

    private IEnumerator PopupFunction(string txt)
    {
        Text.text = txt;
        yield return new WaitForSeconds(Timer);
        Text.text = "";
    }
}
