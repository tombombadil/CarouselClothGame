using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ruckcat;

public class UILocatorCont : HyperSceneObj
{


    public float MaxDistance;
    public List<UILocator> AllLocators = new List<UILocator>();



    public void RegisterLoc(UILocator _uiloc) { AllLocators.Add(_uiloc); }


    public override void Update()
    {
        base.Update();
        CheckLocators();
    }


    private void CheckLocators()
    {
        for (int i = 0; i < AllLocators.Count; i++)
        {
            if (AllLocators[i].GetImageStatus() == true) continue;

            for (int j = 0; j < AllLocators.Count; j++)
            {
                if (AllLocators[j].GetImageStatus() == true) continue;
                if (AllLocators[i] == AllLocators[j]) continue;

                //Debug.Log(Vector3.Distance(AllLocators[i].GetImageObject().position, AllLocators[j].GetImageObject().position));
                if(Vector3.Distance(AllLocators[i].GetImageObject().position, AllLocators[j].GetImageObject().position) < MaxDistance)
                {
                    if (AllLocators[i].isControlledClosed && AllLocators[j].isControlledClosed) continue;
                    if (AllLocators[i].isControlledOpen && AllLocators[j].isControlledClosed) continue;
                    if (AllLocators[i].isControlledClosed && AllLocators[j].isControlledOpen) continue;

                    if (!AllLocators[i].isControlledClosed && AllLocators[j].isControlledOpen)
                    {
                        AllLocators[i].isControlledClosed = true;
                        AllLocators[j].ControlList.Add(AllLocators[i]);
                    }
                    if (AllLocators[i].isControlledOpen && !AllLocators[j].isControlledClosed)
                    {
                        AllLocators[j].isControlledClosed = true;
                        AllLocators[i].ControlList.Add(AllLocators[j]);
                    }
                    if (!AllLocators[i].isControlledClosed && !AllLocators[j].isControlledClosed)
                    {
                        AllLocators[i].isControlledOpen = true;
                        AllLocators[i].ControlList.Add(AllLocators[j]);
                        AllLocators[j].isControlledClosed = true;
                    }
                }
                else
                {
                    if (AllLocators[i].isControlledOpen && AllLocators[j].isControlledClosed )
                    {
                        if(!CheckControls(AllLocators[i], AllLocators[j]))
                        {
                            AllLocators[i].isControlledOpen = false;
                        }
                    }
                }
            }
        }
    }


    private bool CheckControls(UILocator opennedone,UILocator closedone)
    {
        bool isThereOther = false;

        opennedone.ControlList.Remove(closedone);
        closedone.isControlledClosed = false;

        if (opennedone.ControlList.Count > 0) isThereOther = true;

        return isThereOther;
    }



}
