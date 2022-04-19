using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using TMPro;
using UnityEngine;

public class CharCoin : CoreUI
{
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextCount;
    public override void Init()
    {
        base.Init();
     }

    public void SetCount(int count)
    {
        if (TextCount)
        {
            TextCount.SetText(count.ToString());
        }
    }
}
    



