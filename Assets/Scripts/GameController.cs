using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    static GameController _instance;

    int _score; // keeps track of the player score within the game
    int _lives;
    int _target;
    int _enemiesKilled;
    float _multiplier;
    int _highscore;
    string _name;
    bool _paused;
    bool _dead;

    GameData data;
    HUDController _controller;

    void Start()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        _paused = false;
        _dead = false;
        _name = "Test Name";
        _score = 0;
        _lives = 3;
        _multiplier = 1;
        _target = 50;

        if (PlayerPrefs.HasKey("HighScore"))
        {
            _highscore = PlayerPrefs.GetInt("HighScore");
        }

        _controller = GameObject.Find("HUD").GetComponent<HUDController>();
        _controller.UpdateScore(_score);
        _controller.UpdateLives(_lives);
        _controller.UpdateMultiplier(_multiplier);
        _controller.UpdateTarget(_target);
        _controller.UpdateHighScore(_highscore);

        data = new GameData();
    }

    public void IncreaseScore(string enemyType)
    {
        if (enemyType == "Enemy")
        {
            _score += (int)(10 * _multiplier);
            _enemiesKilled++;
        }
        if (_enemiesKilled % 5 == 0) // how many enemies to kill before score increase
        {
            _multiplier += 0.1f;
            _controller.UpdateMultiplier(_multiplier);
        }
        if (_score > _target) // check if the player has reached the target
        { // give the player a reward?
            _target += 50;
            _controller.UpdateTarget(_target);
        }
        if(_score > _highscore)
        {
            _highscore = _score;
            PlayerPrefs.SetInt("HighScore", _highscore);
            _controller.UpdateHighScore(_highscore);
        }
        _controller.UpdateScore(_score);
    }

    public void PlayerDied()
    {
        _controller.UpdateLives(--_lives);

        if (_lives == 0)
        {
            _dead = true;
            CollectData();
        }
        else
        {
            _dead = false;
        }
    }

    void CollectData()
    {
        data.name = _name;
        data.score = _score;

        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(AnalyticManager.PostMethod(jsonData));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            if (_paused)
                Pause();
            else
                Resume();
        }

        if (_dead)
        {
            Dead();
        }
    }

    void Pause()
    {
        Time.timeScale = 0.0f;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    void Dead()
    {
        Time.timeScale = 0.0f;
        transform.GetChild(1).gameObject.SetActive(true);
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        _paused = false;
        _dead = false;
    }

    public void Exit()
    {
        Destroy(GameObject.Find("HUD"));
        Resume();
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
    }

    public void Reload()
    {
        Time.timeScale = 1.0f;
        transform.GetChild(1).gameObject.SetActive(false);
        _dead = false;
        _lives = 3;
        _controller.UpdateLives(_lives);
        _score = 0;

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
