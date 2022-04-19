using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using UnityEngine;
using UnityEngine.Events;

public class UIHealthBar : CoreUI
{
   public RectTransform Bar;
   public RectTransform SeperateLine;
   public override void Init()
   {
      base.Init();

   }

    private void Awake()
    {
        SetProgress(1);
    }

    public override void StartGame()
    {
        base.StartGame();
    }

    public void SetLineStep(int stepCount)
    {
       
       if (SeperateLine)
       {
          float w = (1f / (float)stepCount);
          int count = stepCount + 1;
          for (int i = 0; i < count; i++)
          {
             GameObject go = Instantiate(SeperateLine.gameObject, Vector3.zero, Quaternion.identity, SeperateLine.parent.transform);
             RectTransform item = go.GetComponent<RectTransform>();
             float x = i * w;
             item.anchorMin = new Vector2(x, 0.5f);
             item.anchorMax = new Vector2(x, 0.5f);
             item.localPosition = Vector3.zero;
          }
          
          SeperateLine.gameObject.SetActive(false);
       }
    }

    public void SetProgress(float ratio) //ratio : 0-1
   {
      Vector3 s = Bar.transform.localScale;
      s.x = Mathf.Clamp(ratio, 0, 1);
      Bar.transform.localScale = s;
   }
}
