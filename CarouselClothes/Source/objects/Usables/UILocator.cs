using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;
using UnityEngine.UI;

public class UILocator : HyperSceneObj
{

    public GameObject Followerimage;
    private Camera _MainCam;

    private Canvas RelatedCanvas;
    public Vector2 BoundriesMax;
    public Vector2 BoundriesMin;


    private Image ObjectRenderer;
    private RectTransform _Imageobj; 


    public override void Init()
    {
        base.Init();

        ObjectRenderer = gameObject.GetComponentInChildren<Image>();
        _MainCam = Camera.main;
        RelatedCanvas = (UICont.Instance as UICont).GetComponent<Canvas>();

        var obj =  Instantiate(Followerimage, RelatedCanvas.transform.position, Quaternion.identity, RelatedCanvas.transform);
        _Imageobj = obj.GetComponent<RectTransform>();
        _Imageobj.gameObject.SetActive(false);


         Minx = _MainCam.ViewportToScreenPoint(new Vector3(BoundriesMin.x, 0, 0)).x;
         Maxx = _MainCam.ViewportToScreenPoint(new Vector3(BoundriesMax.x, 0, 0)).x;
         Miny = _MainCam.ViewportToScreenPoint(new Vector3(0, BoundriesMin.y, 0)).y;
         Maxy = _MainCam.ViewportToScreenPoint(new Vector3(0, BoundriesMax.y, 0)).y;

        FindObjectOfType<UILocatorCont>().RegisterLoc(this);
    }

    float Minx;
    float Maxx;
    float Miny;
    float Maxy;

    public override void StartGame()
    {
        base.StartGame();
        _playervppos = _MainCam.WorldToViewportPoint((PlayerCont.Instance as PlayerCont).transform.position);
    }

    public override void Update()
    {
        base.Update();

        if (isClosed) return;

        CheckifinCamera();

        if (isobjectVisible)
        {
            if (_Imageobj.gameObject.activeSelf) _Imageobj.gameObject.SetActive(false);
            return;
        }
        if (!_Imageobj.gameObject.activeSelf) _Imageobj.gameObject.SetActive(true);

        RelocateImage();

        if (isControlledClosed) _Imageobj.GetComponentInChildren<Image>().enabled =false;
        else _Imageobj.GetComponentInChildren<Image>().enabled = true;

    }

    bool isobjectVisible;
    private void CheckifinCamera() //camerada gözüküyor mu kontrolü
    {
        /*if (ObjectRenderer.isVisible) isobjectVisible = true; 
        else isobjectVisible = false;*/

        Vector3 screenPoint = _MainCam.WorldToViewportPoint(gameObject.transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;

        if (onScreen) isobjectVisible = true;
        else isobjectVisible = false;

    }


    private Vector3 _playervppos;
    private Vector3 _objvppos;

    private void RelocateImage()
    {
        _objvppos = _MainCam.WorldToViewportPoint(gameObject.transform.position);
        if (_objvppos.z < 0) _objvppos *= -1;       // Belli bir noktadan sonra z leri - ye dönyor bundan sonra ise x ve y işraret değiştiriyor

        /*var dir = _playervppos - _objvppos;
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90;
        _Imageobj.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

        SetRot();

        SetPose();
    }

    private void SetRot()
    {
        Vector3 targ = _objvppos;
        targ.z = 0f;

        targ.x = targ.x - _playervppos.x;
        targ.y = targ.y - _playervppos.y;

        float angle = Mathf.Atan2(targ.y, targ.x) * Mathf.Rad2Deg - 90;
        _Imageobj.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
 
    private void SetPose()
    {
        var _gos = _MainCam.ViewportToScreenPoint(_objvppos);
        var _ps = _MainCam.ViewportToScreenPoint(_playervppos);

        Vector3 _dir = new Vector3(_gos.x,_gos.y,0) - new Vector3(_ps.x,_ps.y,0) ;

       // Debug.Log("Object  " +_MainCam.WorldToScreenPoint(gameObject.transform.position) + "   player: " + _MainCam.ViewportToScreenPoint(_playervppos));

        Vector3 finalPosition = new Vector3(_dir.x,_dir.y,0) * 2;
        finalPosition.x = Mathf.Clamp(finalPosition.x, Minx, Maxx);
        finalPosition.y = Mathf.Clamp(finalPosition.y, Miny, Maxy);

        _Imageobj.position = finalPosition;

    }

    bool isClosed;
    public void OpenUIlocator() { isClosed = false; }
    public void CloseUIlocator()
    {
        isClosed = true;
        _Imageobj.gameObject.SetActive(false);
    }



    #region ControlMethods

    public List<UILocator> ControlList = new List<UILocator>();
    public bool isControlledClosed { get; set; }
    public bool isControlledOpen { get; set; }

    public bool GetImageStatus() { return isobjectVisible; }
    public RectTransform GetImageObject() { return _Imageobj; }
    #endregion


}
