using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RewardPanel : MonoBehaviour
{
    [SerializeField] GameObject panel;
    [SerializeField] Text timeText;
    [SerializeField] Text rewardText;
    [SerializeField] Button btnOK;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            btnOK.onClick.AddListener(() => panel.transform.DOScale(0, 0.2f).OnComplete(() => {
                Destroy(gameObject);
                GameManager.Instance.isOpenRewardTap = false;
            }));
        }
    }

    public void InitRewardPanel(double time, long reward)
    {

        string stringReward = "";
        if (GameManager.Instance.GetMoney().ToString().Length >= 13) // 1000000000000의자리 = 1T으로 표시
        {
            stringReward = $"{(reward / 1000000000000).ToString("N2")} m";
        }
        if (GameManager.Instance.GetMoney().ToString().Length >= 10) // 1000000000의자리 = 1B으로 표시
        {
            stringReward = $"{(reward / 1000000000).ToString("N2")} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 7) // 1000000의자리 = 1m으로 표시
        {
            stringReward = $"{(reward / 1000000).ToString("N2")} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 4) // 1000의자리 = 1k으로 표시
        {
            stringReward = $"{(reward / 1000).ToString("N2")} k";
        }
        else
        {
            stringReward = reward.ToString();
        }

        timeText.text = $"{time.ToString("N2")}시간 만의 방문이네요!";
        rewardText.text = $"여기, {stringReward} 의 보상이에요.\n열심히 일했습니닷!";
        btnOK.onClick.AddListener(() => panel.transform.DOScale(0, 0.2f).OnComplete(() => {
            GameManager.Instance.AddMoney(reward);
            GameManager.Instance.isOpenRewardTap = false;
            Destroy(gameObject);
        }));
    }
}
