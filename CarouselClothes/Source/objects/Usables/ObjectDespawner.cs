using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;

public class ObjectDespawner : HyperSceneObj
{
    public void Despawnobj()
    {
        StartCoroutine(Des(this));
    }

    IEnumerator Des(CoreSceneObject _obj)
    {
        yield return new WaitForSeconds(2f);
        CoreSceneCont.Instance.DeSpawnItem(_obj);
    }
}
