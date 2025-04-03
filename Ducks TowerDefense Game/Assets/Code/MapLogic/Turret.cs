using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//Ep 4, lock the turret to enemy around 15 min
//Ep 5, creating a bullet and setting its position, around 6 min
public class Turret : MonoBehaviour{

    private Transform target; //turret targeting the target, it is hidden because private, but if public you will see active change as enemy come into range


    [Header("Attributes")]//Bascially setup a header on unity
    public float range = 3f; //Range of the turret
    [Header("Use Bullets")]
    public float fireRate = 2f; //Rate of fire (How Fast turret fire)
    private float fireCountDown = 0f; //Countdown to fire

     [Header("Use Laser")]
    public bool useLaser = false; 
    public LineRenderer lineRenderer;


    [Header("Unity SetUp Fields")]//Bascially setup a header on unity
    public string enemyTag = "Enemy"; //Set an asset on unity to the enemy tab, basically setting up the target to the enemy
    public float rotationSpeed = 30f; // Speed at which the turret rotates
    public GameObject bulletPrefab; //Set an asset on unity to the bullet prefab, basically setting up the bullet
    public Transform firePoint; //Set an asset on unity to the firepoint, basically setting up the firepoint
    


    private WaveTimer waveTimer; // Reference to the WaveTimer script





// Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    void Start(){
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Update target every 0.5 seconds

        // Automatically find the WaveTimer in the scene
        waveTimer = GameObject.Find("GameLogic").GetComponent<WaveTimer>();
        if (waveTimer == null) Debug.LogError("WaveTimer not found!");
    }
//--------------------------------------------------------------------





// UpdateTarget is called to find the nearest enemy within the range of the turret
//--------------------------------------------------------------------
    void UpdateTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //find all the enemies and store it into this array

        float shortestDistance = Mathf.Infinity; //Set the shortest distance to infinity
        GameObject nearestEnemy = null; //Set the nearest enemy to null

        // Find the nearest enemy within the range of the turret
        foreach(GameObject enemy in enemies){
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position); //find the distance between the turret and the enemy
            //if the distance to the enemy is less then the shortest distance, set the shortest distance to the distance to the enemy and set the nearest enemy to the enemy
            if(distanceToEnemy < shortestDistance){
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }


        // If the nearest enemy is within range, set it as the target
        // Otherwise, set the target to null
        if(nearestEnemy != null && shortestDistance <= range){ 
            target = nearestEnemy.transform;
        }
        else{ 
            target = null;
        }


        // Notify WaveTimer if all enemies are defeated
        if (enemies.Length == 0 && waveTimer != null && waveTimer.IsWaveActive()){
            waveTimer.OnWaveDefeated();
        }
    }
//--------------------------------------------------------------------





// Update is called once per frame
//--------------------------------------------------------------------
    void Update(){
        if(target == null) {//if there is no target, exit the method
                if ( useLaser){
                if(lineRenderer.enabled)
                    lineRenderer.enabled = false; 
            }
            return;
            
        }

        LockOnTarget();
       


         // Fire the turret
        if (useLaser){
            Laser();
            return;
        } else {
            // Fire the turret
        if(fireCountDown <= 0f){
            Shoot();
            fireCountDown = 1f / fireRate;
        }

        fireCountDown -= Time.deltaTime; // Decrease the fire countdown
        }
    }
//--------------------------------------------------------------------

   //--------------------------------------------------------------------
    void LockOnTarget(){
        

        // Lock the turret to the target
        Vector3 direction = target.position - transform.position; // Calculate the direction to the target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle in radians and convert to degrees
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle); // Create a target rotation using the calculated angle
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);// Smoothly rotate the turret towards the target
        
    }
//--------------------------------------------------------------------

//--------------------------------------------------------------------
 void Laser(){
        if (!lineRenderer.enabled)
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }
//--------------------------------------------------------------------
// Shoot is called to instantiate a bullet, set its position, and seek the target
//--------------------------------------------------------------------
    void Shoot(){
        //Debug.Log("Shoot");//goes to the console on unity and check if it printing out something, making sure the code is working

        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);//Instantiate a bullet, set its position and rotation
        Bullet bullet = bulletGO.GetComponent<Bullet>();//Get the bullet component

        // If the bullet is not null, seek the target
        if(bullet != null) bullet.Seek(target);
    }
//--------------------------------------------------------------------
}//end of class
