using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerClickHandler
{
    public AppTransition App;
    public Image AppBackground;
    public GameObject InAppApps;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked " + gameObject.name);

        App.GetComponent<AppTransition>().InAppAppsFunction(InAppApps);
        App.GetComponent<AppTransition>().AppBackgroundImage(AppBackground);
        App.GetComponent<AppTransition>().Transition();
    }
}
