using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 50f;

    public int startDamage = 50;
    public float damageIncrement = 0;
    public int damageLevel = 0;

    public float explosionRadius = 0f;
    public GameObject impactEffect;

    public void SetDamageLevel(int _damageLevel) { damageLevel = _damageLevel; }

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null || target.tag == "Untagged")
        {
            if (explosionRadius > 0f)
            {
                HitTarget();
            }
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(target);
    }

    void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 3f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(target);
        }

        Destroy(gameObject);
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }

    void Damage(Transform enemy)
    {
        Enemy e = enemy.GetComponent<Enemy>();

        if (e != null)
        {
            e.Takedamage(startDamage + (damageIncrement * damageLevel));
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
