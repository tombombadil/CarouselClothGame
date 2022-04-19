using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public enum StackType
{
    Normal,
    Randomized

}

[System.Serializable]
public class StackItem
{
    public string Id { get; set; }
    public GameObject obj { get; set; }

}


public class Stack : HyperSceneObj
{
    public StackType _StackType;
    public int TotalObjectCount;
    [FoldoutGroup("StackSettings")] public GameObject SetStartPoint;
    [FoldoutGroup("StackSettings")] public LeanTweenType _easetype;
    [FoldoutGroup("StackSettings")] public Vector3 StackIn;
    [FoldoutGroup("StackSettings")] public Vector3 StackInOffset;
    [FoldoutGroup("StackSettings")] public float MoveTime;
    [FoldoutGroup("StackSettings")] public float TimeBetwMultObjects;
    [FoldoutGroup("StackSettings")] public float TimeBefDestSet = -1;
    [FoldoutGroup("StackSettings")] public List<StackItem> AllObjects = new List<StackItem>();
    [FoldoutGroup("RandomStackSettings")] public BoxCollider RandomPoseBox;
    [FoldoutGroup("OffsetSettings")] public bool isCurvedMove;
    [FoldoutGroup("OffsetSettings")] public float curveUpAmount;



    private GameObject _pointer;

    public override void Init()
    {
        base.Init();

        _pointer = new GameObject("Pointer");
        _pointer.transform.parent = SetStartPoint.transform;
        _pointer.transform.position = Vector3.zero;
        SetSides();
    }

    public override void StartGame()
    {
        base.StartGame();

    }

    [FoldoutGroup("Events")] public UnityEvent OnObjectFirstTouch = new UnityEvent();
    [FoldoutGroup("Events")] public UnityEvent OnObjectSlotted = new UnityEvent();
    [FoldoutGroup("Events")] public UnityEvent OnObjectStartedRelease = new UnityEvent();
    [FoldoutGroup("Events")] public UnityEvent OnObjectSettedPose = new UnityEvent();
    [FoldoutGroup("Events")] public UnityEvent OnObjectCountChanged = new UnityEvent();

    // Verilen Objeyi Animasyonsuz Stacke Ekler
    public void AddObject(GameObject _obj, string _id = "Null")
    {
        AddNewStackItem(_obj, _id);
        _obj.transform.parent = SetStartPoint.transform;
        Vector3 _newvec = CalculatePose();
        _pointer.transform.localPosition = _newvec;
        _obj.transform.localPosition = _pointer.transform.localPosition;
        RotateObj(_obj, SetStartPoint.transform.eulerAngles);
    }

    //Verilen objeyi Stacke Ekler
    public void GetObjectToStack(GameObject _obj, string _id = "Null")
    {
        if (CheckForDuplicate(_obj)) return;

        ObjectCount(1);
        switch (_StackType)
        {
            case StackType.Randomized:
                GetObjectRand(_obj, _id);
                break;
            case StackType.Normal:
                GetObjectNormal(_obj, _id);
                break;
        }
    }

    // Verilen objenin poziyonuna o rotasyonda bir obje yollar
    public void SetObjectToPose(GameObject _obj, string _id = "Null")
    {
        ObjectCount(-1);
        switch (_StackType)
        {
            case StackType.Randomized:
                SetObjectRandom(_obj, _id);
                break;
            case StackType.Normal:
                SetObjectNormal(_obj, _id);
                break;
        }
    }

    //Bu stack e Parametrede verilen stackten belirtilen sayıda obje alır // sadece bu metodda objelerin yok olması çalışır
    public void TakeObject(Stack _stack, int count, string _id = "Null")
    {
        StartCoroutine(TakeDelayer(_stack, count, _id));
    }

    public void DestroyObject(string _id = "Null")
    {
        if (AllObjects.Count <= 0) return;

        ObjectCount(-1);

        StackItem toleft = GetLastObjectById(_id);
        if (toleft == null) return;


        switch (_StackType)
        {
            case StackType.Normal:
                _pointer.transform.localPosition = CalculatePose();
                break;
            case StackType.Randomized:
                _pointer.transform.localPosition = CalculateRandomPose();
                break;
        }

        CoreSceneCont.Instance.DeSpawnItem(toleft.obj.GetComponent<CoreSceneObject>());
    }




    //------------------------Private Methods ------------------------------//



    private void ObjectCount(int _count)
    {
        TotalObjectCount += _count;
        OnObjectCountChanged.Invoke();
    }



    private void GetObjectNormal(GameObject _obj, string _id = "Null")
    {

        AddNewStackItem(_obj, _id);

        _obj.transform.parent = SetStartPoint.transform;

        Vector3 _newvec = CalculatePose();
        _pointer.transform.localPosition = _newvec;
        MoveObj(_obj, _pointer.transform.localPosition);
        RotateObj(_obj, SetStartPoint.transform.eulerAngles);
    }
    private void GetObjectRand(GameObject _obj, string _id = "Null")
    {
        AddNewStackItem(_obj, _id);

        _obj.transform.parent = SetStartPoint.transform;

        Vector3 _newvec = CalculateRandomPose();
        _pointer.transform.position = _newvec;
        MoveObj(_obj, _pointer.transform.localPosition);
        RotateObj(_obj, SetStartPoint.transform.eulerAngles);
    }

    private void SetObjectNormal(GameObject _obj, string _id = "Null")
    {
        StackItem toleft = GetLastObjectById(_id);
        if (toleft == null) return;

        LeanTween.cancel(toleft.obj);

        _pointer.transform.localPosition = CalculatePose();

        toleft.obj.transform.parent = _obj.transform;

        MoveObj(toleft.obj, Vector3.zero);
        RotateObj(toleft.obj, _obj.transform.eulerAngles);
    }

    private void SetObjectRandom(GameObject _obj, string _id = "Null")
    {
        StackItem toleft = GetLastObjectById(_id);
        if (toleft == null) return;

        LeanTween.cancel(toleft.obj);

        _pointer.transform.position = CalculateRandomPose();

        toleft.obj.transform.parent = _obj.transform;

        MoveObj(toleft.obj, Vector3.zero);
        RotateObj(toleft.obj, _obj.transform.eulerAngles);
    }

    /* public GameObject GetAnObject()
     {
         if (AllObjects.Count == 0) return null;
         GameObject toleft = AllObjects[AllObjects.Count - 1];
         LeanTween.cancel(toleft);

         AllObjects.Remove(toleft);

         _pointer.transform.localPosition = CalculatePose();

         return toleft;
     }*/


    IEnumerator TakeDelayer(Stack _stack, int count, string _id = "Null")
    {
        int allobjcount = count;
        if (count == -1) allobjcount = _stack.AllObjects.Count;

        for (int i = 0; i < allobjcount; i++)
        {
            StackItem toleft = _stack.GetLastObjectById(_id);
            if (toleft == null) break;
            _stack.ObjectCount(-1);

            GetObjectToStack(toleft.obj, toleft.Id);
            StartCoroutine(DelayDestroy(toleft));
            yield return new WaitForSeconds(TimeBetwMultObjects);
        }
        _stack.ReOrganizeObjects();
    }


    Vector3 HeightIn = new Vector3(0, 0, 0);
    private int xSide = 0;
    private int ySide = 0;
    private int zSide = 0;

    private Vector3 CalculatePose()
    {
        Vector3 newrot = Vector3.zero;
        if (AllObjects.Count == 0) return newrot;

        Renderer _bound = AllObjects[AllObjects.Count - 1].obj.GetComponentInChildren<Renderer>();

        Vector3 FloorCount = Mathf.FloorToInt((AllObjects.Count - 1) / GetArea()) * HeightIn;

        int xpieces = 0;
        int ypieces = 0;
        int zpieces = 0;

        newrot = new Vector3(
            0 + (_bound.bounds.size.y * FloorCount.x),
            0 + (_bound.bounds.size.y * FloorCount.y),
            0 + (_bound.bounds.size.z * FloorCount.z));



        if (xSide == 0)
        {
            ypieces = ((AllObjects.Count - 1) % ySide);
            zpieces = Mathf.FloorToInt((AllObjects.Count - 1) / ySide) % zSide;
            newrot += new Vector3((StackInOffset.x * FloorCount.x), (StackInOffset.y * ypieces), (StackInOffset.z * zpieces));
        }
        else if (ySide == 0)
        {
            xpieces = (AllObjects.Count - 1) % xSide;
            zpieces = Mathf.FloorToInt((AllObjects.Count - 1) / xSide) % zSide;
            newrot += new Vector3((StackInOffset.x * xpieces), (StackInOffset.y * FloorCount.y), (StackInOffset.z * zpieces));
            //zpieces = (AllObjects.Count - 1) % (xSide + zSide) - xpieces;
        }
        else if (zSide == 0)
        {
            ypieces = ((AllObjects.Count - 1) % ySide);
            xpieces = Mathf.FloorToInt((AllObjects.Count - 1) / ySide) % xSide;
            newrot += new Vector3((StackInOffset.x * xpieces), (StackInOffset.y * ypieces), (StackInOffset.z * FloorCount.z));
        }

        newrot = new Vector3(
            newrot.x + (_bound.bounds.size.x * xpieces),
            newrot.y + (_bound.bounds.size.y * ypieces),
            newrot.z + (_bound.bounds.size.z * zpieces));
        return newrot;
    }

    private Vector3 CalculateRandomPose()
    {
        Vector3 newrandvec = Utils.GetRandomPositionInBoxCollider(RandomPoseBox);
        Debug.Log(newrandvec);
        return newrandvec;
    }


    private int GetArea()
    {
        float stackx = (StackIn.x > 0 ? StackIn.x : 1);
        float stacky = (StackIn.y > 0 ? StackIn.y : 1);
        float stackz = (StackIn.z > 0 ? StackIn.z : 1);

        return Mathf.FloorToInt(stackx * stacky * stackz);
    }

    private void SetSides()
    {
        if (StackIn.x == 0)
        {
            xSide = 0;
            HeightIn = new Vector3(1, 0, 0);
        }
        else xSide = Mathf.FloorToInt(StackIn.x);

        if (StackIn.y == 0)
        {
            ySide = 0;
            HeightIn = new Vector3(0, 1, 0);
        }
        else ySide = Mathf.FloorToInt(StackIn.y);

        if (StackIn.z == 0)
        {
            zSide = 0;
            HeightIn = new Vector3(0, 0, 1);
        }
        else zSide = Mathf.FloorToInt(StackIn.z);

    }

    private void MoveObj(GameObject _obj, Vector3 _pose, bool isOpenCol = false)
    {
        /*LeanTween.moveLocal(_obj, _pose, MoveTime).setEase(_easetype).setOnComplete(()=> {
            _obj.GetComponent<Collider>().enabled = isOpenCol;
        });*/
        LeanTween.moveLocalX(_obj, _pose.x, MoveTime).setEase(_easetype);
        LeanTween.moveLocalZ(_obj, _pose.z, MoveTime).setEase(_easetype).setOnComplete(() =>
        {
            if (_obj.GetComponent<Collider>()) _obj.GetComponent<Collider>().enabled = isOpenCol;
        });

        if (isCurvedMove)
        {
            LeanTween.moveLocalY(_obj, _obj.transform.localPosition.y + curveUpAmount, MoveTime / 2).setEase(_easetype).setOnComplete(() => {
                LeanTween.moveLocalY(_obj, _pose.y, MoveTime / 2).setEase(_easetype);
            });

        }
        else
        {
            LeanTween.moveLocalY(_obj, _pose.y, MoveTime).setEase(_easetype);
        }
    }

    private void RotateObj(GameObject _obj, Vector3 _rot)
    {
        LeanTween.rotateLocal(_obj, _rot, MoveTime);
    }

    IEnumerator DelayDestroy(StackItem _objstack)
    {
        yield return new WaitForSeconds(MoveTime);
        if (TimeBefDestSet == -1) yield return null;
        else
        {
            yield return new WaitForSeconds(TimeBefDestSet);
            int indexOf = AllObjects.FindIndex(e => e.obj == _objstack.obj);
            AllObjects.RemoveAt(indexOf);
            _pointer.transform.localPosition = CalculatePose();
            CoreSceneCont.Instance.DeSpawnItem(_objstack.obj.GetComponent<CoreSceneObject>());
        }
    }

    #region idmethods

    private StackItem GetLastObjectById(string _id)
    {
        StackItem sel = null;

        if (AllObjects.Count == 0) return sel;

        for (int i = AllObjects.Count - 1; i >= 0; i--)
        {
            if (AllObjects[i].Id == _id)
            {
                sel = AllObjects[i];
                break;
            }
        }

        AllObjects.Remove(sel);

        return sel;
    }

    private void AddNewStackItem(GameObject _obj, string _id)
    {
        StackItem newitem = new StackItem();
        newitem.Id = _id;
        newitem.obj = _obj;
        AllObjects.Add(newitem);
    }

    private bool CheckForDuplicate(GameObject _obj)
    {
        bool isHave = false;

        foreach (var item in AllObjects)
        {
            if (item.obj == _obj)
            {
                isHave = true;
                break;
            }
        }
        return isHave;
    }

    private void ReOrganizeObjects() { StartCoroutine(reorg()); }
    IEnumerator reorg()
    {
        if (_StackType == StackType.Randomized) yield return null;
        for (int i = 0; i < AllObjects.Count; i++)
        {
            Vector3 _newvec = CalculatePoseSelected(i);
            if (AllObjects[i].obj.transform.localPosition == _newvec) continue;
            _pointer.transform.localPosition = _newvec;
            MoveObj(AllObjects[i].obj, _pointer.transform.localPosition);
            RotateObj(AllObjects[i].obj, SetStartPoint.transform.eulerAngles);
            yield return new WaitForSeconds(TimeBetwMultObjects);
        }
    }

    private Vector3 CalculatePoseSelected(int _i)
    {
        Vector3 newrot = Vector3.zero;
        if (_i == 0) return newrot;

        Renderer _bound = AllObjects[AllObjects.Count - 1].obj.GetComponentInChildren<Renderer>();

        Vector3 FloorCount = Mathf.FloorToInt((_i) / GetArea()) * HeightIn;

        int xpieces = 0;
        int ypieces = 0;
        int zpieces = 0;

        newrot = new Vector3(
            0 + (_bound.bounds.size.y * FloorCount.x),
            0 + (_bound.bounds.size.y * FloorCount.y),
            0 + (_bound.bounds.size.z * FloorCount.z));



        if (xSide == 0)
        {
            ypieces = ((_i - 1) % ySide);
            zpieces = Mathf.FloorToInt((_i - 1) / ySide) % zSide;
            newrot += new Vector3((StackInOffset.x * FloorCount.x), (StackInOffset.y * ypieces), (StackInOffset.z * zpieces));
        }
        else if (ySide == 0)
        {
            xpieces = (_i - 1) % xSide;
            zpieces = Mathf.FloorToInt((_i - 1) / xSide) % zSide;
            newrot += new Vector3((StackInOffset.x * xpieces), (StackInOffset.y * FloorCount.y), (StackInOffset.z * zpieces));
            //zpieces = (AllObjects.Count - 1) % (xSide + zSide) - xpieces;
        }
        else if (zSide == 0)
        {
            ypieces = ((_i - 1) % ySide);
            xpieces = Mathf.FloorToInt((_i - 1) / ySide) % xSide;
            newrot += new Vector3((StackInOffset.x * xpieces), (StackInOffset.y * ypieces), (StackInOffset.z * FloorCount.z));
        }

        newrot = new Vector3(
            newrot.x + (_bound.bounds.size.x * xpieces),
            newrot.y + (_bound.bounds.size.y * ypieces),
            newrot.z + (_bound.bounds.size.z * zpieces));
        return newrot;
    }

    #endregion



}
