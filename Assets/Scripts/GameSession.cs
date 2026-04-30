using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    private static GameSession instance;

    public int score = 0;
    [SerializeField] Player player;
    [SerializeField] Text scoreText;
    public int sceneIndex;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            instance.HandoffSceneReferences(player, scoreText);
            instance.RefreshScoreText();
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        RefreshScoreText();
    }

    // Start is called before the first frame update
    void Start()
    {
        RefreshScoreText();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        RefreshScoreText();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        RefreshScoreText();
    }

    public void ProcessPlayerDeath()
    {
        ResetGameSession();
    }

    public void ProcessEnemyDeath()
    {
        Destroy(gameObject);
    }

    private void ResetGameSession()
    {
        if (instance == this)
        {
            instance = null;
        }
        Destroy(gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void HandoffSceneReferences(Player newPlayer, Text newScoreText)
    {
        if (newPlayer != null)
        {
            player = newPlayer;
        }

        if (newScoreText != null)
        {
            scoreText = newScoreText;
        }
    }

    private void RefreshScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }
}
