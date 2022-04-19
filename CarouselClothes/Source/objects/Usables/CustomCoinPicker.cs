using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using UnityEngine;

public class CustomCoinPicker : CoinPickup
{
   public GameObject Arrow;
   public ParticleSystem FxGlow;
   public int DownShowUpperLevel = 1;

   public override void Init()
   {
      base.Init();

      {
         if(Arrow) Arrow.SetActive(false);
      }
   }

   protected override void OnDropCompleted()
   {
      base.OnDropCompleted();
      
      if(FxGlow) FxGlow.Play();
      if (LevelCont.Instance.CurrLevel <= DownShowUpperLevel)
      {
         if(Arrow) Arrow.SetActive(true);
      }
   }

   protected override void OnPickCompleted()
   {
      base.OnPickCompleted();
      
      if(FxGlow) FxGlow.gameObject.SetActive(false);
   }


   
}
