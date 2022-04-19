using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Ruckcat;
using UnityEngine;

public interface IClothOwner
{
     Transform GetClothParent();
}

public class ClothCont : CoreCont<ClothCont>
{
    public List<GameObject> ClothPrefabs;
    public GameObject debugcustomerprefab;
    private List<GameObject> listClothPool = new List<GameObject>();
    private List<Cloth> _listStack = new List<Cloth>();
   
    public override void Init()
    {
        base.Init();
        (GameCont.Instance as GameCont)._CarouselCont.EVENT_EMPTY_SLOT.AddListener(onEmptySlot);
        (GameCont.Instance as GameCont)._CarouselCont.EVENT_END_SLOT.AddListener(onEndOfCarousel);
        
        
    }
    public override void Update()
    {
        base.Update();

        
        
    }
    public Cloth GetCloth(IClothOwner customer)
    {
        GameObject PrefabCloth = GetRandomCloth();
        Cloth c = CoreSceneCont.Instance.SpawnItem<Cloth>(PrefabCloth, Vector3.zero, customer.GetClothParent());
        c.transform.localPosition = Vector3.zero;
        c.SetOwnerCustomer(customer);
        _listStack.Add(c);
        return c;
    }

    /* ------ EVENT LISTENERS ------*/
    private void onEmptySlot(ClothSlot slot)
    {
        if (_listStack.Count > 0)
        {
            Cloth c = _listStack[0];
            addToCarousel(c, slot);
        }
        
    }
    private void onEndOfCarousel(ClothSlot slot)
    {

        Cloth c = slot.thecloth;
        removeFromCarousel(c, slot);
        
    }
    /* ------ ADD & REMOVE FROM CAROUSEL ------*/
    private void addToCarousel(Cloth cloth, ClothSlot slot)
    {
        cloth.gameObject.transform.parent = slot.SlotCollider.transform;
        cloth.MoveClothToSlot(slot);
        slot.thecloth = cloth;
        slot.isSlotFull = true;
        _listStack.RemoveAt(0);
    }
    private void removeFromCarousel(Cloth cloth, ClothSlot slot)
    {
        cloth.gameObject.transform.parent = cloth.OwnerCustomer.GetClothParent();
        cloth.MoveClothToCustomer(cloth.OwnerCustomer.GetClothParent().gameObject);
        slot.isSlotFull = false;
        cloth.SetPoint(true);
    }

    
    /* ------ GET RANDOM CLOTH SYSTEM ------*/
    private GameObject GetRandomCloth()
    {
        if (listClothPool.Count == 0)
        {
            listClothPool =  new List<GameObject>(ClothPrefabs);
        }

        GameObject c = null;
        if (listClothPool.Count > 0)
        {
            int rand = UnityEngine.Random.Range(0, listClothPool.Count);
            c = listClothPool[rand];
            listClothPool.Remove(c);
        }
       
        return c;
    }

   
     
}
