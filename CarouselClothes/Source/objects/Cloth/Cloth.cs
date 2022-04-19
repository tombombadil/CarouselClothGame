using System.Collections;
using System.Collections.Generic;
using PaintIn3D;
using UnityEngine;
using Ruckcat;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using UnityEngine.Experimental.Rendering;

public class Cloth : HyperSceneObj
{
    [FoldoutGroup("Cloth")] public ClothTypes _ClothType;
    [FoldoutGroup("Cloth")] public GameObject PrefabHolder;
    [FoldoutGroup("Cloth")] public MeshRenderer Tmrenderer;
    [FoldoutGroup("Ui")] public GameObject UiBubble;
    [FoldoutGroup("Ui")] public GameObject UiPoint;
    [FoldoutGroup("Ui")] public GameObject UiPrefab;
    [FoldoutGroup("Events")] public UnityEvent EVENT_ON_CONNECTED = new UnityEvent();
    [FoldoutGroup("Events")] public UnityEvent EVENT_ON_DISCONNECTED = new UnityEvent();
    [FoldoutGroup("ClothMoveSettings")] public float MoveTime = 0.4f;
    [FoldoutGroup("ClothMoveSettings")] public float AnimDelayToCarousel = 0.5f;
    [FoldoutGroup("ClothMoveSettings")] public float AnimDelayFromCarousel = 0.5f;
    [HideInInspector] public IClothOwner OwnerCustomer;

    private int thisprefabnum;
    private Faces OwnerFace;
    public override void Init()
    {
        base.Init();
        //SelectPrefab();
    }

    public override void Update()
    {
        base.Update();
    }

    public void SetOwnerCustomer(IClothOwner customer)
    {
        OwnerCustomer = customer;
    }

    public void SelectPrefab()
    {
        int prefabnum = -1;

        while(prefabnum == -1)
        {
            prefabnum = UnityEngine.Random.Range(0, PrefabHolder.transform.childCount);

            if((GameCont.Instance as GameCont).GetisClothActive(_ClothType,prefabnum))
            {
                prefabnum = -1;
            }
        }
        thisprefabnum = prefabnum;
        (GameCont.Instance as GameCont).AddPrefabToList(_ClothType, thisprefabnum, PrefabHolder.transform.childCount);
        PrefabHolder.transform.GetChild(prefabnum).gameObject.SetActive(true);
        CompareMesh = PrefabHolder.transform.GetChild(prefabnum).gameObject.GetComponent<MeshRenderer>();
    }

    public void MoveClothToSlot(ClothSlot toGo)
    {
        Vector3 start = transform.position;
        LeanTween.value(0, 1, MoveTime).setEase(LeanTweenType.easeOutExpo).setOnUpdate((float value) =>
        {
          
            Vector3 end = toGo.SlotCollider.transform.position;
            Vector3 pos = start + ((end - start) * value);
            transform.position = pos;
        }).setDelay(AnimDelayToCarousel);
        
        LeanTween.rotateLocal(gameObject, Vector3.zero, MoveTime).setEase(LeanTweenType.easeOutExpo).setDelay(AnimDelayToCarousel);
        EVENT_ON_CONNECTED.Invoke();
    }

    public void MoveClothToCustomer(GameObject toGo)
    {
        LeanTween.move(gameObject, toGo.transform.position, MoveTime).setDelay(AnimDelayFromCarousel);
        LeanTween.rotateY(gameObject, toGo.transform.eulerAngles.y, MoveTime).setDelay(AnimDelayFromCarousel).setOnComplete(() => {  });
        EVENT_ON_DISCONNECTED.Invoke();

    }

    private float ClothPoint = 0;
    private MeshRenderer CompareMesh;
    private float CalculatePoint()
    {
        float _p = CompareForPoints(Tmrenderer,CompareMesh);
        ClothPoint = _p;
        return _p;
    }

    private float CompareForPoints(MeshRenderer Tone,MeshRenderer Ttwo)
    {

        Texture2D tonetex = toTexture2D((RenderTexture)Tone.material.GetTexture("_BaseMap"));
        Texture2D ttwotex = ((Texture2D)Ttwo.material.GetTexture("_BaseMap"));

        Color[] firsttex = tonetex.GetPixels();
        Color[] secondtex = ttwotex.GetPixels();

        float ComparePoints = 0;

        for (int y = 0; y < firsttex.Length; y++)
        {
            if (firsttex[y] == secondtex[y])
                ComparePoints++;
        }

        float PointPercentage = (ComparePoints - 7365) / (firsttex.Length- 7365);

        Debug.Log("ComparePoint amount:  " + (ComparePoints - 7365) + "  Percentage: " + PointPercentage);
        return PointPercentage;
    }

    private Texture2D toTexture2D(RenderTexture rt)
    {
        var texture = new Texture2D(rt.width, rt.height, rt.graphicsFormat, 0, TextureCreationFlags.None);
        RenderTexture.active = rt;
        texture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        texture.Apply();

        RenderTexture.active = null;

        return texture;
    }

    public void SetFace(Faces _f)
    {
        OwnerFace = _f;
    }


    public void SetPrefab(bool isOpen)
    {
        HandleBubbleUi(isOpen);
        SetBubblePrefab(isOpen);
    }

    public void SetPoint(bool isOpen)
    {
        CalculatePoint();
        (GameCont.Instance as GameCont).SetBubbleUi(ClothPoint,OwnerFace);
        //HandleBubbleUi(isOpen);
        //SetBubblePoint(isOpen);
    }



    public void HandleBubbleUi(bool isOpen) { UiBubble.SetActive(isOpen); }
    public void SetBubblePoint(bool isOpen) 
    {
        UiPoint.SetActive(isOpen);
        if(isOpen == true)
        {
           // CalculatePoint();
           // UiPoint.GetComponentInChildren<SelectImages>().SetImagePoint(ClothPoint);
            UiPoint.GetComponentInChildren<SelectStar>().SetStarPoint(ClothPoint);
            LeanTween.moveLocal(UiBubble.gameObject, new Vector3(-4, 9, 0), 0.5f).setEase(LeanTweenType.easeOutBounce).setDelay(0);
            LeanTween.rotateLocal(UiBubble.gameObject, new Vector3(-10   , 20, 14), 0.5f).setEase(LeanTweenType.easeOutBounce).setDelay(0);
            float scale = 0.5f;
            LeanTween.scale(UiBubble.gameObject, new Vector3(scale,scale,scale), 0.5f).setEase(LeanTweenType.easeOutBounce).setDelay(0);
                
        }       
    }
    public void SetBubblePrefab(bool isOpen) { UiPrefab.SetActive(isOpen); }

}
