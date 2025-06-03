using UnityEngine;
using System.Collections;

public class SpawnWave : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemiesPerWave = 5;
    [SerializeField] private float timeBetweenEnemies = 0.5f;
    [SerializeField] private float timeBeforeFirstWave = 3f;
    [SerializeField] private float timeBetweenWaves = 5f; // Tempo de espera APÓS uma onda terminar

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    private int waveNumber = 0;
    private float waveCountdown;
    private bool isSpawningWave = false; // Flag para controlar se uma onda está ativa

    void Start()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnWave: Nenhum spawn point definido!");
            enabled = false; // Desabilita o script se não houver pontos de spawn
            return;
        }
        if (enemyPrefab == null)
        {
            Debug.LogError("SpawnWave: Prefab do inimigo não definido!");
            enabled = false; // Desabilita o script se não houver prefab
            return;
        }
        waveCountdown = timeBeforeFirstWave;
    }

    void Update()
    {
        // Se uma onda já está sendo spawnada, a coroutine está cuidando disso.
        // O Update só gerencia o countdown para a próxima onda quando NENHUMA onda está ativa.
        if (isSpawningWave)
        {
            return;
        }

        if (waveCountdown <= 0f)
        {
            // Inicia a coroutine para spawnar a onda e marca que uma onda está ativa
            StartCoroutine(SpawnSingleWaveCoroutine());
            isSpawningWave = true;
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    IEnumerator SpawnSingleWaveCoroutine()
    {
        waveNumber++;
        Debug.Log("Iniciando Onda: " + waveNumber);

        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }

        Debug.Log("Onda " + waveNumber + " Completamente Spawnada.");

        // A onda terminou de spawnar. Agora configuramos o tempo para a PRÓXIMA onda.
        waveCountdown = timeBetweenWaves;
        isSpawningWave = false; // Libera para o Update começar a contagem regressiva para a próxima onda
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return; // Checagem extra de segurança
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(enemyPrefab, randomSpawnPoint.position, randomSpawnPoint.rotation);
        // Debug.Log("Inimigo spawnado em: " + randomSpawnPoint.name); // Opcional para debug
    }
}