using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC : MonoBehaviour
{
    public DialogueManager dm;
    public NavMeshAgent nma;
    public World world;

    [Header("Dialogue")]
    public string NPCName;
    public List<string> NPCDialogue;

    [Header("NPC Move To Positions")]
    public List<GameObject> WhereToMove;
    public ArrayList a;

    [Range(0,24)]
    public List<int> WhenToMove;

    [Header("NPC Actions")]
    public TextMeshPro ActivityText;
    [Range(0, 24)]
    public List<int> ActivitesAtTime;
    public List<string> ActivityName;

    [Header("Booleans")]
    public bool ConversationRange = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && ConversationRange == true)
        {
            DialogueHandler();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            ConversationRange = true;
            Debug.Log(other.name + " Has Hit " + name);
            dm.ShowText(NPCName, NPCDialogue[dm.CurrentDialogue]);
        }

        if(other.CompareTag("MoveToPos") && WhereToMove[world.CurrentTime].transform.position == other.transform.position)
        {
            nma.ResetPath();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationRange = false;
            Debug.Log(other.name + " Has Left " + name);
            dm.HideText();
        }
    }

    public void DialogueHandler()
    {
        if (dm.CurrentDialogue >= NPCDialogue.Count)
        {
            dm.HideText();
        }
        else
        {
            dm.ShowText(NPCName, NPCDialogue[dm.CurrentDialogue]);
        }
    }

    public void MoveToObject()
    {
        if (WhereToMove[world.CurrentTime] != null)
        {
            nma.destination = WhereToMove[world.CurrentTime].transform.position;
            transform.LookAt(nma.destination);
        }
    }

    public void DoActivity()
    {
        ActivityText.text = ActivityName[world.CurrentTime];
        Debug.Log("DoActivity() function running");
    }

    public void CancelActivity()
    {
        ActivityText.text = " ";
        Debug.Log("CancelActivity() function running");
    }
}
