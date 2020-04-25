using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool _isGameOver;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        { RestartLevel(); }

        if (Input.GetKeyDown(KeyCode.Escape)) // if (Input.GetKey("escape"))
        { QuitGame(); }
    }

    // Restart the level.
    void RestartLevel()
    {
        SceneManager.LoadScene(1);
    }

    // Close the game.
    void QuitGame()
    {
        Application.Quit(); // OBS!: Fungerar inte i editorn! =)
    }

    // Game is over.
    public void GameOver()
    {
        _isGameOver = true;
    }
}
