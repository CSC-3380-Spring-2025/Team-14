using UnityEngine;
using System.Collections;

[System.Serializable]   
public class TurretBlueprint{
    public GameObject prefab;
    public int cost;
    [HideInInspector] public bool isUnlocked; // Indicates if the turret is unlocked for purchase

// This calc the money you get back when selling a turret
    public int GetSellAmount () => cost / 2;
}//End of TuuretBlueprint class

