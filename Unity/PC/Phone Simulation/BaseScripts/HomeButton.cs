using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HomeButton : MonoBehaviour, IPointerClickHandler
{
    public AppTransition transition;

    public void OnPointerClick(PointerEventData eventData)
    {
        HomeButtonClicked();
    }

    public void HomeButtonClicked()
    {
        if(transition.isTransitionShowing == true)
        {
            transition.Transition();
        }
    }
}
