using HighlightPlus;
using Ruckcat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;
using Char = Ruckcat.Char;
using Touch = Ruckcat.Touch;
using System;
using System.Collections;
using UnityEngine.Events;


public class Enemy : Char
{
    
    public override void Init()
    {
        base.Init();

    }


    public override void OnDeSpawn()
    {
        base.OnDeSpawn();

    }

    public void CustomEnemyInit()
    {
        
    }

    public override void StartGame()
    {
        base.StartGame();
    }

    public override void Update()
    {
        base.Update();

    }

    public override void FixedUpdate()
    {
        //base.FixedUpdate();
        //animator.SetFloat("Speed", GetVelocity().magnitude*10);
    }


    public override void OnTouchChild(Ruckcat.Touch touch)
    {
        base.OnTouchChild(touch);
    }

    public override void OnTouch(Touch touch) { base.OnTouch(touch); }
    public override void OnContact(ContactInfo contact)
    {
        base.OnContact(contact); 
    }
    public override void OnContactChild(ContactInfo contact) { base.OnContactChild(contact); }

    #region EndLevel

    protected override void GameOver(GameResult result) { base.GameOver(result); }



    bool isEnded;
    private void LevelStatus(GameStatus _status)
    {
        if (isEnded) return;
        //Debug.Log("Status Event:  " + _status);
        if (_status == GameStatus.GAMEOVER_SUCCEED)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Sad");
            isEnded = true;
        }
        if (_status == GameStatus.GAMEOVER_FAILED)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Dance");
            isEnded = true;
        }
    }

    #endregion


}