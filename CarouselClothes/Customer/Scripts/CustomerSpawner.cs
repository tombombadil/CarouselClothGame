using System;
using System.Collections;
using System.Collections.Generic;
using Ruckcat;
using UnityEngine;

public class CustomerSpawner : CoreCont<CustomerSpawner>
{
    public List<GameObject> Customers;
    public GameObject AreaA;
    public GameObject AreaB;
    public GameObject Exit;
    private List<CustomerController> listAStack = new  List<CustomerController>();
    private List<CustomerController> listBStack = new  List<CustomerController>();
    private int customerCounter = 0;

    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    void Update()
    {
        // if (CustomerController.ThrowedCheck)
        // {
        //     ReloadCustomer();
        // }
    }

    IEnumerator SpawnCustomer()
    {
        for (int i = 0; i < AreaA.transform.childCount; i++)
        {
            Vector3 spawnPoint = AreaA.transform.GetChild(i).position;
            CustomerController newCustomer = spawnCustomer(spawnPoint);
           
            newCustomer.Id = i;
            newCustomer.Destination = AreaA.transform.GetChild(i).position;
          
            yield return new WaitForSeconds(0);
        }
    }

    private void onCustomerThrowed(CustomerController customer)
    {
        listAStack.RemoveAt(0);
        listBStack.Add(customer);
        ReloadCustomer(); //hemen yeni bir customer ekleyelim!
        for (int i = 0; i < listAStack.Count; i++)
        {
             listAStack[i].Destination = AreaA.transform.GetChild(i).position; //A stack listesinin siralamasini duzenler
             listAStack[i].isArriveToB = false;
             
        }
    }
    private void onCustomerCatched(CustomerController customer)
    {
        listBStack.RemoveAt(0);
        for (int i = 0; i < listBStack.Count; i++)
        {
            listBStack[i].Destination = AreaB.transform.GetChild(i).position; 
            listBStack[i].isArriveToB = false;
        }
    }
    
    
    public void ReloadCustomer()
    {
        int lastIndexOfA = AreaA.transform.childCount - 1;
        if (listAStack.Count < lastIndexOfA)
        {
            Vector3 spawnPoint = AreaA.transform.GetChild(lastIndexOfA).position;
            CustomerController newCustomer = spawnCustomer(spawnPoint);
            newCustomer.Id = lastIndexOfA;
       
            newCustomer.Destination = AreaA.transform.GetChild(lastIndexOfA).position;
            CustomerController.ThrowedCheck = false;
        }
       
       
    }

    private CustomerController spawnCustomer(Vector3 pos)
    {
        int random = UnityEngine.Random.Range(0, Customers.Count);
        GameObject prefab = Customers[random];
        CustomerController newCustomer = CoreSceneCont.Instance.SpawnItem<CustomerController>(prefab, pos, this.transform, true);
        newCustomer.EventThrowed.AddListener(onCustomerThrowed);
        newCustomer.EventCatched.AddListener(onCustomerCatched);
        listAStack.Add(newCustomer);
        newCustomer.name = "Customer" + customerCounter;
        customerCounter++;
        return newCustomer;
    }

    public int GetLastIndexOfB()
    {
        return listBStack.Count;
    }
}