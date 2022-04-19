using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;

public class OnEnemySpawned : UnityEvent<GameObject> { }

public class ObjectSpawner : HyperSceneObj
{
    public GameObject[] ObjecttoSpawn;
    public int SpawnAmount;
    public float SpawnTimer;
    public float SpawnBetObjtime;
    public GameObject[] Points;


    public OnEnemySpawned OnSpawned = new OnEnemySpawned();

    float elapsed = 0.0f;
    bool isTimerStarted;
    public override void Update()
    {
        base.Update();

        if (!isTimerStarted) return;

        if(elapsed < SpawnTimer)
        {
            elapsed += Time.deltaTime;
        }
        else if(elapsed > SpawnTimer)
        {
            SpawnObjects(SpawnAmount);
        }

    }


    public void SetTimer()
    {
        elapsed = 0f;
        isTimerStarted = true;
    }


    public void SpawnObjects(int amount)
    {
        isTimerStarted = false;
        StartCoroutine(delayedspawn(amount));
    }



    IEnumerator delayedspawn(int amount)
    {
        CoreSceneObject _item = null;
        int randobj = 0;
        for (int i = 0; i < amount; i++)
        {
            randobj = UnityEngine.Random.Range(0, ObjecttoSpawn.Length);
            _item = CoreSceneCont.Instance.SpawnItem<CoreSceneObject>(ObjecttoSpawn[randobj], GetPoint().transform.position, null, true);
            OnSpawned.Invoke(_item.gameObject);
            yield return new WaitForSeconds(SpawnBetObjtime);
        }      
    }


    private GameObject GetPoint()
    {
        int rpoint = UnityEngine.Random.Range(0, Points.Length);

        return Points[rpoint];
    }

}
