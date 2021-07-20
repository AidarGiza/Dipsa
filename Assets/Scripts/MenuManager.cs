using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject coinsLabel;
    public GameObject scoreManager;

    void Start()
    {
        scoreManager = GameObject.Find("ScoreManager");
        coinsLabel.GetComponent<Text>().text = scoreManager.GetComponent<ScoreManager>().Coins.ToString();
    }

}
