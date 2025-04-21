using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

//Ep 4, lock the turret to enemy around 15 min
//Ep 5, creating a bullet and setting its position, around 6 min
public class Turret : MonoBehaviour{

    private Transform target; //turret targeting the target, it is hidden because private, but if public you will see active change as enemy come into range
    private Enemy targetEnemy; 
    public GameObject upgradeUI;
    public Button upgradeButton;
    public Button sellButton;
    public int purchaseCost = 0;
    private int level = 1;
    private float fireRateBase;
    private float targetingRangeBase;
    private int baseCost = 100;
    

    //Attributes of the turret
    [Header("Attributes")]//Bascially setup a header on unity
    public float range = 3f; //Range of the turret
    public float fireRate = 2f; //Rate of fire (How Fast turret fire)
    private float fireCountDown = 0f; //Countdown to fire

    [Header("Attack Damage")]
    public int bulletDamage = 50;


    //laser attributes
    [Header("Use Laser")]
    public bool useLaser = false; 
    public LineRenderer lineRenderer;
    public int DamageOverTime = 30; // each sec the laser does 30 dmg 
    public float slowAmount = .5f;

    


    //Basically setup a header on unity
    [Header("Unity SetUp Fields")]//Bascially setup a header on unity
    public string enemyTag = "Enemy"; //Set an asset on unity to the enemy tab, basically setting up the target to the enemy
    public float rotationSpeed = 30f; // Speed at which the turret rotates
    public GameObject bulletPrefab; //Set an asset on unity to the bullet prefab, basically setting up the bullet
    public Transform firePoint; //Set an asset on unity to the firepoint, basically setting up the firepoint
    


    private WaveTimer waveTimer; // Reference to the WaveTimer script

    [Header("Freeze Turret")]
    public bool canFreeze = false;
    public float freezeDuration = 2f;
    private HashSet<Enemy> frozenEnemies = new HashSet<Enemy>();
    
    [Header("Nuke")]
    public bool isNuke = false;
    public float nukeDamage = 99999f;




// Start is called once before the first execution of Update after the MonoBehaviour is created
//--------------------------------------------------------------------
    void Start(){
        if (isNuke)
    {
        KillAllEnemies();
        Destroy(gameObject);
        return;
    }
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Update target every 0.5 seconds

        // Automatically find the WaveTimer in the scene
        waveTimer = GameObject.Find("GameLogic").GetComponent<WaveTimer>();
        if (waveTimer == null) Debug.LogError("WaveTimer not found!");
        fireRateBase = fireRate;
        targetingRangeBase = range; 
        upgradeButton.onClick.AddListener(Upgrade);
        sellButton.onClick.AddListener(SellTurret);
        Debug.Log("New FireRate: " + fireRate);
        Debug.Log("New Range: " + range);
        Debug.Log("New Cost: " + CalculateCost());
    }
//--------------------------------------------------------------------





// UpdateTarget is called to find the nearest enemy within the range of the turret
//--------------------------------------------------------------------
    void UpdateTarget(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag); //find all the enemies and store it into this array
        GameObject nearestEnemy = null; //Set the nearest enemy to null
        float shortestDistance = Mathf.Infinity; //Set the shortest distance to infinity

        // Find the nearest enemy within the range of the turret
        foreach(GameObject enemy in enemies){
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript == null || enemyScript.IsDead) continue;

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
            targetEnemy = nearestEnemy.GetComponent<Enemy>();
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
        //if there is no target, exit the method
        if(target == null) {
            if (useLaser){
                if(lineRenderer.enabled)
                    lineRenderer.enabled = false; 
            }
            return;            
        }

        LockOnTarget(); // Lock the turret to the target

        // if the turret is using a laser, fire the laser
        if (useLaser){
            Laser();
            return;
        } 

        // If the turret is not using a laser, fire bullets
        else {
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
    void Laser() {
        if (targetEnemy == null || targetEnemy.IsDestroyed) return; // Ensure the target is valid and not destroyed

        targetEnemy.TakeDamage(DamageOverTime * Time.deltaTime); // Apply damage over time
        targetEnemy.Slow(slowAmount); // Apply slow effect

        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }
//--------------------------------------------------------------------
// Shoot is called to instantiate a bullet, set its position, and seek the target
//--------------------------------------------------------------------
    void Shoot() {
        if (target == null) return; // Ensure there is a valid target
       if (canFreeze) {
        if (frozenEnemies.Contains(targetEnemy)) {
            target = null;
            targetEnemy = null;
            return;
        }
         if (lineRenderer != null) {
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
        lineRenderer.enabled = true;
    }
        targetEnemy.Freeze(freezeDuration);
        frozenEnemies.Add(targetEnemy);
        target = null;
        targetEnemy = null;
    }
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Instantiate bullet
        Bullet bullet = bulletGO.GetComponent<Bullet>(); // Get the bullet component

        if (bullet != null) bullet.Seek(target); // Assign the target to the bullet
        bullet.damage = bulletDamage;
    }
    
//--------------------------------------------------------------------

    public void OpenUpgradeUI(){
        Debug.Log("Opening upgrade UI");
        upgradeUI.SetActive(true);
    }
    public void CloseUpgradeUI(){
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }
    public void Upgrade(){
        if(CalculateCost()> PlayerStats.Money)return;

        PlayerStats.Money -=CalculateCost();

        level++;
        fireRate = CalculateFireRate();
        range = CalculateRange();

        Debug.Log("New FireRate: " + fireRate);
        Debug.Log("New Range: " + range);
        Debug.Log("New Cost: " + CalculateCost());

        CloseUpgradeUI();


    }
    
    public void SellTurret(){
        int sellAmount = Mathf.RoundToInt(purchaseCost * 0.5f);
        PlayerStats.Money += sellAmount;

        Debug.Log($"Turret sold for {sellAmount}");

        Destroy(gameObject);
    }
    private int CalculateCost(){
        return Mathf.RoundToInt(baseCost * Mathf.Pow(level, .8f));
    }

    private float CalculateFireRate()
    {
        return fireRateBase * Mathf.Pow(level, .6f);
    }
    private float CalculateRange(){
        return  targetingRangeBase * Mathf.Pow(level, .4f);
    }
    private void KillAllEnemies()
{
    GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
    foreach (GameObject enemy in enemies)
    {
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        if (enemyScript != null && !enemyScript.IsDestroyed)
        {
            enemyScript.TakeDamage(nukeDamage);
        }
    }
}
}//end of class