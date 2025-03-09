using UnityEngine;
public class Bullet : MonoBehaviour
{
    private Transform target;
    public float speed = 30f;
    public GameObject effect;
    private Economy economy;

    void Start()
    {
        economy = FindObjectOfType<Economy>();
    }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = target.position - transform.position;
        float distanceOfFrame = speed * Time.deltaTime;

        if (direction.magnitude <= distanceOfFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(direction.normalized * distanceOfFrame, Space.World);
    }

    void HitTarget()
    {
        GameObject effectIns = (GameObject)Instantiate(effect, transform.position, transform.rotation);
        Destroy(effectIns, 0.5f);

        if (economy != null)
        {
            economy.AddMoney(20);
        }

        Destroy(target.gameObject);
        Destroy(gameObject);
    }
}