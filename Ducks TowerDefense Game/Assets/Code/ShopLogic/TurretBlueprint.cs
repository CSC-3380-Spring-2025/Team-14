using UnityEngine;
using System.Collections;

[System.Serializable]   
public class TurretBlueprint
{
    public GameObject prefab;
    public int cost;
    [HideInInspector] public bool isUnlocked; // Indicates if the turret is unlocked for purchase


    public int GetSellAmount (){
        return cost / 2;
    }
}

