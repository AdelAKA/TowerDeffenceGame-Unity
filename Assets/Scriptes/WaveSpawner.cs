using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class WaveSpawner : MonoBehaviour
{
    public Transform normalEnemyPrefab;
    static Transform bossEnemySpawnPrefab;
    public Transform fastEnemyPrefab;
    public Transform bossEnemyPrefab;

    private int normalEnemyCount = 0;
    private int fastEnemyCount = 0;
    private int doubleEnemyCount = 0;
    private int bossEnemyCount = 0;
    private bool finishSpawn = true;
    private bool spawnEnemies = true;

    [HideInInspector]
    private Enemy normalEnemy;
    private Enemy fastEnemy;
    private Enemy bossEnemy;

    public Transform spawnPoint;

    public float timeBetweenWaves = 5f;
    private float countdown;

    public Text[] waveCountdownText;
    public Text missileLauncherCountText;
    public Text laserBeamerCountText;

    private int waveIndex = 1;

    void Start()
    {
        normalEnemy = normalEnemyPrefab.GetComponent<Enemy>();
        fastEnemy = fastEnemyPrefab.GetComponent<Enemy>();
        bossEnemy = bossEnemyPrefab.GetComponent<Enemy>();

        bossEnemySpawnPrefab = normalEnemyPrefab;

        countdown = timeBetweenWaves;
    }

    void Update()
    {
        if (Input.GetKeyDown("o")) spawnEnemies = !spawnEnemies;
        if (Input.GetKeyDown("y")) { waveIndex++; PlayerStatus.Rounds++; UpdateStatus(); }

        missileLauncherCountText.text = PlayerStatus.missileLauncherBuildOwn.ToString();
        laserBeamerCountText.text = PlayerStatus.laserBeamerBuildOwn.ToString();
        if (!spawnEnemies) return;

        if (countdown <= 0f && finishSpawn)
        {
            ArrayList enemiesToSpawn = ManageSpawingEnemies();
            StartCoroutine(SpawnWave(enemiesToSpawn));

            waveIndex++;
            PlayerStatus.Rounds++;
            UpdateStatus();
        }

        countdown -= Time.deltaTime;

        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        foreach (Text wave in waveCountdownText)
        {
            wave.text = string.Format("{0:00.00}", countdown);
        }
    }

    void UpdateStatus()
    {
        int currentWaveIndex = waveIndex - 1;

        countdown = currentWaveIndex % 10 != 0 ? timeBetweenWaves : timeBetweenWaves * 2;
        PlayerStatus.Money += currentWaveIndex % 10 == 0 ? 100 : (currentWaveIndex % 10) * 10;

        if (currentWaveIndex % 2 == 0)
        {
            // PlayerStatus.UpgradePowerUps++;
        }
        if (currentWaveIndex % 5 == 0)
        {
            PlayerStatus.missileLauncherBuildOwn++;
        }
        if (currentWaveIndex % 10 == 0)
        {
            PlayerStatus.Lives += 5;
            PlayerStatus.laserBeamerBuildOwn++;
            // UpgradeEnemies();
        }
    }

    // How many and what enemies are going to spawn this wave
    ArrayList ManageSpawingEnemies()
    {
        finishSpawn = false;
        // enemiesToSpawn.Clear();
        ArrayList eTS = new ArrayList();
        int currentWave = waveIndex % 10;
        if ((new[] { 3, 6, 9 }).Contains(currentWave)) // Double enemies
        {
            doubleEnemyCount++;
            eTS = AddEnemies(doubleEnemyCount, null);
        }
        else if ((new[] { 2, 5, 8 }).Contains(currentWave)) // Fast Enemies
        {
            fastEnemyCount++;
            eTS = AddEnemies(fastEnemyCount, fastEnemyPrefab);
        }
        else if ((new[] { 1, 4, 7 }).Contains(currentWave)) // Normal Enemies
        {
            normalEnemyCount++;
            eTS = AddEnemies(normalEnemyCount, normalEnemyPrefab);
        }
        else if (currentWave == 0) // Boss Fight !!!
        {
            bossEnemyCount++;
            ArrayList a1 = AddEnemies(doubleEnemyCount / 2, null);
            ArrayList a2 = AddEnemies(bossEnemyCount, bossEnemyPrefab);
            eTS.AddRange(a1);
            eTS.AddRange(a2);
        }
        return eTS;
    }

    ArrayList AddEnemies(int count, Transform prefab)
    {
        ArrayList arr = new ArrayList();
        if (prefab == null)
        {
            for (int i = 0; i < count; i++)
            {
                arr.Add(fastEnemyPrefab);
                arr.Add(normalEnemyPrefab);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                arr.Add(prefab);
            }
        }
        return arr;
    }

    IEnumerator SpawnWave(ArrayList enemiesToSpawn)
    {
        foreach (Transform enemy in enemiesToSpawn)
        {
            SpawnEnemy(enemy);
            if (enemy == bossEnemyPrefab)
                yield return new WaitForSeconds(0.7f);
            else
                yield return new WaitForSeconds(0.5f);
        }

        finishSpawn = true;
    }

    void SpawnEnemy(Transform prefab)
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }

    // void UpgradeEnemies()
    // {
    //     Debug.Log("Enemies Upgraded!");
    //     normalEnemy.Upgrade();
    //     fastEnemy.Upgrade();
    //     bossEnemy.Upgrade();
    //     Debug.Log("new Helth " + normalEnemy.startHealth);
    // }
}
