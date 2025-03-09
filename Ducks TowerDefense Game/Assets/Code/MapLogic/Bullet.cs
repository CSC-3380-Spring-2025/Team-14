using UnityEngine;

public class Bullet : MonoBehaviour{

    private Transform target;
    public float speed = 30f;
    public GameObject effect;

    public void Seek(Transform _target){
        target = _target;
    }

    // Update is called once per frame
    void Update(){
        if(target == null){
            Destroy(gameObject);
            return;//Destroy code will sometime take some time so calling a return prevent the code from continuing until it destroy the object
        }


        Vector3 direction = target.position - transform.position;
        float distanceOfFrame = speed * Time.deltaTime;
        
        if(direction.magnitude <= distanceOfFrame){
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceOfFrame, Space.World);


    }

    void HitTarget(){
        GameObject effectIns = (GameObject)Instantiate(effect, transform.position, transform.rotation);
        Destroy(effectIns, 0.5f);
        Destroy(target.gameObject);
        Destroy(gameObject);
        return;

    }


}
