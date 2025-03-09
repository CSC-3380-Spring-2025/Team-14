using UnityEngine;

public class Enemy : MonoBehaviour{
    public float speed = 10f;//Speed
    private Transform target;//next target of wavepoint
    private int wavepointIdenx = 0;//incrementing to make sure we have the correct number of waypoint


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        target = WayPoint.points[0];//Target is the first waypoint
        
    }

    // Update is called once per frame, updating target to each waypoint
    void Update(){
        Vector3 dir = target.position - transform.position; //find the next wave length, so we can move
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World); //move in this direction
        //normalized is something you have to do, basically to make sure this is always the same length and fix speed, that the only thing controlling the speed will be our speed
        //time.deltatime  make sure the speed we are moving at is not dependent on the frame rate
        //space.world mean the space that we are moving is relative to the world space

        if(Vector3.Distance(transform.position, target.position) <= 0.08f){
            //find if the distance of the enemy postion and target(waypoint) position is less then 0.2 unit
            //if the character is lagging, you might need to change 0.2f to something else like 0.4f
            GetNextWayPoint();//new target will be the next waypoint
        }

    }

    void GetNextWayPoint(){

        if(wavepointIdenx >= WayPoint.points.Length - 1){
            Destroy(gameObject);//once index is greater then or equal to the total waypoint, destroy the asset(enemy)
            return;//sometime the destroy method take a bit of time before calling it, causes the code to continue reading, so to make sure if this happens call return
        }
        wavepointIdenx++;//increment the index
        target = WayPoint.points[wavepointIdenx];//new target will be the next index
    }
}
