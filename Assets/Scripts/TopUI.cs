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
        if (GameManager.Instance.GetMoney().ToString().Length >= 13) // 1000000000000의자리 = 1T으로 표시
        {
            coinText.text = $"현재 돈 : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000000000, 2)} m";
        }
        if (GameManager.Instance.GetMoney().ToString().Length >= 10) // 1000000000의자리 = 1B으로 표시
        {
            coinText.text = $"현재 돈 : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000000, 2)} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 7) // 1000000의자리 = 1m으로 표시
        {
            coinText.text = $"현재 돈 : {System.Math.Round(GameManager.Instance.GetMoney() / 1000000, 2)} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 4) // 1000의자리 = 1k으로 표시
        {
            coinText.text = $"현재 돈 : {System.Math.Round(GameManager.Instance.GetMoney() / 1000, 2)} k";
        }
        else
        {
            coinText.text = $"현재 돈 : {System.Math.Round(GameManager.Instance.GetMoney(), 2)}";
        }
    }

    public void UpdateCurrentKillCount()
    {
        GameManager.Instance.SaveData();
        killText.text = $"토벌 수 : {GameManager.Instance.KillCount}";
    }
}
