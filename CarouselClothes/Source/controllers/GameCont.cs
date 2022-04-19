using Ruckcat;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public enum ClothTypes
{
    Tshirt,
    Dress,
    Coat,
    Skirt
}

public class ClothList
{
    public ClothTypes _Type;
    public List<int> ClothPrefablist = new List<int>();
}

public class GameCont : HyperGameCont
{
    private List<ClothList> AllLists = new List<ClothList>();
    public CarouselCont _CarouselCont;
    public CoinPickup _coin;
    public float CollectWaitTime;
    public GameObject UiBubble;
    public float UiBubbleCloseTime;

    public override void Init()
    {
        base.Init();
    }


    public override void StartGame()
    {
        base.StartGame();
    }


    public override void Update()
    {
        base.Update();
    }

    public void Coint(float _p)
    {
        if (_p >= 0.9f)
        {
            _coin.Amount = 25;
        }
        else if (_p >= 0.3f && _p < 0.9f)
        {
            _coin.Amount = 10;
        }
        else if (_p < 0.3f)
        {
            _coin.Amount = 5;
        }
        _coin.Create();
        StartCoroutine(CollectCoin());
    }

    IEnumerator CollectCoin()
    {
        yield return new WaitForSeconds(CollectWaitTime);
        CoinCollect();
    }

    private void CoinCollect()
    {      
        _coin.CollectCoin();
    }

    public void SetBubbleUi(float _p,Faces _f)
    {
        UiBubble.SetActive(true);
        UiBubble.GetComponentInChildren<SelectImages>().SetImagePoint(_p,_f);
        UiBubble.GetComponentInChildren<SelectStar>().SetStarPoint(_p);
        Coint(_p);
        StartCoroutine(CloseBubble());
    }

    IEnumerator CloseBubble()
    {
        yield return new WaitForSeconds(UiBubbleCloseTime);
        UiBubble.SetActive(false);
    }

    public bool GetisClothActive(ClothTypes _ctype,int pnum)
    {
        bool isActive = false;
        ClothList Clist = GetList(_ctype);

        if(Clist == null)
        {
            ClothList newlist = new ClothList();
            newlist._Type = _ctype;
            AllLists.Add(newlist);

            return isActive;
        }


        for (int i = 0; i < Clist.ClothPrefablist.Count; i++)
        {
            if (pnum == Clist.ClothPrefablist[i])
                isActive = true;

        }
        return isActive;
    }

    public void AddPrefabToList(ClothTypes _t, int pnum, int maxprefabcount)
    {
        ClothList Clist = GetList(_t);

        Clist.ClothPrefablist.Add(pnum);

        if (Clist.ClothPrefablist.Count >= maxprefabcount) Clist.ClothPrefablist.Clear();
    }

    private ClothList GetList(ClothTypes _t)
    {
        ClothList returncloth = null;

        for (int i = 0; i < AllLists.Count; i++)
        {
            if (AllLists[i]._Type == _t) returncloth = AllLists[i];
        }

        return returncloth;
    }


    protected override void GameOver(GameResult result)
    {
        base.GameOver(result);
    }

}