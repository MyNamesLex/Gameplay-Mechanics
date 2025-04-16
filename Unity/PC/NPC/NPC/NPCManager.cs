using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public List<NPC> NPCList;

    // Start is called before the first frame update
    [System.Obsolete]
    void Start()
    {
        for(int i = 0; i < transform.GetChildCount(); i++)
        {
            NPCList.Add(transform.GetChild(i).GetComponent<NPC>());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckMovement(int CurrentTime)
    {
        foreach(NPC n in NPCList)
        {
            for(int i = 0; i < n.WhenToMove.Count; i++)
            {
                Debug.Log("CheckMovement() function running");
                if (CurrentTime == n.WhenToMove[i])
                {
                    n.MoveToObject();
                    break;
                }
            }
        }
    }

    public void CheckActivity(int CurrentTime)
    {
        foreach (NPC n in NPCList)
        {
            for (int i = 0; i < n.ActivitesAtTime.Count; i++)
            {
                Debug.Log("CheckActivity() function running");
                if (CurrentTime == n.ActivitesAtTime[i])
                {
                    n.DoActivity();
                    break;
                }
                else
                {
                    n.CancelActivity();
                }
            }
        }
    }

}
