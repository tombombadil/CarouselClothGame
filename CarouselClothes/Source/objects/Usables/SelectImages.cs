using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.UI;

public enum FaceCond
{
    Win,
    Loose
}
public enum Faces
{
    G1,
    G2,
    G3
}
[System.Serializable]
public class Face
{
    public Faces _Setup;
    public Sprite Happy;
    public Sprite Sad;
}
public class SelectImages : HyperSceneObj
{
    public Image ImageRef;

    public Face[] Setups;



    public void SetImagePoint(float _p,Faces _fc)
    {
        Face _f = GetSetup(_fc);
        if(_p >= 0.9f)
        {
            SetImage(_f,FaceCond.Win);
        }
        else if(_p >= 0.3f && _p < 0.9f)
        {
            SetImage(_f, FaceCond.Win);
        }
        else if (_p < 0.3f)
        {
            SetImage(_f, FaceCond.Loose);
        }
    }



    private void SetImage(Face _f,FaceCond _c)
    {
        if (_c == FaceCond.Win) ImageRef.sprite = _f.Happy;
        else if (_c == FaceCond.Loose) ImageRef.sprite = _f.Sad;
    }

    private Face GetSetup(Faces _f)
    {
        Face _to = null;
        foreach (var item in Setups)
        {
            if (_f == item._Setup) _to = item;
        }
        return _to;
    }

}
