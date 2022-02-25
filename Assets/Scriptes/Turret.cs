using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private Transform target;
    private Enemy targetEnemy;


    [Header("General")]
    public float range = 15f;
    public float startRange = 15f;
    public int enemyLockOnNumber = 1;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    public float startFireRate = 1f;
    public float fireRateIncrement = 0f;
    public float fireCountdown = 0f;

    [Header("Upgrades")]
    public int damageLevel = 0;
    public int rangeLevel = 0;
    public int fireRateLevel = 0;
    public int slowDownLevel = 0;

    [Header("Use Laser")]
    public bool useLaser = false;

    public int damageOverTime = 30;
    public int startDamageOverTime = 30;
    public int damageOverTimeIncrement = 1;
    public float slowAmount = .5f;
    public float startSlowAmount = .5f;
    public float slowDownIncrement = .02f;

    private bool laserOnCooldown = false;
    public float laserMaxCoolDown = 1f;
    private float laserOnCooldownCountDown = 0f;
    public float laserMaxShootTime = 3f;
    private float laserShootCountDown = 0f;

    public ParticleSystem impactEffect;
    public LineRenderer lineRenderer;
    public Light impactLight;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";
    public Transform partToRotate;
    public float turnspeed = 10f;
    public Transform firePoint;

    public int GetDamageLevel() { return damageLevel; }
    public int GetRangeLevel() { return rangeLevel; }
    public int GetFireRateLevel() { return fireRateLevel; }
    public int GetSlowDownLevel() { return slowDownLevel; }

    void Start()
    {
        range = startRange;
        fireRate = startFireRate;
        slowAmount = startSlowAmount;
        damageOverTime = startDamageOverTime;

        // damageLevel = 0;
        // rangeLevel = 0;
        // fireRateLevel = 0;
        // slowDownLevel = 0;
        UpdateValues();
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    // void UpdateTarget()
    // {
    //     GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
    //     float shortestDistance = Mathf.Infinity;
    //     GameObject nearestEnemy = null; 

    //     foreach (GameObject enemy in enemies)
    //     {
    //         float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
    //         if (distanceToEnemy < shortestDistance)
    //         {
    //             shortestDistance = distanceToEnemy;
    //             nearestEnemy = enemy;
    //         }
    //     }

    //     if (nearestEnemy != null && shortestDistance <= range)
    //     {
    //         target = nearestEnemy.transform;
    //     }
    //     else
    //     {
    //         target = null;
    //     }
    // }

    public void UpdateValues()
    {
        // Bullet damage is modified withing the bullet Script 
        range = startRange + (rangeLevel);
        fireRate = startFireRate + (fireRateLevel * fireRateIncrement);

        // Laser Stuff
        damageOverTime = startDamageOverTime + (damageLevel * damageOverTimeIncrement);
        slowAmount = startSlowAmount + (slowDownLevel * slowDownIncrement);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        int highestWaypoint = 0;
        float closestDistance = Mathf.Infinity;

        GameObject mostDangerousEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;

            EnemyMovement tempE = enemy.GetComponent<EnemyMovement>();
            int tempWayPoint = tempE.GetWayPointIndex;
            float tempDistanceLeft = tempE.GetDistanceLeft;

            if (tempWayPoint > highestWaypoint)
            {
                highestWaypoint = tempWayPoint;
                closestDistance = tempDistanceLeft;
                mostDangerousEnemy = enemy;
            }
            else if (tempWayPoint == highestWaypoint)
            {
                if (tempDistanceLeft < closestDistance)
                {
                    highestWaypoint = tempWayPoint;
                    closestDistance = tempDistanceLeft;
                    mostDangerousEnemy = enemy;
                }
            }
        }

        if (mostDangerousEnemy != null)
        {
            target = mostDangerousEnemy.transform;
            targetEnemy = mostDangerousEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
            fireCountdown = 0;
        }
    }

    void Update()
    {
        if (target == null || target.tag == "Untagged")
        {
            if (useLaser)
            {
                if (lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }

            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            if (laserOnCooldown)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;

                laserOnCooldownCountDown -= Time.deltaTime;
                if (laserOnCooldownCountDown <= 0)
                {
                    laserOnCooldown = false;
                    laserShootCountDown = laserMaxShootTime;
                }
            }
            else
            {
                laserShootCountDown -= Time.deltaTime;
                if (laserShootCountDown <= 0)
                {
                    laserOnCooldown = true;
                    laserOnCooldownCountDown = laserMaxCoolDown;

                }
                else Laser();
            }
        }
        else
        {
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
    }

    void LockOnTarget()
    {
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnspeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    void Laser()
    {
        targetEnemy.Takedamage(damageOverTime * Time.deltaTime);
        targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        Vector3 dir = transform.position - target.position;

        impactEffect.transform.position = target.position + dir.normalized * 0.5f * target.localScale.x;
        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    void Shoot()
    {
        GameObject tempBullet = bulletPrefab;
        tempBullet.GetComponent<Bullet>().SetDamageLevel(damageLevel);
        GameObject bulletGO = (GameObject)Instantiate(tempBullet, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
