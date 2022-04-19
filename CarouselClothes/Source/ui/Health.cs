using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;

public class EventHealthPercentage : UnityEvent<float> { }

public class Health : HyperSceneObj
{
    public UIHealthBar UIBar;
    [HideInInspector] public UnityEvent EventOnShot = new UnityEvent();
    [HideInInspector] public UnityEvent EventKilled = new UnityEvent();
    public int HealtAmount;
    
    private int shotCounter = 0;

    public override void Init()
    {
        base.Init();

        ResetBar();

    }

    public void ResetBar()
    {
        shotCounter = HealtAmount;
        if (UIBar)
        {
            UIBar.SetLineStep(HealtAmount);
            UIBar.SetProgress(1);
        }
    }

    [HideInInspector] public EventHealthPercentage EventHealthPerc = new EventHealthPercentage();
    public bool Shot(int _amount)
    {

        bool isKilled = false;
        int amount = Mathf.Min(_amount, shotCounter);
        shotCounter -= amount;
        if (shotCounter > 0)
        {
           
           
            float ratio = ((float) shotCounter / (float) HealtAmount);
            UIBar.SetProgress(ratio);
            //Debug.Log("shotCounter " + shotCounter  +  " ratio " + ratio);
            EventOnShot.Invoke();
            EventHealthPerc.Invoke(ratio * 100);

        }
        else
        {
            EventKilled.Invoke();
            isKilled = true;
        }

        return isKilled;
    }

    public void SetView(bool isOpen)
    {
        // isOpened = isOpen;
        UIBar.gameObject.SetActive(isOpen);
    }


    

}
