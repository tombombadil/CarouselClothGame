using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.UI;
using UnityEngine.Events;

public class ButtonClickedEvent : UnityEvent<Button> { }


public class ButtonCont : HyperButton
{
    public Button ThisButton;
    public ButtonClickedEvent ClickEvent = new ButtonClickedEvent();
    public override void Init()
    {
        base.Init();
        ThisButton.onClick.AddListener(InvokeButtonEvent);
    }

    private void InvokeButtonEvent()
    {
        Debug.Log("ButtonPressed");
        ClickEvent.Invoke(ThisButton);
    }

    bool isTouched;
    public override void OnTouch(TouchUI info)
    {
        base.OnTouch(info);
        if (!isTouched)
        {
            isTouched = true;
            Debug.Log("ButtonPressed");
            ClickEvent.Invoke(ThisButton);
            (PlayerCont.Instance as PlayerCont).isTouchLocked = true;
            StartCoroutine(TouchLock());
        }
    }

    IEnumerator TouchLock()
    {
        yield return new WaitForSeconds(0.3f);
        isTouched = false;
        (PlayerCont.Instance as PlayerCont).isTouchLocked = false;
    }

}
