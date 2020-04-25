using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Text _restartText;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Sprite[] _livesSprite;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore(0);
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if(_gameManager == null)
        { Debug.Log("Error: GameManager is null!"); }
        ShowGameOver(false);
        _restartText.enabled = false;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int lives)
    {
        _livesImage.sprite = _livesSprite[lives];
    }

    public void ShowGameOver(bool show)
    {
        _gameOverText.enabled = show;
        if (show)
        {
            StartCoroutine("BlinkGameOverText");
            _restartText.enabled = true;
            _gameManager.GameOver();
        }
    }

    IEnumerator BlinkGameOverText()
    {
        bool onOff = false;
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            _gameOverText.enabled = onOff;
            onOff = !onOff;
        }
    }
}
