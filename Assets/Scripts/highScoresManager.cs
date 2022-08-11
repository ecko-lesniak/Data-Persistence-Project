using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class highScoresManager : MonoBehaviour
{
    public TextMeshProUGUI Players;
    public TextMeshProUGUI Scores;

    // Start is called before the first frame update
    void Start()
    {
        var highScores = PersistenceManager.Instance.HighScores;
        Players.text = string.Join('\n', highScores.Select(s => s.PlayerName));
        Scores.text = string.Join('\n', highScores.Select(s => s.Score));
    }

    public void ShowMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
