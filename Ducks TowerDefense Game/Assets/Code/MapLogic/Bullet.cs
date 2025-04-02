using UnityEngine;
public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 30f;

    public int damage = 50; 
    public static int totalKills = 0;

    //For AOE effect
    public float expolsionRadius = 0f;
    public GameObject effect;
    private Economy economy;
     

    void Start(){
        economy = Object.FindFirstObjectByType<Economy>();
        
    }

    public void Seek(Transform _target){
        target = _target;
    }

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
        transform.LookAt(target);
        
        // Rotate the bullet to face the target in 2D
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // Calculate the angle
        transform.rotation = Quaternion.Euler(0, 0, angle); // Apply rotation around the Z-axis
        //Debug.DrawLine(transform.position, target.position, Color.red, 0.1f);
        //Debug.Log("Bullet Position: " + transform.position);//Check to see if bullet is moving
    }

    void HitTarget(){
        GameObject effectIns = (GameObject)Instantiate(effect, transform.position, transform.rotation);
        Destroy(effectIns, 0.5f);
        
        if (economy != null){
            economy.AddMoney(20);
        }


        //AOE Effect
        
        if(expolsionRadius > 0f){
            Explode();
        }
        else{
            Damage(target);
        }
        
        Destroy(gameObject);    
        return;
    }

    //For AOE affect
    void Explode(){
        Collider[] colliders = Physics.OverlapSphere(transform.position, expolsionRadius);
        foreach (Collider collider in colliders){
            Debug.Log("Detected: " + collider.name);
            if (collider.tag == "Enemy"){
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy){
       Enemy e = enemy.GetComponent<Enemy>();
        
        if ( e != null){
             e.TakeDamage(damage);
        }
       
    }


    void OnDrawGizmosSelected(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, expolsionRadius);
    }

    
}