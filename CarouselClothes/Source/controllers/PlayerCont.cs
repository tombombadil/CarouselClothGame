using System;
using System.Collections.Generic;
using LiquidVolumeFX;
using Ruckcat;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Touch = Ruckcat.Touch;
using Sirenix.OdinInspector;
using PaintIn3D;




public class PlayerCont : HyperPlayer
{
    [FoldoutGroup("SprayMovement")] public GameObject PaintObject;
    [FoldoutGroup("SprayMovement")]public float UpDownSpeed;
    [FoldoutGroup("SprayMovement")] public Vector2 HorLimits;
    [FoldoutGroup("SprayMovement")] public Vector2 VerLimits;
    [FoldoutGroup("SprayMovement")] public ParticleSystem sprayparticle;
    [FoldoutGroup("SprayMovement")] public P3dPaintSphere SpraySettings;
    [FoldoutGroup("SprayMovement")] public float SprayRadius;
    public LiquidVolume LiquidVolume;
    public Material DynamicColorMat;
    public Material DynamicColorMat2;


    private Camera _cam;
    public override void Init()
    {
        base.Init();
        _cam = Camera.main;
        SpraySettings.Radius = 0;
    }

    public void SetColor(Color color)
    {
        if (LiquidVolume)
        {
            LiquidVolume.liquidColor1 = color;
        }

        if (DynamicColorMat)
        {
            DynamicColorMat.color = color;
        }
        if (DynamicColorMat2)
        {
            Color c = color;
            c.a = 0.1f;
            DynamicColorMat2.color = color;
        }
    }



        public override void StartGame()
        {
            base.StartGame();
        }

        public override void Update()
        {
            base.Update();

        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            Time.timeScale = 3f;
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            Time.timeScale = 0.2f;
        }

        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Time.timeScale = 1f;
        }

    }

    public override void FixedUpdate()
    {
       // base.FixedUpdate();
    }



    public override void OnContact(ContactInfo contact)
    {
        base.OnContact(contact);

    }



    public override void OnContactChild(ContactInfo contact)
        {
            base.OnContactChild(contact);
        }

    #region movement input

    /*---------------- INPUT & MOVEMENT ----------------- */
    Vector2 startPoint;
    Vector2 updatePoint;
    Vector3 tempPos;
    public bool isTouchLocked;
    public override void onTouch(Ruckcat.Touch _touch)
    {
        if (isTouchLocked) return;
        base.onTouch(_touch);

        if (_touch.Phase == TouchPhase.Began)
        {
            startPoint = _touch.GetPoint(0);
            tempPos = transform.position;
            OpenCloseSpray(true);
            sprayparticle.Play();
        }

        if (_touch.Phase == TouchPhase.Moved)
        {
            if (tempPos != Vector3.zero)
            {
                updatePoint = _touch.GetPoint(1);

                transform.position = new Vector3(transform.position.x,
                        tempPos.y + (updatePoint.y - startPoint.y) * UpDownSpeed,
                        transform.position.z);
                startPoint = _touch.GetPoint(1);
                tempPos = transform.position;
            }
        }

        if (_touch.Phase == TouchPhase.Ended)
        {
            OpenCloseSpray(false);
            sprayparticle.Stop();
        }

            float xPose = Mathf.Clamp(transform.position.x, HorLimits.x, HorLimits.y);
        float yPose = Mathf.Clamp(transform.position.y, VerLimits.x, VerLimits.y);
        transform.position = new Vector3(xPose, yPose, transform.position.z);


        
        /*RaycastHit _hit;
        Ray _ray = _cam.ScreenPointToRay(_touch.GetScreenPoint());
        Physics.Raycast(_ray,out _hit, float.PositiveInfinity,RayLayer);*/

        // PaintObject.transform.LookAt(_hit.transform);
    }


    #endregion

    public void OpenCloseSpray(bool isOpen)
    {
        if (isOpen)
        {
            SpraySettings.Radius = SprayRadius;
        }
        else
        {
            SpraySettings.Radius = 0;
        }
    }

    #region Endlevel
    protected override void GameOver(GameResult result) { base.GameOver(result); }

    bool isEnded;
    private void LevelStatus(GameStatus _status)
    {
        if (isEnded) return;
        //Debug.Log("Status Event:  " + _status);
        if(_status == GameStatus.GAMEOVER_SUCCEED)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Dance");
            isEnded = true;
        }
        if (_status == GameStatus.GAMEOVER_FAILED)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetTrigger("Sad");
            isEnded = true;
        }
    }

    #endregion

    
}



