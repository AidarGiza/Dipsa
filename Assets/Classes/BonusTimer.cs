using UnityEngine;
using UnityEngine.UI;

class BonusTimer
{
    public float Time
    {
        get => time;
        set
        {
            TimerUI.GetComponent<Text>().text = Mathf.RoundToInt(value).ToString();
            time = value;
        }
    }
    private float time;

    public GameObject TimerUI { get; set; }
}
