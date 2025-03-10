using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private PlayerController player;
    public static GameManager instance;
    public Text liveText;
    public Text moneyText;
    public Text moneyTextEnd;
    public Text moneyTextLose;
    public Text rocketText;
    public Text upgradeCostText;
    public int money;
    public float starWait;
    public GameObject[] enemies;
    public Boundary boundary;
    public Vector2 spawnWait;

    public int enemyCountMax = 3;//them cai nay nua 

    public float spawnWaitMin;
    public float waveWait;
    public float waveWaitMin;
    public bool gameOver = false;
    private int enemyCount = 1;
    public GameObject loseWindow;
    public GameObject winWindow;
    public GameObject bossCaller;
    public GameObject bossPrefab;
    private bool isBossSpawned = false; // Flag to track if the boss has been spawned
    public Slider starProgressBar; // Thanh tiến trình
    public int maxStars = 10; // Số sao cần để chiến thắng
    private int currentStars = 0; // Số sao đã thu thập
    public string nextLevelScreen;
    private bool isGameOver = false;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        instance = this;
        money = PlayerPrefs.GetInt("Money");
    }

    private void Start()
    {
        
        
            StartCoroutine(SpawnWave());
       

    }

    private void Update()
    {
        SetMoneyText();
        if (!isGameOver) // Ch? ki?m tra GameOver n?u ch?a k?t thúc trò ch?i
        {
            SetMoneyText();
            GameOver(); // G?i GameOver() trong Update
        }


        // Check if bossCaller is null and no boss is instantiated
        if (!isBossSpawned && bossCaller == null && GameObject.FindWithTag("Boss") == null)
        {
            // Instantiate the boss at the desired position
            Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), boundary.yMin, 0);
            Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
            isBossSpawned = true; // Set the flag to true when the boss is spawned
            Debug.Log("Boss has been instantiated at position: " + spawnPosition);
            AudioManager.instance.BossMusic();
        }
    }

    IEnumerator SpawnWave()
    {
        yield return new WaitForSeconds(starWait);
        while (!gameOver)
        {
            if (!isBossSpawned) // Only spawn enemies if the boss hasn't spawned
            {
                for (int i = 0; i < enemyCount; i++)
                {
                    GameObject enemy = enemies[Random.Range(0, enemies.Length)];
                    Vector3 spawnPosition = new Vector3(Random.Range(boundary.xMin, boundary.xMax), boundary.yMin, 0);
                    Instantiate(enemy, spawnPosition, Quaternion.identity);
                    yield return new WaitForSeconds(Random.Range(spawnWait.x, spawnWait.y));
                }
                enemyCount++;
                if (enemyCount >= enemyCountMax)
                {
                    enemyCount = enemyCountMax;
                    spawnWait.x -= 0.1f;
                    spawnWait.y -= 0.1f;
                    if (spawnWait.y <= spawnWaitMin)
                    {
                        spawnWait.y = spawnWaitMin;
                    }
                    if (spawnWait.x <= spawnWaitMin)
                    {
                        spawnWait.x = spawnWaitMin;
                    }
                    yield return new WaitForSeconds(waveWait);
                    waveWait -= 0.1f;
                    if (waveWait <= waveWaitMin)
                    {
                        waveWait = waveWaitMin;
                    }
                }
            }
            else
            {
                // If the boss is spawned, wait for a short duration before checking again
                yield return new WaitForSeconds(1f); // Adjust this as necessary
            }
        }
    }

    public void GameOver()
    {
        if (player.lives <= 0 && !isGameOver) // Ch? g?i n?u ch?a có game over
        {
            moneyTextLose.text = money.ToString();
            isGameOver = true; // ?ánh d?u là game over
            loseWindow.SetActive(true);
            Time.timeScale = 0; // D?ng th?i gian
            AudioManager.instance.LoseMusic();
            StopAllCoroutines();
        }
        else
        {
            Time.timeScale = 1; // ??m b?o th?i gian v?n ch?y n?u ch?a game over
        }
    }
    public void GameWon()
    {
        moneyTextEnd.text = money.ToString();
        winWindow.SetActive(true); // Hiển thị UI chiến thắng
        Time.timeScale = 0; // Dừng game
        AudioManager.instance.VictoryMusic();

        StopAllCoroutines(); // Dừng tất cả Coroutine (bao gồm SpawnWave)
    }

    public void ReplyLevel()
    {
        PlayerPrefs.SetInt("Money", 0); // Đặt tiền về 0
        PlayerPrefs.Save(); // Lưu lại giá trị mới
        //Play again the scene theat we are currently inside of it
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        AudioManager.instance.LevelMusic();
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(nextLevelScreen);
        AudioManager.instance.LevelMusic();
    }
    public void BackToMainMenu()
    {
        //Load the scene with index zero
        SceneManager.LoadScene(0);

        AudioManager.instance.LevelMusic();
    }

    public void SetLivesText(int lives)
    {
        liveText.text = "x " + lives.ToString();
    }

    public void SetMoneyText()
    {
        moneyText.text = money.ToString();
    }

    public void SetMoneyTextEnd()
    {
        moneyTextEnd.text = money.ToString();
    }

    public void SetRocketText(int rocket)
    {
        rocketText.text = "x" + rocket.ToString();
    }

    public void SetUpgradeCostText(int upgradeCost)
    {
        if (player.level < player.maxLevel)
        {
            upgradeCostText.text = upgradeCost.ToString();
        }
        else if (player.level == player.maxLevel)
        {
            upgradeCostText.text = "Max";
        }
    }

    public void AddStar()
    {
        currentStars++;
        starProgressBar.value = (float)currentStars / maxStars; // Cập nhật thanh tiến trình

        if (currentStars >= maxStars)
        {
            GameWon(); // Kích hoạt chiến thắng
        }
    }
}
