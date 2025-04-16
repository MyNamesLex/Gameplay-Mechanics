using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AppTransition : MonoBehaviour
{
    public Animator anim;
    public bool isTransitionShowing;
    public Image img;
    public GameObject InAppApps;
    public PhoneHandler handler;
    public void Transition()
    {
        if(isTransitionShowing)
        {
            anim.SetBool("isShowing", false);
            isTransitionShowing = false;
            StartCoroutine(TimeUntilinAppsAppDisappear());
        }
        else
        {
            anim.SetBool("isShowing", true);
            isTransitionShowing = true;
            InAppApps.SetActive(true);
        }
    }

    public void AppBackgroundImage(Image Background)
    {
        img.color = Background.color;
        img.sprite = Background.sprite;
    }

    public void InAppAppsFunction(GameObject InAppAppsNew)
    {
        for(int i = 0; i < handler.AllInAppApps.Count; i++)
        {
            if(handler.AllInAppApps[i].gameObject.activeInHierarchy)
            {
                handler.AllInAppApps[i].gameObject.SetActive(false);
            }
        }
        InAppApps = InAppAppsNew;
    }

    public IEnumerator TimeUntilinAppsAppDisappear()
    {
        yield return new WaitForSeconds(0.8f);
        InAppApps.SetActive(false);
        yield return null;
    }
}
