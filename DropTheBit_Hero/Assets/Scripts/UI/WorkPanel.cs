using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkPanel : MonoBehaviour
{
    [SerializeField] Image workImage;
    [SerializeField] Text workName;
    [SerializeField] Text upgradeCostText;
    [SerializeField] Text currentUpgradeText;
    [SerializeField] Button upgradeBtn;
    [SerializeField] GameObject lockPanel;

    WorkData data = null;

    public void InitWorkPanel(int index)
    {
        data = GameManager.Instance.GetWorkDataByindex(index);
        workImage.sprite = Resources.Load<Sprite>(data.Image_Path);
        workName.text = data.Name;
    }

    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    public void Refresh(bool isUnlocked) // 원본함수
    {
        if(data.Isunlocked) // 이미 해금되었다면 
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked와 반대로 움직여야함.

        data.Isunlocked = isUnlocked;
        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.RoundToInt(data.Upgradecost * (0.5f * data.Upgradecount))}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시
        upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
    }
    public void Refresh() // 원본함수
    {
        if(data.Upgradecount >= data.Leastup) // 최소 업그레이드 이상 업글되면
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(data.Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.RoundToInt(data.Upgradecost * ( 0.5f * data.Upgradecount ))}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시
        upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < data.Upgradecost * data.Upgradecount) // 돈이 적으면 취소
        {
            return;
        }

        GameManager.Instance.AddMoney(-Mathf.RoundToInt(data.Upgradecost * (0.5f * data.Upgradecount)));

        ++data.Upgradecount;
        data.Yield += data.Yield * 0.1f;
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        return data.Isunlocked;
    }

}
