using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.UI;
using UnityEngine.Events;
using PaintIn3D;




public class UiButtonColors : HyperSceneObj
{
    public ButtonCont[] ColourButtons;
    public ParticleSystem SprayParticle;
    public P3dPaintSphere PaintCont;
    public override void Init()
    {
        base.Init();

        foreach (var item in ColourButtons)
        {
            item.ClickEvent.AddListener(ChangeSprayColour);
        }

        ChangeSprayColour(ColourButtons[0].ThisButton);
    }

    private void ChangeSprayColour(Button clickedbutton)
    {
        Debug.Log("ColourChanged");
        PaintCont.Color = clickedbutton.colors.normalColor;
        SprayParticle.startColor = clickedbutton.colors.normalColor;
        (PlayerCont.Instance as PlayerCont).SetColor( clickedbutton.colors.normalColor);
    }
    
    
}
