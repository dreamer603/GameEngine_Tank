using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Objects")]
    public GameObject zombie;
    public GameObject survivor;
    
    [Header("SpawnPoints")]
    public Transform[] zombieSpawnPoints;
    public Transform[] survivorSpawnPoints;
    
    private int _score = 0;
    private int _currentZombies = 0;
    private int _maxZombies = 30;
    private float _spawnCoolTime = 5f;
    private float _currentTime = 0f;
    
    // Start is called before the first frame update
    void Awake()
    {
        _score = 0;
        _currentZombies = 0;
        _currentTime = 0f;
        
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < survivorSpawnPoints.Length; i++)
        {
            survivor.transform.position = survivorSpawnPoints[i].position;
            Instantiate(survivor);
        }

        for (int i = 0; i < zombieSpawnPoints.Length; i++)
        {
            zombie.transform.position = zombieSpawnPoints[i].position;
            Instantiate(zombie);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= _spawnCoolTime && _currentZombies < _maxZombies)
        {
            int spawnPointIndex = Random.Range(0, zombieSpawnPoints.Length);
            zombie.transform.position = zombieSpawnPoints[spawnPointIndex].position;
            Instantiate(zombie);
            _currentTime = 0f;
            _currentZombies += 1;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }
}
