using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.UI;

public class SelectStar : HyperSceneObj
{
    public Image[] StarImages;

    public void SetStarPoint(float _p)
     {
        if (_p >= 0.9f)
        {
            SetStars(3);
        }
        else if (_p >= 0.3f && _p < 0.9f)
        {
            SetStars(2);
        }
        else if (_p < 0.3f)
        {
            SetStars(1);
        }
    }

    private void SetStars(int _count)
    {
        for (int i = 0; i < StarImages.Length; i++)
        {
            if(i < _count)
            {
                StarImages[i].color = Color.white;
            }
            else
            {
                StarImages[i].color = Color.black;
            }
        }
    }
}
