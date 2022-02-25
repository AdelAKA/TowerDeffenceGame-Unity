using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float startSpeed = 10f;
    [SerializeField] public float speed;

    public float startHealth = 100f;
    [SerializeField] private float health;

    public int damage = 1;

    public int bounty = 50;

    public int damagePayback = 10;

    [Header("Boss")]
    public bool isBoss = false;

    public int startSpawnCount = 2;
    public int enemySpawnCount;

    public GameObject deathEffect;

    [Header("UnityStuff")]
    public Image healthBar;

    private int multiplayer;

    void Start()
    {
        multiplayer = (PlayerStatus.Rounds - 1) / 10;
        speed = startSpeed + startSpeed * (0.1f * multiplayer);
        health = startHealth + startHealth * (0.2f * multiplayer);
        enemySpawnCount = startSpawnCount + multiplayer;
    }

    public void Takedamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / (startHealth + startHealth * (0.2f * multiplayer));

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    void Die()
    {
        PlayerStatus.Money += bounty;

        GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (isBoss)
        {
            // WaveSpawner ws = UnityEngine.Object.FindObjectOfType<WaveSpawner>();
            // ArrayList enemiesToSpawn = new ArrayList();
            // for (int i = 0; i < enemySpawnCount; i++)
            // {
            //     Debug.Log("spawn from boss " + enemySpawnCount + i);
            //     enemiesToSpawn.Add(ws.normalEnemyPrefab);
            // }
            // StartCoroutine(WaveSpawner.SpawnWave(enemiesToSpawn, transform));

            // Transform position = transform;
            // int tempWaypoint = transform.GetComponent<EnemyMovement>().GetWayPointIndex;
            // UnityEngine.Object.
            // FindObjectOfType<WaveSpawner>().
            // SpawnEnemyFromBoss(
            //     position,
            //     enemySpawnCount,
            //     tempWaypoint
            //     );

            Debug.Log("boss dead");
            gameObject.tag = "Untagged";
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<EnemyMovement>().enabled = false;
            StartCoroutine(BossSpawn());
            Destroy(gameObject, enemySpawnCount);
        }
        else Destroy(gameObject);
    }

    IEnumerator BossSpawn()
    {
        Debug.Log("boss Spawn");
        for (int i = 0; i < enemySpawnCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        WaveSpawner ws = UnityEngine.Object.FindObjectOfType<WaveSpawner>();
        Transform e = Instantiate(ws.normalEnemyPrefab, transform.position, transform.rotation);
        e.GetComponent<EnemyMovement>().setWaypointIndex(transform.GetComponent<EnemyMovement>().GetWayPointIndex);
    }

    // public void Upgrade()
    // {
    //     health *= 1.05f;
    //     speed *= 1.02f;
    //     enemySpawnCount++;
    // }
}
