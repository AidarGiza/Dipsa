using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public int Level
    {
        get => level;
        set
        {
            spawnPeriod = spawnPeriodBase - value * 5 * 0.001f;
            speedFactor = 1 + value * 5 * 0.01f;
            levelLabel.GetComponent<Text>().text = value.ToString();
            level = value;
        }
    }
    private int level;

    public float spawnPeriod;
    float spawnPeriodBase;
    public GameObject enemy;
    public GameObject core;
    public GameObject gameOverCanvas;
    public GameObject ui;
    public GameObject scoreLabel;
    public GameObject levelLabel;
    public GameObject scoreManager;
    public GameObject timersPanel;
    public GameObject timerObject;
    public int objectSpawnRareness;

    public GameObject immortalityObject;
    public GameObject speedUpObject;
    public GameObject slowDownObject;
    public GameObject coinObject;
    public GameObject dangerObject;

    public GameObject PausePanel;

    public int score = 0;
    public int destroyedEnemies;
    public int killsToLevelUp = 7;
    public float speedFactor = 1;
    
    
    bool gameIsOver;
    private float counter;
    private bool oneClick;
    private float doubleClickTime;
    private float doubleClickDelay = 0.5f;
    private List<BonusTimer> speedUpBonusTimers;
    private List<BonusTimer> slowDownBonusTimers;
    private List<BonusTimer> immBonusTimers;

    
    void Start()
    {
        gameOverCanvas.SetActive(false);
        ui.SetActive(true);
        spawnPeriodBase = spawnPeriod;
        scoreLabel.GetComponent<Text>().text = score.ToString();
        levelLabel.GetComponent<Text>().text = level.ToString();
        scoreManager = GameObject.Find("ScoreManager");
        core.GetComponent<Core>().onBonusHit += DoBonusAction;
        speedUpBonusTimers = new List<BonusTimer>();
        slowDownBonusTimers = new List<BonusTimer>();
        immBonusTimers = new List<BonusTimer>();
    }
    
    void Update()
    {
        if (!gameIsOver)
        {
            counter += Time.deltaTime;
            if (counter > spawnPeriod)
            {
                counter = 0;
                SpawnEnemy();
            }

            int number1 = Random.Range(0, objectSpawnRareness);
            int number2 = Random.Range(0, objectSpawnRareness);

            if (number1 == number2)
            {
                byte objectNumber = (byte)Random.Range(0, 5);
                GameObject obj = null;
                switch (objectNumber)
                {
                    case 0:
                        {
                            obj = immortalityObject;
                        }
                        break;
                    case 1:
                        {
                            obj = slowDownObject;
                        }
                        break;
                    case 2:
                        {
                            obj = speedUpObject;
                        }
                        break;
                    case 3:
                        {
                            obj = coinObject;
                        }
                        break;
                    case 4:
                        {
                            obj = dangerObject;
                            obj.GetComponent<Danger>().damage = Random.Range(10f, 30f);
                        }
                        break;
                    default:
                        break;
                }
                SpawnObject(obj);
            }

            if (speedUpBonusTimers.Count > 0)
            {
                for (int timerId = 0; timerId < speedUpBonusTimers.Count; timerId++)
                {
                    speedUpBonusTimers[timerId].Time -= Time.deltaTime;
                    if (speedUpBonusTimers[timerId].Time <= 0)
                    {
                        speedFactor -= 0.25f;
                        RelfreshSpeedFactorForObjects();
                        Destroy(speedUpBonusTimers[timerId].TimerUI);
                        speedUpBonusTimers.RemoveAt(timerId);
                        timerId--;
                    }
                }
            }

            if (slowDownBonusTimers.Count > 0)
            {
                for (int timerId = 0; timerId < slowDownBonusTimers.Count; timerId++)
                {
                    slowDownBonusTimers[timerId].Time -= Time.deltaTime;
                    if (slowDownBonusTimers[timerId].Time <= 0)
                    {
                        speedFactor += 0.25f;
                        RelfreshSpeedFactorForObjects();
                        Destroy(slowDownBonusTimers[timerId].TimerUI);
                        slowDownBonusTimers.RemoveAt(timerId);
                        timerId--;
                    }
                }
            }

            if (immBonusTimers.Count > 0)
            {
                for (int timerId = 0; timerId < immBonusTimers.Count; timerId++)
                {
                    immBonusTimers[timerId].Time -= Time.deltaTime;
                    if (immBonusTimers[timerId].Time <= 0)
                    {
                        if (immBonusTimers.Count == 1) core.GetComponent<Core>().SetImmortality(false);
                        Destroy(immBonusTimers[timerId].TimerUI);
                        immBonusTimers.RemoveAt(timerId);
                        timerId--;
                    }
                }
            }

#if UNITY_ANDROID && !UNITY_EDITOR
            foreach (var touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    ViewHits(ray);
                }
            }
#else
            // Общие действия при нажатии левой кнопки мыши
            if (Input.GetMouseButtonDown(0))
            {
                // Пустить луч от курсора
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
              
                ViewHits(ray);
            }
#endif
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }

            scoreLabel.GetComponent<Text>().text = score.ToString();

            if (core.GetComponent<Core>().leftHealth <= 0 && core.GetComponent<Core>().upHealth <= 0 && core.GetComponent<Core>().rightHealth <= 0 && core.GetComponent<Core>().downHealth <= 0)
            {
                Destroy(core);
                Over();
            }

            if (oneClick)
            {
                if (Time.time > (doubleClickTime + doubleClickDelay)){
                    oneClick = false;    
                }
            }
        }
    }

    void ViewHits(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Уничтожить объект, если нажатый объект - враг
            if (hit.transform.gameObject.tag == "Enemy" || hit.transform.gameObject.tag == "BadBonus")
            {
                score++;
                destroyedEnemies++;
                if (destroyedEnemies % 7 == 0) Level = destroyedEnemies / 7;
                if (hit.transform.gameObject.tag == "Enemy") hit.transform.gameObject.GetComponent<Enemy>().Kill();
                else Destroy(hit.transform.gameObject);
            }
            if (hit.transform.gameObject.tag == "GoodBonus")
            {
                DoBonusAction(hit.transform.gameObject);
                Destroy(hit.transform.gameObject);
            }
            if (hit.transform.gameObject == core)
            {
                if (!oneClick)
                {
                    oneClick = true;
                    doubleClickTime = Time.time;
                }
                else
                {
                    oneClick = false;
                    Pause();
                }
                
            }
        }
        else
        {
            if (score > 0)
            {
                score--;
            }
        }
    }

    /// <summary>
    /// Действия при проигрыше
    /// </summary>
    void Over()
    {
        if (scoreManager != null)
        {
            if (scoreManager.GetComponent<ScoreManager>().highscore < score)
            {
                scoreManager.GetComponent<ScoreManager>().SaveScore(score);
            }
        }
        gameIsOver = true;
        ui.SetActive(false);
        gameOverCanvas.SetActive(true);
    }

    /// <summary>
    /// Спавнит врага на случайной позиции за пределами экрана со случайной скоростью
    /// </summary>
    void SpawnEnemy()
    {
        // Спавн в рандомной точке за экраном
        Vector3 spawnPoint = GetRandom2DPoint();
        float enemyScale = Random.Range(0.8f, 1.4f);
        enemy.GetComponent<MovingObject>().speed = enemy.GetComponent<Enemy>().normalSpeed * (2 - enemyScale);
        enemy.GetComponent<Enemy>().damage = enemy.GetComponent<Enemy>().normalDamage * enemyScale;
        enemy.GetComponent<MovingObject>().speedFactor = speedFactor;
        //enemy.transform.localScale = new Vector3(enemy.transform.localScale.x * enemyScale, enemy.transform.localScale.y * enemyScale, enemy.transform.localScale.z * enemyScale);
        //enemy.GetComponent<MovingObject>().speed = Random.Range(15,30); 
        //enemy.GetComponent<Enemy>().damage = Random.Range(6, 12);
        GameObject copy = Instantiate(enemy, spawnPoint,new Quaternion());
    }

    void SpawnObject(GameObject objectToSpawn)
    {
        // Спавн в рандомной точке за экраном
        Vector3 spawnPoint = GetRandom2DPoint();

        if (objectToSpawn.GetComponent<MovingObject>().speed == 0) objectToSpawn.GetComponent<MovingObject>().speed = 20;
        objectToSpawn.GetComponent<MovingObject>().speedFactor = speedFactor;
        Instantiate(objectToSpawn, spawnPoint, new Quaternion());
    }

    Vector3 GetRandom2DPoint()
    {
        var spX = Random.Range(-80f, 80f);
        float spY;
        if ((spX >= 60 && spX <= 80) || (spX <= -60 && spX >= -80))
        {
            spY = Random.Range(-60, 60);
        }
        else
        {
            spY = Random.Range(40, 60) * Mathf.Round(Random.Range(0, 2) * 2 - 1);
        }

        return new Vector3(spX, 0, spY);
    }

    void DoBonusAction(GameObject bonus)
    {
        foreach (var component in bonus.GetComponents<MonoBehaviour>())
        {
            if (component is SpeedUp)
            {
                speedFactor += 0.25f;
                RelfreshSpeedFactorForObjects();
                GameObject tUI = Instantiate(timerObject, parent: timersPanel.transform, false);
                tUI.GetComponent<Text>().color = Color.red;
                speedUpBonusTimers.Add(new BonusTimer() { TimerUI = tUI, Time = (component as SpeedUp).duration });
            }
            if (component is SlowDown)
            {
                speedFactor -= 0.25f;
                RelfreshSpeedFactorForObjects();
                GameObject tUI = Instantiate(timerObject, parent: timersPanel.transform, false);
                tUI.GetComponent<Text>().color = Color.green;
                slowDownBonusTimers.Add(new BonusTimer() { TimerUI = tUI, Time = (component as SlowDown).duration });
            }
            if (component is Immortality)
            {
                core.GetComponent<Core>().SetImmortality(true);
                GameObject tUI = Instantiate(timerObject, parent: timersPanel.transform, false);
                tUI.GetComponent<Text>().color = Color.blue;
                immBonusTimers.Add(new BonusTimer() { TimerUI = tUI, Time = (component as Immortality).duration });
            }
            if (component is Coin)
            {
                if (scoreManager != null) scoreManager.GetComponent<ScoreManager>().Coins++;
            }
            if (component is Danger)
            {
                core.GetComponent<Core>().MakeCommonDamage((component as Danger).damage);
            }
        }
    }

    void RelfreshSpeedFactorForObjects()
    {
        List<GameObject> objects = new List<GameObject>();
        objects.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        objects.AddRange(GameObject.FindGameObjectsWithTag("BadBonus"));
        objects.AddRange(GameObject.FindGameObjectsWithTag("GoodBonus"));
        foreach (var obj in objects)
        {
            obj.GetComponent<MovingObject>().speedFactor = speedFactor;
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
    }

    public void Continue()
    {
        Time.timeScale = 1;
        PausePanel.SetActive(false);
    }

    public void ExitToMenu()
    {
        if (scoreManager != null)
        {
            if (scoreManager.GetComponent<ScoreManager>().highscore < score)
            {
                scoreManager.GetComponent<ScoreManager>().SaveScore(score);
            }
        }
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}