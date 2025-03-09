using UnityEngine;

public class WayPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public static Transform[] points;//number of positions in the array

    void Awake(){
        points = new Transform[transform.childCount];//tells you the amount of waypoint there are and create the amount of element in the array
        for(int i = 0; i < points.Length; i++){//Set each number to an element on the array
            points[i] = transform.GetChild(i);
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
