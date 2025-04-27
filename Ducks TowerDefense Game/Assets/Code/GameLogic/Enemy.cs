using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.AI;

// This script is responsible for enemy behavior, including movement, health management, and special abilities.
public class Enemy : MonoBehaviour{

    private Enemy enemy; // Reference to the Enemy component


    // ======== CORE STATS (All Enemies) ========
    [Header("Basic Attributes")]
    public float startSpeed = 3f;//Speed
    public float startHealth = 100; // Initial health of the enemy
    public int value = 50; // Money value of the enemy when defeated
    public Image healthBar; // Reference to the health bar UI element
    private float speed; // Current speed of the enemy
    private float health; // Health of the enemy


    // ======== WAYPOINTS ========
    private Transform target;//next target of wavepoint
    private int wavepointIndex  = 0;//incrementing to make sure we have the correct number of waypoint
    public static int enemiesRemaining = 0; // Static variable to track the number of enemies remaining
    private  WaveTimer waveTimer; // Reference to the WaveTimer script


   
    // ======== BOSS TYPE SWITCHES ========
    [Header("Boss Type Configuration")]
    public bool isRegenBoss = false; // Flag to check if the enemy is a Regeneration Boss
    public bool isResurrectionBoss = false; // Flag to check if the enemy is a Resurrection Boss
    public bool isEmperorBoss = false; // Flag to check if the enemy is an Emperor Boss - Emperor Boss is easy, Basic Enemy, increase health, and in spawner script create a loop to spawn enemy before emperor



    // ======== REGENERATION BOSS SPECIFIC ========
    [Header("Regeneration Boss Settings")]
    public float regenRate = 50f; // Amount of health to regenerate per interval
    public float regenInterval = 3.5f; // Time interval for health regeneration
    public float regenTimer = 0f; // Timer for health regeneration



    // ======== RESURRECTION BOSS SPECIFIC ========
    [Header("Resurrection Boss Settings")]
    public float resurrectHealthPercent = 1.0f; // Percentage of health to restore upon resurrection (1.0 = 100%)
    public float resurrectDelay = 3f;
    public int maxResurrections = 1;
    public float invulnerabilityDuration = 1f;
    private bool isInvulnerable;
    private int resurrectionCount = 0;
    private bool isDead = false; // Flag to check if the enemy is dead for resurrection logic
    public bool IsDead => isDead; // Method to check if the enemy is dead for other scripts
    

    private bool isDestroyed = false; // Flag to check if the enemy is destroyed for the Endpath method
    public bool IsDestroyed => isDestroyed; // Property to check if the enemy is destroyed for other scripts

    private bool isFrozen = false;


// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        speed = startSpeed;
        health = startHealth; // Initialize health to the starting value
        enemy = GetComponent<Enemy>();

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
    public void TakeDamage( float amount ){
        if (isDestroyed || isDead || isInvulnerable) return; // Prevent further damage if already destroyed

        health -= amount;

        healthBar.fillAmount = health / startHealth; // Update the health bar UI

        if (health <= 0){
            Die();
        }
    }
//--------------------------------------------------------------------

    public void Slow (float Pct){
        speed = startSpeed * (1f - Pct);
    }

//--------------------------------------------------------------------
    void Die(){
        if (isDestroyed || isDead) return; // Ensure Die() is only called once

        // Resurrection Boss Logic
        if (isResurrectionBoss && resurrectionCount < maxResurrections){
            StartCoroutine(Resurrect());
            return;
        }

        // Standard Death Logic
        isDestroyed = true; // Mark the enemy as destroyed
        PlayerStats.Money += value; // Add money to the player's stats
        enemiesRemaining--;

        if (enemiesRemaining <= 0 && waveTimer != null) {
            waveTimer.OnWaveDefeated();
        }
        Destroy(gameObject); // Destroy the enemy
    }   
//--------------------------------------------------------------------

    IEnumerator Resurrect() {
        // Mark as dead and increment count
        isDead = true;
        resurrectionCount++;
        
        // Store references to components
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>(); 
        Collider2D collider2D = GetComponent<Collider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        
        // Disable visuals and physics
        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (collider2D != null) collider2D.enabled = false;
        if (rb != null) rb.simulated = false; // Important for 2D physics
        
        // Optional: Play death particles
        //if (deathParticles != null) deathParticles.Play();
        
        // Wait for resurrection delay
        yield return new WaitForSeconds(resurrectDelay);
        
        // Resurrection
        isDead = false;
        health = startHealth * resurrectHealthPercent;
        
        // Re-enable components
        if (spriteRenderer != null) spriteRenderer.enabled = true;
        if (collider2D != null) collider2D.enabled = true;
        if (rb != null) rb.simulated = true;
        
        // Update health UI
        if (healthBar != null) {
            healthBar.fillAmount = health / startHealth;
            healthBar.gameObject.SetActive(true);
        }
        
        // Optional: Play resurrection particles
        //if (resurrectParticles != null) resurrectParticles.Play();

        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }

    public void Freeze(float duration){
        if (isFrozen) return;
        StartCoroutine(FreezeCoroutine(duration));
    }
    private IEnumerator FreezeCoroutine(float duration){
        isFrozen = true;
        float originalSpeed = speed;
        speed = 0f;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        isFrozen = false;
    }

// Update is called once per frame, updating target to each waypoint
//--------------------------------------------------------------------
    void Update(){
        if (isDead) return; // Skip if currently "dead" (resurrecting)

        // Regen Boss Logic
        if(isRegenBoss) {
            RegenerateHealth();
        }

        // Target is null if the enemy is destroyed or dead
        // Target is the next waypoint
        if(target == null) return;

        //if the target is not null, then we are going to move towards the target
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

        // Add head rotation logic
        // Add head rotation logic
        if (target != null) {
            RotateTowardsTarget();
        }

    }
//--------------------------------------------------------------------
    void RotateTowardsTarget() {
        Vector3 direction = target.position - transform.position;
        direction.Normalize();
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f; // Adjust angle to match sprite orientation
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }



//--------------------------------------------------------------------
    void RegenerateHealth() {
        if (health <= 0) return; 

        regenTimer += Time.deltaTime;

        // Check if the regeneration timer has reached the interval
        // If so, regenerate health and reset the timer
        // Also ensure health does not exceed the starting health
        if (regenTimer >= regenInterval) {
            regenTimer = 0f;
            health += regenRate;

            if (health > startHealth) 
                health = startHealth;

            // Optional: update health bar
            if (healthBar != null)
                healthBar.fillAmount = health / startHealth; // Use startHealth here as well
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
       if (isDestroyed) return; // Prevent double-counting if somehow called again

    isDestroyed = true; // Mark as destroyed so it's not handled twice
    PlayerStats.Lives--; // Reduce lives
    enemiesRemaining--; // Decrement enemy counter

    if (enemiesRemaining <= 0 && waveTimer != null) {
        waveTimer.OnWaveDefeated();
    }

    Destroy(gameObject);
}
//--------------------------------------------------------------------
}//end of class Enemy
