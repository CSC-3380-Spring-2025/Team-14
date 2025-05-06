using UnityEngine;

public class StayUpright: MonoBehaviour
{
//This make the game object stay upright making sure the the UpgradeUi doesnt move--------------------------------------------------------------------
    void Update()
    {
        transform.rotation = Quaternion.identity; // Reset the rotation to no rotation makes the object rotation zero on all axes
    }
//--------------------------------------------------------------------
}