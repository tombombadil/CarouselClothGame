using System;
using System.Collections;
using System.Collections.Generic;
using LiquidVolumeFX;
using UnityEngine;
using Ruckcat;
using Sirenix.OdinInspector;
using UnityEngine.Events;

[System.Serializable]
public class ClothSlot
{
    public bool isSlotFull;
    public Cloth thecloth;
    public Collider SlotCollider;
}
public class Eventslot: UnityEvent<ClothSlot> { }

public class CarouselCont : HyperSceneObj
{
    public float TimeOfOpenPerSlot = 8f ;
    public float SpeedMomentumIncrease = 0.5f;
    public float SpeedMomentumDecrease = -0.5f;
    public ClothSlot[] AllSlots;
    [FoldoutGroup("CarouselSettings")] public GameObject RotateObj;
    [FoldoutGroup("CarouselSettings")] public float CarouselNormSpeed;
    [FoldoutGroup("CarouselSettings")] public float CarouselSlowSpeed;
    [FoldoutGroup("CarouselSettings")] public float CarouselFastSpeed;
    [FoldoutGroup("CarouselSettings")] public Collider StartPoint;
    [FoldoutGroup("CarouselSettings")] public Collider EndPoint;
    [FoldoutGroup("CarouselSettings")] public Collider ShowPoint;
    [FoldoutGroup("CarouselSettings")] public Collider ClosePoint;
    [FoldoutGroup("CarouselSettings")] public Collider SlowAreaStart;
    [FoldoutGroup("CarouselSettings")] public Collider SlowAreaEnd;
    public Eventslot EVENT_EMPTY_SLOT = new Eventslot();
    public Eventslot EVENT_END_SLOT = new Eventslot();
    private float  timeCounter = 0;
    private int  openClothCounter = 0;
    private float cspeed;


    public override void Init()
    {
        base.Init();
        cspeed = CarouselNormSpeed;
        cSpeedMomentum = cspeed;
        foreach (var item in AllSlots)
        {
            item.SlotCollider.gameObject.SetActive(false);
        }

        openSlot(0);
    }
    public override void StartGame()
    {
        base.StartGame();
        cspeed = CarouselNormSpeed;
        
        if (CoreInputCont.Instance) CoreInputCont.Instance.EventTouch.AddListener(onTouch);
    }

    private  void onTouch(Ruckcat.Touch _touch)
    {
        if(_touch.Phase == TouchPhase.Began)
        MakeSlowCSepeed(true);
        if(_touch.Phase == TouchPhase.Ended)
            MakeSlowCSepeed(false);
    }

    public override void Update()
    {
        base.Update();

        if (IsGameStarted)
        {
            TurnCarousel();
        }


        if (timeCounter > TimeOfOpenPerSlot)
        {
            if (openClothCounter < AllSlots.Length)
            {
                openSlot(openClothCounter);
               
            }

            TimeOfOpenPerSlot = 0;
        }
        timeCounter += Time.deltaTime;

    }

    private void openSlot(int index)
    {
        if (index < AllSlots.Length)
        {
            AllSlots[index].SlotCollider.gameObject.SetActive(true);
            openClothCounter++;
        }
    }

    
    public override void OnContactChild(ContactInfo contact)
    {
        base.OnContactChild(contact);


        if(contact.Type == ContactType.TRIGGER_ENTER)
        {
            if (!contact.Target.CompareTag("CarouselPoints") && !contact.Target.CompareTag("ShowP") && !contact.Target.CompareTag("CloseP")) return;
            if(contact.Target.GetComponent<Collider>() == StartPoint && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                if (!thisSlot.isSlotFull)
                {
                    //Debug.Log("StartPoint Triggered   " + thisSlot.SlotCollider.name);
                    EVENT_EMPTY_SLOT.Invoke(thisSlot);
                }
            }

            if(contact.Target.GetComponent<Collider>() == EndPoint && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                if (thisSlot.isSlotFull)
                {
                   // Debug.Log("EnPoint Triggered   " + thisSlot.SlotCollider.name);
                    EVENT_END_SLOT.Invoke(thisSlot);
                }              
            }

            if (contact.Target.GetComponent<Collider>() == ShowPoint && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                if (thisSlot.isSlotFull)
                {
                    thisSlot.thecloth.SelectPrefab();
                    thisSlot.thecloth.SetPrefab(true);
                    Debug.Log("ShowP Triggered   " + thisSlot.SlotCollider.name);
                }
                
            }

            if (contact.Target.GetComponent<Collider>() == ClosePoint && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                if (thisSlot.isSlotFull)
                {
                    thisSlot.thecloth.SetPrefab(false);
                    Debug.Log("CloseP Triggered   " + thisSlot.SlotCollider.name);
                }
            }
            
            
            if (contact.Target.GetComponent<Collider>() == SlowAreaStart && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                if (thisSlot.isSlotFull)
                {
                    cspeed = CarouselSlowSpeed;
                    inSlowArea = true;
                }
                else
                {
                    cspeed = CarouselFastSpeed;
                    inSlowArea = false;
                }
            }

            if (contact.Target.GetComponent<Collider>() == SlowAreaEnd && contact.Other.CompareTag("Slot"))
            {
                ClothSlot thisSlot = GetSlot(contact.Other);
                cspeed = CarouselFastSpeed;
                inSlowArea = false;
            }
        }
    }

    private ClothSlot GetSlot(Collider checkcol)
    {
        ClothSlot tocheck = null;
        foreach (var item in AllSlots)
        {
            if(item.SlotCollider == checkcol)
            {
                tocheck = item;
                break;
            }
        }
        return tocheck;
    }

 
    private float  _cSpeed = 0;
    private float cSpeedMomentum = 0;
    private float cSpeedMomentumValue = 0;
    private bool inSlowArea;

    private void TurnCarousel()
    {
        Debug.Log("cspeed " + cspeed);
        cSpeedMomentum += cSpeedMomentumValue;
        cSpeedMomentum = Mathf.Clamp(cSpeedMomentum, 0.6f, cspeed);
         _cSpeed = Mathf.Lerp(_cSpeed, cSpeedMomentum, Time.deltaTime);
        RotateObj.transform.eulerAngles += new Vector3(0, _cSpeed * Time.deltaTime ,0);
    }

    public void MakeSlowCSepeed(bool isActive)
    {
        
            if(isActive)
                 cSpeedMomentumValue = SpeedMomentumDecrease;
            else
                cSpeedMomentumValue = SpeedMomentumIncrease;
      
    }
    
    
   
    
    



}
