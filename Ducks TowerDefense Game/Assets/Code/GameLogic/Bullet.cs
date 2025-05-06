using UnityEngine;
using System.Collections.Generic;

public class Bullet : MonoBehaviour{

    // ========== Basic Variables ==========
    private Transform target;
    public float speed = 10f;
    public int damage = 50; //damge on enemy
    public static int totalKills = 0;// number of kills you have

    // ========= AOE Variables ==========
    public float explosionRadius = 0f; // radius of aoe
    public GameObject effect;
    private bool hasHitTarget = false; // Prevent multiple hits

//assign the target
    public void Seek(Transform _target) => target = _target;
    
//This is used to move the bullet to the target.
//after it hits a target it calls hit target    
    void Update(){
        if (target == null){
            Destroy(gameObject);
            return;
        }
        Vector3 direction = target.position - transform.position;
        float distanceOfFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceOfFrame){
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceOfFrame, Space.World);
        
        // Rotate the bullet to face the target in 2D
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + -90f; // Calculate the angle
        transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation around the Z-axis
        //Debug.DrawLine(transform.position, target.position, Color.red, 0.1f);
        //Debug.Log("Bullet Position: " + transform.position);//Check to see if bullet is moving
    }

//This deals with what happens when the bullet hits the target
    void HitTarget() {
        if (hasHitTarget) return; // Prevent multiple hits
        hasHitTarget = true;

        GameObject effectIns = (GameObject)Instantiate(effect, transform.position, transform.rotation);
        Destroy(effectIns, 0.5f);

        if (explosionRadius > 0f) Explode();
        else Damage(target);

        Destroy(gameObject);
    }
//This deals AOE damge by doing damage to all of the enemies in the radius
    void Explode() {
        // Changed to Physics2D for 2D games
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        HashSet<Enemy> damagedEnemies = new HashSet<Enemy>();

        foreach (Collider2D collider in colliders) {
            if (collider.tag == "Enemy") {
                Enemy e = collider.GetComponent<Enemy>();
                if (e != null && !damagedEnemies.Contains(e)) {
                    Damage(collider.transform);
                    damagedEnemies.Add(e);
                }
            }
        }
    }
//This deals damage to an enemy
    void Damage(Transform enemy) {
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null && !e.IsDestroyed) e.TakeDamage(damage); // Check if the enemy is not already destroyed
        
    }
}//End of Bullet.cs