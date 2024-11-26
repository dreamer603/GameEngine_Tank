using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    public static LoadSceneManager Instance;
    private bool _isGameEnd;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _isGameEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isGameEnd && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("GameStartScene");
        }
    }

    public void GameStart()
    {
        SceneManager.LoadScene("MainScene");
    }
    
    public void GameOver()
    {
        SceneManager.LoadScene("GameOverScene");
        _isGameEnd = true;
    }

    public void GameClear()
    {
        SceneManager.LoadScene("GameClearScene");
        _isGameEnd = true;
    }
}
