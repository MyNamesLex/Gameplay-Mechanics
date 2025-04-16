using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PhoneScreen : MonoBehaviour
{
    public List<GameObject> Pages;
    public List<GameObject> PageBackground;
    public int currentpage;
    Vector2 FirstScreenPress;
    Vector2 SecondScreenPress;
    Vector2 CurrentSwipe;
    int PhoneScreenMask;
    public Animator anim;


    public void Start()
    {
        PhoneScreenMask = LayerMask.NameToLayer("PhoneScreen");
    }

    public void Update()
    {
        Swipe();
    }

    public bool IsMouseOverScreen()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raycastresults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastresults);

        for (int index = 0; index < raycastresults.Count; index++)
        {
            RaycastResult curRaysastResult = raycastresults[index];

            if (curRaysastResult.gameObject.layer == PhoneScreenMask)
            {
                return true;
            }
        }

        return false;
    }

    public void Swipe()
    {
        if(IsMouseOverScreen() == false || anim.GetBool("isShowing") == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            FirstScreenPress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
            SecondScreenPress = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            CurrentSwipe = new Vector2(SecondScreenPress.x - FirstScreenPress.x, SecondScreenPress.y - FirstScreenPress.y);

            CurrentSwipe.Normalize();

            if (CurrentSwipe.x < 0 && CurrentSwipe.y > -0.5f && CurrentSwipe.y < 0.5f)
            {
                if(currentpage + 1 != Pages.Count + 1)
                {
                    currentpage++;
                    ShowCurrentPage();
                }
                Debug.Log("right");
            }

            if (CurrentSwipe.x > 0 && CurrentSwipe.y > -0.5f && CurrentSwipe.y < 0.5f)
            {
                if (currentpage - 1 != 0)
                {
                    currentpage--;
                    ShowCurrentPage();
                }
                Debug.Log("left");
            }
        }
    }

    public void ShowCurrentPage()
    {
        switch(currentpage)
        {
            case 1:
                Pages[0].SetActive(true);
                Pages[1].SetActive(false);
                Pages[2].SetActive(false);
                PageBackground[0].SetActive(true);
                PageBackground[1].SetActive(false);
                PageBackground[2].SetActive(false);
                break;

            case 2:
                Pages[0].SetActive(false);
                Pages[1].SetActive(true);
                Pages[2].SetActive(false);
                PageBackground[0].SetActive(false);
                PageBackground[1].SetActive(true);
                PageBackground[2].SetActive(false);
                break;

            case 3:
                Pages[0].SetActive(false);
                Pages[1].SetActive(false);
                Pages[2].SetActive(true);
                PageBackground[0].SetActive(false);
                PageBackground[1].SetActive(false);
                PageBackground[2].SetActive(true);
                break;
        }
    }
}
