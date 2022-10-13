using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager game;
    private int score;
    private int scoreMilestone;
    private int health;
    public bool isAlive;
    public bool isFlipped;
    public Text scoreText;
    public Text healthText;
    public Material scoreCollectableMaterial0;
    public Material scoreCollectableMaterial1;
    public Material scoreCollectableMaterial2;
    public Material[] scoreCollectableMatArr;
    public Camera mainCamera;
    public Camera sideCamera;
    public PlayerController playerController;
    public bool isPaused;
    public GameObject pauseMenu;
    public GameObject gameOverPanel;
    private string menuSceneName = "MenuScene";
    public Text finalScoreText;
    public GameObject pauseButton;
    public GameObject jumpButton;
    public GameObject switchButton;

    private void Awake()
    {
        game = this;
        scoreCollectableMatArr = new Material[3] { scoreCollectableMaterial0, scoreCollectableMaterial1, scoreCollectableMaterial2 };
    }
    void Start()
    {
        score = 0;
        health = 3;
        isAlive = true;
        isFlipped = false;
        isPaused = false;
        scoreText.text = "" + 0;
        healthText.text = "" + 3;
    }

    public void ModifyScore(int change)
    {
        score += change;
        scoreMilestone += change;
        if(scoreMilestone >= 50)
        {
            scoreMilestone -= 50;
            playerController.forwardSpeed += playerController.speedIncrease;
        }
        scoreText.text = "" + score;
    }

    public void ModifyHealth(int change, bool debugMode)
    {
        if (health + change > 3 && !debugMode && change != -1)
            return;
        if(health + change < 0)
        {
            isAlive = false;
            LoadGameOver();
        }
        health += change;
        healthText.text = "" + health;
    }
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.Stop("MenuMusic");
        AudioManager.instance.Stop("GameplayMusic");
        AudioManager.instance.Play("GameplayMusic");
        switchButton.SetActive(true);
        jumpButton.SetActive(true);
        pauseButton.SetActive(true);
    }
    public void Pause()
    {
        AudioManager.instance.Pause("GameplayMusic");
        AudioManager.instance.Play("MenuMusic");
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;
        switchButton.SetActive(false);
        jumpButton.SetActive(false);
        pauseButton.SetActive(false);
    }
    public void Resume()
    {
        AudioManager.instance.Stop("MenuMusic");
        AudioManager.instance.UnPause("GameplayMusic");
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;
        switchButton.SetActive(true);
        jumpButton.SetActive(true);
        pauseButton.SetActive(true);
    }
    public void LoadMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(menuSceneName);
    }
    void LoadGameOver()
    {
        AudioManager.instance.Stop("GameplayMusic");
        AudioManager.instance.Play("MenuMusic");
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        if (score < 50)
            finalScoreText.text = "You scored below 50 points (" + score + "), get good!";
        else
            finalScoreText.text = "You scored " + score + " points, good job!";
        switchButton.SetActive(false);
        jumpButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void ToggleMute()
    {
        AudioManager.instance.ToggleMute();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
