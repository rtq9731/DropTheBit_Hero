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
        if (GameManager.Instance.GetMoney().ToString().Length >= 13) // 1000000000000���ڸ� = 1T���� ǥ��
        {
            stringReward = $"{(reward / 1000000000000).ToString("N2")} m";
        }
        if (GameManager.Instance.GetMoney().ToString().Length >= 10) // 1000000000���ڸ� = 1B���� ǥ��
        {
            stringReward = $"{(reward / 1000000000).ToString("N2")} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 7) // 1000000���ڸ� = 1m���� ǥ��
        {
            stringReward = $"{(reward / 1000000).ToString("N2")} m";
        }
        else if (GameManager.Instance.GetMoney().ToString().Length >= 4) // 1000���ڸ� = 1k���� ǥ��
        {
            stringReward = $"{(reward / 1000).ToString("N2")} k";
        }
        else
        {
            stringReward = reward.ToString();
        }

        timeText.text = $"{time.ToString("N2")}�ð� ���� �湮�̳׿�!";
        rewardText.text = $"����, {stringReward} �� �����̿���.\n������ ���߽��ϴ�!";
        btnOK.onClick.AddListener(() => panel.transform.DOScale(0, 0.2f).OnComplete(() => {
            GameManager.Instance.AddMoney(reward);
            GameManager.Instance.isOpenRewardTap = false;
            Destroy(gameObject);
        }));
    }
}
