using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Core : MonoBehaviour
{
    public float health = 100;

    public float leftHealth = 100;
    public float upHealth = 100;
    public float rightHealth = 100;
    public float downHealth = 100;

    public GameObject uiHealth;

    public GameObject uiLeftHealth;
    public GameObject uiUpHealth;
    public GameObject uiRightHealth;
    public GameObject uiDownHealth;

    private bool immortality = false;

    public delegate void BonusHit(GameObject sender);
    
    public event BonusHit onBonusHit;

    void Start()
    {
        UpdateUI();
    }

    private void FixedUpdate()
    {

    }

    public void MakeDamage(float damage, string side)
    {
        if (!immortality)
        {
            switch (side)
            {
                case "LeftTrigger":
                    {
                        if (leftHealth > 0) leftHealth -= damage;
                        if (upHealth > 0) upHealth -= damage * 0.5f;
                        if (rightHealth > 0) rightHealth -= damage * 0.25f;
                        if (downHealth > 0) downHealth -= damage * 0.5f;
                    }
                    break;
                case "UpTrigger":
                    {
                        if (leftHealth > 0) leftHealth -= damage * 0.5f;
                        if (upHealth > 0) upHealth -= damage;
                        if (rightHealth > 0) rightHealth -= damage * 0.5f;
                        if (downHealth > 0) downHealth -= damage * 0.25f;
                    }
                    break;
                case "RightTrigger":
                    {
                        if (leftHealth > 0) leftHealth -= damage * 0.25f;
                        if (upHealth > 0) upHealth -= damage * 0.5f;
                        if (rightHealth > 0) rightHealth -= damage;
                        if (downHealth > 0) downHealth -= damage * 0.5f;
                    }
                    break;
                case "DownTrigger":
                    {
                        if (leftHealth > 0) leftHealth -= damage * 0.5f;
                        if (upHealth > 0) upHealth -= damage * 0.25f;
                        if (rightHealth > 0) rightHealth -= damage * 0.5f;
                        if (downHealth > 0) downHealth -= damage;
                    }
                    break;
            }
            if (leftHealth < 0) leftHealth = 0;
            if (upHealth < 0) upHealth = 0;
            if (rightHealth < 0) rightHealth = 0;
            if (downHealth < 0) downHealth = 0;
            UpdateUI();
            ChangeModel();
        }
    }

    public void MakeCommonDamage(float damage)
    {
        leftHealth -= damage;
        upHealth -= damage;
        rightHealth -= damage;
        downHealth -= damage;
        UpdateUI();
        ChangeModel();
    }

    void UpdateUI()
    {
        uiHealth.GetComponent<Text>().text = Mathf.CeilToInt(health).ToString();

        uiLeftHealth.GetComponent<Text>().text = Mathf.CeilToInt(leftHealth).ToString();
        uiUpHealth.GetComponent<Text>().text = Mathf.CeilToInt(upHealth).ToString();
        uiRightHealth.GetComponent<Text>().text = Mathf.CeilToInt(rightHealth).ToString();
        uiDownHealth.GetComponent<Text>().text = Mathf.CeilToInt(downHealth).ToString();
    }

    /// <summary>
    /// Изменение модельки аватара "на лету"
    /// </summary>
    void ChangeModel()
    {

    }

    /// <summary>
    /// Присвоить флаг бессмертия
    /// </summary>
    /// <param name="isImmortal">флаг бессмертия</param>
    public void SetImmortality(bool isImmortal)
    {
        immortality = isImmortal;
    }

    public void SendBonusActionToManager(GameObject bonus)
    {
        onBonusHit(bonus);
    }
}
