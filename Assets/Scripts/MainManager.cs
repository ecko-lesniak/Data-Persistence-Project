using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    public GameObject NewHighScoreText;
    public TMP_InputField PlayerNameInputField;

    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        var bestScore = PersistenceManager.Instance.HighScores.OrderByDescending(s => s.Score).First();
        BestScoreText.text = $"Best Score : {bestScore.PlayerName} : {bestScore.Score}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(0);
            }
        }

        if(NewHighScoreText.gameObject.activeInHierarchy && Input.GetKeyDown(KeyCode.Return) && !string.IsNullOrEmpty(PlayerNameInputField.text)) {
            // save new high score
            SaveNewHighScore(PlayerNameInputField.text, m_Points);
            SceneManager.LoadScene(2);
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if(IsNewHighScore())
        {
            // get players name
            NewHighScoreText.gameObject.SetActive(true);
        }
        else
        {
            GameOverText.SetActive(true);
            m_GameOver = true;
        }
    }

    private bool IsNewHighScore()
    {
        var highScores = PersistenceManager.Instance.HighScores;
        return highScores.Count < 3 || highScores.Exists(s => m_Points > s.Score);
    }

    private void SaveNewHighScore(string playerName, int score)
    {
        PersistenceManager.Instance.HighScores.Add(new PersistenceManager.HighScore { PlayerName = playerName, Score = score });
        PersistenceManager.Instance.HighScores = PersistenceManager.Instance.HighScores.OrderByDescending(s => s.Score).Take(3).ToList();
        PersistenceManager.Instance.SaveHighScores();
    }
}
