using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using UnityEngine;

public class DebugCustomer : HyperSceneObj, IClothOwner
{
    // public Transform ClothParent;
    // public GameObject debugprefab;

    public override void Init()
    {
        base.Init();

      // Cloth cloth =  ClothCont.Instance.GetCloth(this);
    }

    public Transform GetClothParent()
    {
        return this.transform;
    }

   
}
