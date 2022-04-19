using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Char = Ruckcat.Char;

public class EventClothThrowed : UnityEvent<CustomerController> {}
public class CustomerController : Char, IClothOwner
{
    /* params */
    public Transform ClothParent;
    public GameObject Caraousel;
    public Faces GirlFace;
  
    public float TimeOffsetGoToB = 0.5f;
    public float TimeOffsetGoToExit = 3f;
    
    /* others */
    private static int i = 0;
   [HideInInspector] public EventClothThrowed EventThrowed = new EventClothThrowed();
   [HideInInspector] public EventClothThrowed EventCatched = new EventClothThrowed();
  
    public Animator anim;
   
    [HideInInspector] public Cloth cloth;
    [FoldoutGroup("EnemySettings")] public Ruckcat.FloatField Speed;

    private List<Customer> customers = new List<Customer>();
    public int _id; Vector3 _destination;
    public int Id { get => _id; set => _id = value; }
    public Vector3 Destination { get => _destination; set => _destination = value; }
    
    public bool isDestinationExit = false;
    
    public static bool ThrowedCheck = false;
        
    [HideInInspector] public bool isArriveToB = false;

    public bool PickCloth = false;

    public override void Init()
    {
        base.Init();
        if(ClothCont.Instance)
        {
            cloth =  ClothCont.Instance.GetCloth(this);
            cloth.EVENT_ON_CONNECTED.AddListener(onClothConnectToCarousel);
            cloth.EVENT_ON_DISCONNECTED.AddListener(onClothDisConnectFromCarousel);
            cloth.SetFace(GirlFace);
        }

        anim.GetComponent<Animator>();
        transform.LookAt(new Vector3(Caraousel.transform.position.x,
            transform.position.y,
            Caraousel.transform.position.z));
    }

    private void onClothConnectToCarousel( )
    {
        Throwed();
    }
    private void onClothDisConnectFromCarousel( )
    {
        Catched();
    }

    public void Update()
    {
        if (isDestinationExit && isArriveToB)
        {
            Destroy(this.gameObject);
        }
        
        if (Input.GetKeyDown(KeyCode.E))
        {
            Throwed();
        }

        // if (ThrowedCheck)
        // {
        //     Invoke("OthersThrowedMove",1.4f);
        //     ThrowedCheck = false;
        //
        // }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Catched();
        }
    }

    public override void FixedUpdate()
    {
        MoveCustomer();
    }

    public void MoveCustomer()
    {
        if (!isArriveToB)
        {
            animator.SetFloat("Speed", GetVelocity().magnitude * 10);
            transform.LookAt(new Vector3(Destination.x,
                transform.position.y,
                Destination.z));
            transform.Translate(transform.forward * Speed.GetValue() * Time.deltaTime, Space.World);
                

            Vector3 target = Destination;
            Vector3 customerPos = this.transform.position;
            customerPos.y = 0;
            target.y = 0;
            Vector3 distance = customerPos - target;
            Debug.Log(name + " distance " + distance.magnitude);
            if (distance.magnitude < 0.5)
            {
                animator.SetFloat("Speed", 0);
                isArriveToB = true;
            }
           
        }
        else
        {
            transform.LookAt(new Vector3(Caraousel.transform.position.x,
                transform.position.y,
                Caraousel.transform.position.z));
        }
        
    }
    
    public void Throwed()
    {
        // if (this.Destination == AreaA.transform.GetChild(0).position)
        {
            animator.SetFloat("Speed", GetVelocity().magnitude * 0);
            anim.SetBool("PickCloth",true);
            isArriveToB = true;
            Invoke("ThrowedMove",1.4f);
            this.Destination = CustomerSpawner.Instance.AreaB.transform.GetChild(CustomerSpawner.Instance.GetLastIndexOfB()).position;
            this.Id = -1;
        }
        // else if (Id > 0)
        // {
        //     Invoke("OthersThrowedMove",1.4f);
        // }
        ThrowedCheck = true;
        
        Invoke("dispatchThroweEvent",TimeOffsetGoToB);
    }

    private void dispatchThroweEvent()
    {
        EventThrowed.Invoke(this);
    }

    public void ThrowedMove()
    {
        Debug.Log("as");
        anim.SetBool("PickClothExit",true);
        anim.SetBool("PickCloth",false);
        isArriveToB = false;
        animator.SetFloat("Speed", GetVelocity().magnitude * 10);
    }

    // public void OthersThrowedMove()
    // {
    //     isArriveToB = false;
    //     this.Destination = AreaA.transform.GetChild(Id-1).position;
    //     this.Id = Id - 1;
    // }
    public void Catched()
    {
        if (Id < 0)
        {
            if (Id == -1)
            {
                anim.SetBool("HangCloth",true);
                isArriveToB = true;
                Invoke("CatchedMove",TimeOffsetGoToExit);
                this.Destination = CustomerSpawner.Instance.Exit.transform.position;
                
                
            }
            // else if(Id < -1)
            // {
            //     Invoke("OthersCatchedMove",AfterCatchWaitTime+1.4f); 
            // }
        }
    }
    
    
    
    public void CatchedMove()
    {
        Debug.Log("as");
        anim.SetBool("HangClothExit",true);
        anim.SetBool("HangCloth",false);
        isArriveToB = false;
        animator.SetFloat("Speed", GetVelocity().magnitude * 10);
        isDestinationExit = true;
        Invoke("dispatchCathcEvent", 0.5f);
    }
    private void dispatchCathcEvent()
    {
        EventCatched.Invoke(this);
    }
 

    // public void OthersCatchedMove()
    // {
    //     this.Id = Id + 1;
    //     this.Destination = AreaB.transform.GetChild(-(Id+1)).position;
    //     isArriveToB = false;
    // }

    public Transform GetClothParent()
    {
        return ClothParent;
    }

    IEnumerator Delay(int delay)
    {
        yield return new WaitForSeconds(delay);
    }
}


