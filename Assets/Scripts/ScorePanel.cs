using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    public GameObject scoreLabel;
    public GameObject scoreManager;

    void Awake()
    {
        scoreManager = GameObject.Find("ScoreManager");
    }

    public void UpdateScoreLabel()
    {
        scoreLabel.GetComponent<Text>().text = scoreManager.GetComponent<ScoreManager>().highscore.ToString();
    }
}
