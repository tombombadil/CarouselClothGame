using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;
using TMPro;
using Touch = Ruckcat.Touch;

public class PageGame : HyperPageGame
{
    public RectTransform CenterCircle;
    public RectTransform BoundsCircle;

    private Vector2 startPoint;
    public override void Init()
    {
        base.Init();
        CoreInputCont.Instance.EventTouch.AddListener(onInputHandler);
    }

    public override void Update()
    {
        base.Update();
        
        
    }

    public Rect GetInputBounds()
    {
        Rect rect = new Rect(0, 0, 1, 1);
        if(BoundsCircle)
            rect = (BoundsCircle.transform as RectTransform).rect;

        return rect;
    }

    public void UpdateJoystick(Vector2 centerCirclePos, Vector2 pos)
    {
        CenterCircle.transform.localPosition = new Vector3(centerCirclePos.x, centerCirclePos.y, 0);

        CenterCircle.transform.parent.position = pos;
    }
    
    

    private void onInputHandler(Touch touch)
    {
        
    }

    public void ShowJoystick(bool isShow)
    {
        CenterCircle.gameObject.SetActive(isShow);
        BoundsCircle.gameObject.SetActive(isShow);
    }


}

