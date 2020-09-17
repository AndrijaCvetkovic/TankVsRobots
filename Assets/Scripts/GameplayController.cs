using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayController : MonoBehaviour
{
    public Enemies enemiesList;
    public int score = 0;
    public Text scoreText;
    public Text gameOvertext;
    public PlayerController player_contoller;
    public GameObject actionBar;
    public GameObject armorInfo;
    public GameObject ammoInfo;

    public Boosts boosts;
    public GameObject boostToTakePrefab;

    public int timerCounter = 0;
    public float levelBonus = 1;
    public int numberOfEnemiesInWeave = 2;
    public int enemiesLvl = 1;
    public bool gameOver = false;
    public int maxEnemyLvl = 0;

    Vector2 prevBoostCordinates;

    public GameObject resultWindow;
    public Text coins;

    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.instance.initNewSources();
        Time.timeScale = 1;
        StartCoroutine("waitForSecondThenIncreaseTimer");
        spawnEnemies(numberOfEnemiesInWeave);

        //test
        SpawnUsableBoost();
        SpawnUnusableBoost();
        prevBoostCordinates = new Vector2(0.5f, 0.5f);
        maxEnemyLvl = enemiesList.returnMaxLvlEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = score.ToString();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseGame();
        }

    }

    IEnumerator waitForSecondThenIncreaseTimer()
    {
        while (gameOver == false)
        {
            yield return new WaitForSeconds(5);
            spawnEnemies(numberOfEnemiesInWeave);
            timerCounter++;
            if (timerCounter % 24 == 0)
            {
                numberOfEnemiesInWeave++;
                SpawnUnusableBoost();
                if (enemiesLvl > 2)
                    SpawnUsableBoost();

            }
            //inicijalno na %48 menjam sada na 24
            if (timerCounter % 48 == 0)
            {
                if (maxEnemyLvl >= enemiesLvl + 1)
                    enemiesLvl++;
                numberOfEnemiesInWeave--;
                SpawnUsableBoost();
            }

        }
    }

    public void spawnEnemies(int numberOfEnemies)
    {

        float prevXpos = 0, prevYpos = 0;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int randomIndex;
            bool foundRightOne = false;
            int lvlOfEnemy = 0;
            while (foundRightOne == false)
            {
                
                randomIndex = Random.Range(0, enemiesList.enemies.Count);
                lvlOfEnemy = enemiesList.enemies[randomIndex].GetComponent<Enemy>().level;

                if (enemiesLvl > 2)
                {
                    while (lvlOfEnemy < enemiesLvl - 1)
                    {
                        randomIndex = Random.Range(0, enemiesList.enemies.Count);
                        lvlOfEnemy = enemiesList.enemies[randomIndex].GetComponent<Enemy>().level;
                    }
                }

                if (lvlOfEnemy <= enemiesLvl)
                {
                    bool foundRightPosition = false;

                    GameObject enemy = Instantiate(enemiesList.enemies[randomIndex]);
                    float randomX, randomY;
                    int randomTmp;

                    System.Random rnd = new System.Random();

                    randomX = Random.Range(0, 10) / 10f;
                    randomY = Random.Range(0, 10) / 10f;

                    randomTmp = Random.Range(0, 10) % 2;
                    if (randomTmp == 0)
                    {
                        randomTmp = Random.Range(0, 10) % 2;
                        if (randomTmp == 0)
                        {
                            randomX = 0;
                        }
                        else
                            randomX = 1;
                    }
                    else
                    {
                        randomTmp = Random.Range(0, 10) % 2;
                        if (randomTmp == 0)
                        {
                            randomY = 0;
                        }
                        else
                            randomY = 1;
                    }

                    //Debug.Log("Vratio sam: " + randomX.ToString() + " " + randomY.ToString());

                    enemy.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(randomX, randomY, 0));
                    enemy.transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0f);

                    prevXpos = randomX;
                    prevYpos = randomY;

                    foundRightOne = true;
                }
            }
        }
    }

    public void GameOver()
    {
        
        gameOver = true;
        gameOvertext.gameObject.SetActive(true);
        coins.text = (score / 10).ToString();
        resultWindow.SetActive(true);
        actionBar.SetActive(false);
        armorInfo.SetActive(false);
        ammoInfo.SetActive(false);
        StartCoroutine("showResultThenGoToMainScene");
    }

    public void HighScoreShow()
    {
        scoreText.text = "NEW HIGH SCORE: " + score.ToString();
    }


    //desava se da se na isto mesto stvore 2 ili vise itema zato ne moze da se uzmu
    public void SpawnUsableBoost()
    {
        //boostToTakePrefab

        int boostIndex = UnityEngine.Random.Range(0, (boosts.usableBoosts.Count - 1) * 10);
        usableBoost ub = boosts.usableBoosts[boostIndex % boosts.usableBoosts.Count];
        

        GameObject g = GameObject.Instantiate(boostToTakePrefab);
        float xPos = Random.Range(1, 9) / 10f;
        float yPos = Random.Range(1, 9) / 10f;

        g.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 0));
        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, 0);

        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ub.icon;

        g.GetComponent<Boost>().usable_Boost = ub;

        g.GetComponent<Boost>().usable = true;

        prevBoostCordinates.x = xPos;
        prevBoostCordinates.y = yPos;

    }

    public void SpawnUnusableBoost()
    {
        //int boostIndex = UnityEngine.Random.Range(0, (boosts.usableBoosts.Count - 1) * 10);
        int boostIndex = UnityEngine.Random.Range(0, boosts.unusableBoosts.Count);
        if (boostIndex == boosts.unusableBoosts.Count)
            boostIndex -= 1;
        //unusableBoost ub = boosts.unusableBoosts[boostIndex % boosts.usableBoosts.Count];
        unusableBoost ub = boosts.unusableBoosts[boostIndex];
        //Debug.Log("NASO SAM " + boostIndex + "   " + (boosts.unusableBoosts.Count - 1));
        GameObject g = GameObject.Instantiate(boostToTakePrefab);
        float xPos = Random.Range(1, 9) / 10f;
        float yPos = Random.Range(1, 9) / 10f;

        while(prevBoostCordinates.x == xPos && yPos == prevBoostCordinates.y)
        {
            xPos = Random.Range(1, 9) / 10f;
            yPos = Random.Range(1, 9) / 10f;
        }

        g.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(xPos, yPos, 0));
        g.transform.position = new Vector3(g.transform.position.x, g.transform.position.y, 0);

        g.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = ub.icon;

        g.GetComponent<Boost>().unusable_Boost = ub;

        g.GetComponent<Boost>().usable = false;

        prevBoostCordinates.x = xPos;
        prevBoostCordinates.y = yPos;

    }

    IEnumerator showResultThenGoToMainScene()
    {
        //resultWindow.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MenuScene");
    }

    public void pauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void unpauseBtnClick()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }

    public void goBackToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

}
