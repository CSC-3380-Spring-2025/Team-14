using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour{
    public float speed = 10f;//Speed
    private Transform target;//next target of wavepoint
    private int wavepointIndex  = 0;//incrementing to make sure we have the correct number of waypoint
    public static int enemiesRemaining = 0; // Static variable to track the number of enemies remaining
    private  WaveTimer waveTimer; // Reference to the WaveTimer script

    public int health = 100; 

    public int value = 50;



// Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    void Start(){
        target = WayPoint.points[0];//Target is the first waypoint
        enemiesRemaining++; // Increment the number of enemies when spawned

        // Automatically find the WaveTimer in the scene
        // Basically, we are finding the WaveTimer script in the scene and storing it in the waveTimer variable
        // If the script is not found, we log an error message
        waveTimer = GameObject.Find("GameLogic").GetComponent<WaveTimer>();
        if (waveTimer == null){
            Debug.LogError("WaveTimer not found!");
        }
    }
//--------------------------------------------------------------------

//--------------------------------------------------------------------
    public void TakeDamage( int amount ){
        health -= amount;

        if (health <= 0){
            Die();
        }
    }
//--------------------------------------------------------------------

//--------------------------------------------------------------------
    void Die(){
        PlayerStats.Money += value;
        Destroy(gameObject);
    }   
//--------------------------------------------------------------------

// Update is called once per frame, updating target to each waypoint
//--------------------------------------------------------------------
    void Update(){
        //if the target is null, return
        if(target == null) return;

        //dir is the direction of the target - the current position of the enemy
        //transform.translate is the movement of the enemy in the direction of the target
        Vector3 dir = target.position - transform.position; //find the next wave length, so we can move
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);  //move in this direction
                                                                                    //normalized is something you have to do, basically to make sure this is always the same length and fix speed, that the only thing controlling the speed will be our speed
                                                                                    //time.deltatime  make sure the speed we are moving at is not dependent on the frame rate
                                                                                    //space.world mean the space that we are moving is relative to the world space
        //if the distance between the enemy and the target is less then 0.08f, then get the next waypoint
        if(Vector3.Distance(transform.position, target.position) <= 0.08f){
            //find if the distance of the enemy postion and target(waypoint) position is less then 0.2 unit
            //if the character is lagging, you might need to change 0.2f to something else like 0.4f
            GetNextWayPoint();//new target will be the next waypoint
        }

    }
//--------------------------------------------------------------------





// GetNextWayPoint is called to find the next waypoint in the path
//--------------------------------------------------------------------
    void GetNextWayPoint(){
        //if the wavepointIndex is greater then or equal to the total number of waypoints, destroy the enemy and notify that the enemy is defeated
        //if the wavepointIndex is less then the total number of waypoints, increment the index and set the target to the next waypoint
        if(wavepointIndex >= WayPoint.points.Length - 1){
            Endpath();// this calls end path that causes the numb of lives to go down and to kill the final enemy if it hits the end of the path 
            return;//sometime the destroy method take a bit of time before calling it, causes the code to continue reading, so to make sure if this happens call return
        }
        wavepointIndex++;//increment the index
        target = WayPoint.points[wavepointIndex];//new target will be the next index
    }
//--------------------------------------------------------------------

//--------------------------------------------------------------------
    void Endpath (){
        PlayerStats.Lives--;// once the enemy hits the end the number of lives the player has goes down by 1 
        Destroy(gameObject, 0f);//once index is greater then or equal to the total waypoint, destroy the asset(enemy)
    }
//--------------------------------------------------------------------
}//end of class Enemy
