using UnityEngine;
using System.Collections;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject fastEnemyPrefab;
    public GameObject tankEnemyPrefab;
    public GameObject bossEnemyPrefab;
    public float spawnRadius = 10f;
    public int enemiesPerWave = 5;
    public int maxSpawnAtOnce = 6;
    public float timeBetweenSpawns = 1.5f;
    public TextMeshProUGUI waveAnnouncementText;

    private int waveNumber = 0;
    private bool waveActive = false;
    private bool waitingForUpgrade = false;

    void Start()
    {
        StartCoroutine(StartWaveWithCountdown());
    }

    void Update()
    {
        if (waitingForUpgrade) return;
        if (!waveActive) return;

        bool noEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length == 0;
        bool noBoss = GameObject.FindGameObjectsWithTag("Boss").Length == 0;

        if (noEnemies && noBoss)
        {
            waveActive = false;
            waitingForUpgrade = true;
            bool isBossWave = waveNumber % 5 == 0;
            bool isUpgradeWave = waveNumber % 2 == 0 && !isBossWave;

            if (isBossWave || isUpgradeWave)
                UpgradeManager.Instance.ShowUpgrades(isBossWave);
            else
                SetWaitingForUpgrade(false);
        }
    }

    IEnumerator StartWaveWithCountdown()
    {
        waveNumber++;
        GameManager.Instance.UpdateWave(waveNumber);

        bool isBossWave = waveNumber % 5 == 0;

        waveAnnouncementText.gameObject.SetActive(true);

        if (isBossWave)
        {
            waveAnnouncementText.color = Color.red;
            waveAnnouncementText.text = "BOSS WAVE!";
            yield return new WaitForSeconds(2f);
        }
        else
        {
            waveAnnouncementText.color = Color.yellow;
            waveAnnouncementText.text = "Wave " + waveNumber;
            yield return new WaitForSeconds(1f);
        }

        waveAnnouncementText.text = "3";
        yield return new WaitForSeconds(1f);
        waveAnnouncementText.text = "2";
        yield return new WaitForSeconds(1f);
        waveAnnouncementText.text = "1";
        yield return new WaitForSeconds(1f);

        waveAnnouncementText.gameObject.SetActive(false);
        waveActive = true;

        if (isBossWave)
        {
            SpawnBoss();
            int extraEnemies = Mathf.Min(waveNumber, 10);
            StartCoroutine(SpawnEnemiesGradually(extraEnemies));
        }
        else
        {
            int enemiesToSpawn = enemiesPerWave + (waveNumber * 2);
            StartCoroutine(SpawnEnemiesGradually(enemiesToSpawn));
        }
    }

    void SpawnBoss()
    {
        Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
        GameObject bossObj = Instantiate(bossEnemyPrefab, spawnPos, Quaternion.identity);
        bossObj.GetComponent<Boss>().Init(waveNumber);
    }

    IEnumerator SpawnEnemiesGradually(int total)
    {
        int spawned = 0;

        while (spawned < total)
        {
            int batch = Mathf.Min(maxSpawnAtOnce, total - spawned);

            for (int i = 0; i < batch; i++)
            {
                Vector2 spawnPos = Random.insideUnitCircle.normalized * spawnRadius;
                GameObject prefabToSpawn = ChooseEnemyPrefab();
                Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
                spawned++;
            }

            if (spawned < total)
                yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    GameObject ChooseEnemyPrefab()
    {
        if (waveNumber < 3) return enemyPrefab;

        int rand = Random.Range(0, 10);
        if (waveNumber >= 6 && rand < 2) return tankEnemyPrefab;
        if (rand < 4) return fastEnemyPrefab;
        return enemyPrefab;
    }

    public void SetWaitingForUpgrade(bool waiting)
    {
        waitingForUpgrade = waiting;
        if (!waiting) StartCoroutine(StartWaveWithCountdown());
    }
}