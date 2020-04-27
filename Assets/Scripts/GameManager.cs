using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverUI;
    public GameObject enemy;
    public GameObject tool;
    public TMP_Text scoreText;
    public Image energyBar;
    private int wave = 1;
    private int score = 0;
    internal int xLimit = 70;
    internal int zLimit = 50;
    public bool isGameOver = false;
    public bool isMenu = true;
    public GameObject distanceWarning;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver && !isMenu)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
            {
                StartWave();
                wave++;
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level1", LoadSceneMode.Single);
    }

    private void StartWave()
    {
        for(int i = 0; i < wave; i++)
        {
            Instantiate(enemy, RandomPosition(0), enemy.transform.rotation);
        }
        if (wave % 2 == 0)
        {
            Instantiate(tool, RandomPosition(10), tool.transform.rotation);
        }
    }

    internal void DecreaseLife(int v)
    {
        var newEnergy = energyBar.fillAmount - (v / 100.0f);
        if(newEnergy <= 0)
        {
            newEnergy = 0;
            isGameOver = true;
            gameoverUI.SetActive(true);
        }
        energyBar.fillAmount = newEnergy;
    }

    internal void IncreaseLife(int v)
    {
        var newEnergy = energyBar.fillAmount + (v / 100.0f);
        if (newEnergy > 1.0f)
        {
            newEnergy = 1.0f;
        }
        energyBar.fillAmount = newEnergy;
    }

    internal void IncreaseScore(int v)
    {
        score += v;
        scoreText.text = "Score: " + score;
    }

    internal void ShowDistanceWarning(bool v)
    {
        distanceWarning.SetActive(v);
    }

    private Vector3 RandomPosition(float y)
    {
        return new Vector3(Random.Range(-xLimit, xLimit), y, Random.Range(-zLimit, zLimit));
    }
}
