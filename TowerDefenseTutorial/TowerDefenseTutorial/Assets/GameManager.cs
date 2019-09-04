using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject spawnPoint;
    public Text waveCountText;
    public int waveCount;
    private int waveNumber = 1;

    void Start()
    {
        StartCoroutine("WaveCountDown");
    }

    void Update()
    {

    }

    IEnumerator WaveCountDown()
    {
        waveCountText.text = waveCount.ToString();
        for (int i = waveCount; i > 0; i--)
        {
            waveCountText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }
        waveCount = 5;

        StartCoroutine("WaveCountDown");
        StartCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < waveNumber; i++)
        {
            MakeEnemy();
            yield return new WaitForSeconds(0.5f);
        }
        waveNumber++;
    }
    private void MakeEnemy()
    {
        Instantiate<GameObject>(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
    }
}
