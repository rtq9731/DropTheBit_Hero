using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopUI : MonoBehaviour
{
    [SerializeField] Text coinText;
    [SerializeField] Text killText;

    public void UpdateCurrentCoin()
    {
        GameManager.Instance.SaveData();
        if (GameManager.Instance.GetMoney().ToString().Length >= 13) // 1000000000000���ڸ� = 1T���� ǥ��
        {
            coinText.text = $"���� �� : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000000000, 2)} m";
        }
        if (GameManager.Instance.GetMoney().ToString().Length >= 10) // 1000000000���ڸ� = 1B���� ǥ��
        {
            coinText.text = $"���� �� : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000000, 2)} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 7) // 1000000���ڸ� = 1m���� ǥ��
        {
            coinText.text = $"���� �� : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000, 2)} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 4) // 1000���ڸ� = 1k���� ǥ��
        {
            coinText.text = $"���� �� : {System.Math.Round(GameManager.Instance.GetMoney() / 1000, 2)} k";
        }
        else
        {
            coinText.text = $"���� �� : {System.Math.Round(GameManager.Instance.GetMoney(), 2)}";
        }
    }

    public void UpdateCurrentKillCount()
    {
        GameManager.Instance.SaveData();
        killText.text = $"��� �� : {GameManager.Instance.KillCount}";
    }
}
