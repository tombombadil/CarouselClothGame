using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;
using TMPro;

public class PageGameOver : HyperPageGameOver
{
    private Emoji emoji;
    private GameResult currResult;

    public override void Init()
    {
        base.Init();
    }


    public override void SetGameResult(GameResult _result)
    {
        if (currResult != _result)
        {
            base.SetGameResult(_result);

            if (_result == GameResult.SUCCEED)
            {
                emoji = StateSucc.GetComponentInChildren<Emoji>();
                if (emoji) emoji.ShowRandom();
            }
            else if (_result == GameResult.FAILED)
            {
                emoji = StateFail.GetComponentInChildren<Emoji>();
                if (emoji) emoji.ShowRandom();
            }


            SetText("TxTLevel", "Level " + LevelCont.Instance.CurrLevel.ToString());
            currResult = _result;
        }
    }
}