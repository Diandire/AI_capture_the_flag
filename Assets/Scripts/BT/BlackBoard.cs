using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackBoard : MonoBehaviour
{
    public static bool BlueFlagTaken=false,RedFlagTaken=false,BlueFlagInBase=true,RedFlagInBase=true;
    public GameObject RedFlagCarrier=null,BlueFlagCarrier=null;

    public bool PowerUpAvailable()
    {
        if(GameObject.Find(Names.PowerUp)!=null&&GameObject.Find(Names.PowerUp).layer!=0)return true;
        else return false;
    } 
    public bool HealthKitAvailable()
    {
        if(GameObject.Find(Names.HealthKit)!=null&&GameObject.Find(Names.HealthKit).layer!=0)return true;
        else return false;
    } 
}
