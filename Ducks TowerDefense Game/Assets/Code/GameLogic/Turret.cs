using System;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;


public class Turret : MonoBehaviour{
    private Transform target; //turret targeting the target, it is hidden because private, but if public you will see active change as enemy come into range
    private Enemy targetEnemy; 
    private int level = 1;
    private float fireRateBase;
    private float targetingRangeBase;
    private int baseCost = 100;

    // ======== UI ========
    [Header("UI")]
    public GameObject upgradeUI;
    public Button upgradeButton;
    public Button sellButton;
    public int purchaseCost = 0;
    
    // ======== UI Text ========
    [Header("UI Panel")]
    public GameObject upgradeOptionsPanel;
    public TextMeshProUGUI costText;
    public TextMeshProUGUI notEnoughMoneyText;

    // ======== CORE STATS ========
    [Header("Attributes")]//Bascially setup a header on unity
    public float range = 3f; //Range of the turret
    public float fireRate = 2f; //Rate of fire (How Fast turret fire)
    private float fireCountDown = 0f; //Countdown to fire
    public GameObject bulletPrefab; //Set an asset on unity to the bullet prefab, basically setting up the bullet
    public Transform firePoint; //Set an asset on unity to the firepoint, basically setting up the firepoint
    public int bulletDamage = 50;

    // ======== LOCK TO ENEMY ========
    [Header("Enemy Tag")] // Correctly placed above public fields
    public string enemyTag = "Enemy"; //Set an asset on unity to the enemy tab, basically setting up the target to the enemy
    public float rotationSpeed = 30f; // Speed at which the turret rotates
    
    private WaveTimer waveTimer; // Reference to the WaveTimer script

    // ======== LASER STATS ========
    [Header("Use Laser")]
    public bool useLaser = false; 
    public LineRenderer lineRenderer;
    public int DamageOverTime = 30; // each sec the laser does 30 dmg 
    public float slowAmount = .5f;

    //
    // ======== FREEZE TURRET ========
    [Header("Freeze Turret")]
    public bool canFreeze = false;
    public float freezeDuration = 2f;
    private HashSet<Enemy> frozenEnemies = new HashSet<Enemy>();
    
    // ======== NUKE TURRET ========
    [Header("Nuke")]
    public bool isNuke = false;
    public float nukeDamage = 99999f;

    // ======== Range Indicator ========
    public GameObject rangeInd;

// Start is called once before the first execution of Update after the MonoBehaviour is created    
    void Start(){
        InvokeRepeating("UpdateTarget", 0f, 0.5f); //Update target every 0.5 seconds

        // Automatically find the WaveTimer in the scene
        waveTimer = FindFirstObjectByType<WaveTimer>();
        if (waveTimer == null) Debug.LogError("WaveTimer not found!");
        if (isNuke){
            KillAllEnemies();
            if (waveTimer != null && waveTimer.IsWaveActive())
            {
                waveTimer.OnWaveDefeated();
            }
            Destroy(gameObject);
            return;
        }
        fireRateBase = fireRate;
        targetingRangeBase = range; 
        upgradeButton.onClick.AddListener(OpenUpgradeOptions);
        sellButton.onClick.AddListener(SellTurret);
        if (rangeInd != null){
            float worldSize = range * 2f;
            float spriteSize = rangeInd.GetComponent<SpriteRenderer>().bounds.size.x / rangeInd.transform.localScale.x;
            float scale = worldSize / spriteSize;
            rangeInd.transform.localScale = Vector3.one * scale;
        }
    }

// UpdateTarget is called to find the nearest enemy within the range of the turret
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
            targetEnemy = null;
        }
        // Notify WaveTimer if all enemies are defeated
        if (enemies.Length == 0 && waveTimer != null && waveTimer.IsWaveActive()) waveTimer.OnWaveDefeated();
    }

//Handles aiming, attacking, and firerate cooldowns
    void Update(){
        //if there is no target, exit the method
        if(target == null) {
            if (useLaser){
                if(lineRenderer.enabled) lineRenderer.enabled = false; 
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

// Rotates the turret to face the target--
    void LockOnTarget(){
        if (target == null) return;
        Vector3 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Add 90 degrees if sprite's head points up (Y-axis)
        angle -= 90f; // Subtract 90 degrees to align sprite's "up" with target
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

//Applies damage and slow effects on the target when using the laser--
// uses LineRenderer to connect the turret to the target    
    void Laser() {
        if (targetEnemy == null || targetEnemy.IsDestroyed) return; // Ensure the target is valid and not destroyed

        targetEnemy.TakeDamage(DamageOverTime * Time.deltaTime); // Apply damage over time
        targetEnemy.Slow(slowAmount); // Apply slow effect

        if (!lineRenderer.enabled) lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }

// Shoot is called to instantiate a bullet, set its position, and seek the target
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
                StartCoroutine(ShowFreezeLaser());
            }
            targetEnemy.Freeze(freezeDuration);
            frozenEnemies.Add(targetEnemy);
            target = null;
            targetEnemy = null;
        }
    
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation); // Instantiate bullet
        Bullet bullet = bulletGO.GetComponent<Bullet>(); // Get the bullet component

        if (bullet != null) bullet.Seek(target); // Assign the target to the bullet
        bullet.damage = bulletDamage; // Set the bullet damage
    }
    
// Opens the upgrade UI for this turret.
    public void OpenUpgradeUI(){
        Debug.Log("Opening upgrade UI");
        upgradeUI.SetActive(true);
    }

// Closes the upgrade UI
    public void CloseUpgradeUI(){
        upgradeUI.SetActive(false);
        UIManager.main.SetHoveringState(false);
    }

 //Sells the turret for half of the amount that you got it for 
    public void SellTurret(){
        int sellAmount = Mathf.RoundToInt(purchaseCost * 0.5f);
        PlayerStats.Money += sellAmount;
        Debug.Log($"Turret sold for {sellAmount}");
        Destroy(gameObject);
    }

    private int CalculateCost() => Mathf.RoundToInt(baseCost * Mathf.Pow(level, .8f));// Calculates the cost of the next upgrade based on the turret's level
    private float CalculateFireRate() => fireRateBase * Mathf.Pow(level, .6f);// Calculates the fire rate after upgrading
    private float CalculateRange() => targetingRangeBase * Mathf.Pow(level, .4f);// Calculates the attack range after upgrading
// Instantly kills all enemies on the map--
    private void KillAllEnemies(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        foreach (GameObject enemy in enemies){
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null && !enemyScript.IsDestroyed){
                enemyScript.TakeDamage(nukeDamage);
            }
        }
    }

//shows the freeze laser for the set amount time
    private IEnumerator ShowFreezeLaser(){
        lineRenderer.enabled = true; 
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(.5f);
        lineRenderer.enabled = false;
    }

//Opens the upgrage UI(the firerate range and damage)
    public void OpenUpgradeOptions(){ //Onclick
        if (upgradeOptionsPanel == null || costText == null) return;
        upgradeOptionsPanel.SetActive(true); 
        costText.text = $"Cost: {CalculateCost()}" ; // Update the cost text
    }

//Closes the upgrage UI(the firerate range and damage)
    public void CloseUpgradeOptions() { //onClick
        if (upgradeOptionsPanel != null)
            upgradeOptionsPanel.SetActive(false); 
            upgradeUI.SetActive(false);
    }

//Shows "Not Enough Money" for 1.5 seconds
    private IEnumerator ShowNotEnoughMoney(){
        notEnoughMoneyText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f); // Longer display time
        notEnoughMoneyText.gameObject.SetActive(false); // Only hide text
        upgradeUI.SetActive(false);
    }

//Upgrades the turret's fire rate and range if the player has enough money
    public void UpgradeFireRateAndRange(){
        if (CalculateCost() > PlayerStats.Money){
            StartCoroutine(ShowNotEnoughMoney()); // Show not enough money message
            return; 
        }

        PlayerStats.Money -= CalculateCost();
        level++;
        fireRate = CalculateFireRate();
        range = CalculateRange();

        Debug.Log("Upgraded Fire Rate and Range!");
        Debug.Log("New Fire Rate: " + fireRate);
        Debug.Log("New Range: " + range);

        CloseUpgradeOptions();
    }

//Upgrades the turret's bullet damage if the player has enough money
    public void UpgradeDamage(){
        if (CalculateCost() > PlayerStats.Money){
            StartCoroutine(ShowNotEnoughMoney()); // Show not enough money message
            return; 
        }

        PlayerStats.Money -= CalculateCost();
        level++;

        bulletDamage = Mathf.RoundToInt(50 * Mathf.Pow(level, 0.6f)); 
        Debug.Log("Upgraded Bullet Damage! New Damage: " + bulletDamage);

        CloseUpgradeOptions(); 
    }
//Shows the turrret's range indicator
    public void ShowRange(float seconds){
        if (rangeInd != null){
            rangeInd.SetActive(true);
            StartCoroutine(HideRange(seconds));
        }
    }

//hides the turret range indicator
    private IEnumerator HideRange(float delay){
        yield return new WaitForSeconds(delay);
        if(rangeInd != null){
            rangeInd.SetActive(false);
        }
    }
}// End of Turret.cs