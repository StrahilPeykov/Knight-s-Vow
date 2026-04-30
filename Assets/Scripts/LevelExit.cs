using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float LevelLoadDelay = 2f;
    [SerializeField] float LevelExitSlowMoFactor = 0.2f;
    bool isTransitioning = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isTransitioning)
        {
            return;
        }

        if (!other.CompareTag("Player") && other.GetComponent<Player>() == null)
        {
            return;
        }

        isTransitioning = true;
        //calls a short delay
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()
    {
        Time.timeScale = LevelExitSlowMoFactor;
        yield return new WaitForSecondsRealtime(LevelLoadDelay);
        Time.timeScale = 1f;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //loads the next scene
        SceneManager.LoadScene(currentSceneIndex + 1); //onBossDeath = mainMenu
    }
}
