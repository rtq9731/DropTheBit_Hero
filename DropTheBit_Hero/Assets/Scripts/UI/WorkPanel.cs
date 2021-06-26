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
    [SerializeField] Slider ProgressBar;
    [SerializeField] Button upgradeBtn;
    [SerializeField] GameObject lockPanel;

    public WorkData data = null;
    WorkUI.WorkYieldTimer timer = null;

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

        if (data.Isunlocked && timer != null)
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked와 반대로 움직여야함.
        data.Isunlocked = isUnlocked;

        if (data.Isunlocked && timer == null)
        {
            timer = new WorkUI.WorkYieldTimer(data.Moneycool, data.Yield, ProgressBar);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.Round(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    public void Refresh() // 오버로딩 함수
    {
        if(data.Upgradecount >= data.Leastup) // 최소 업그레이드 이상 업글되면
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(data.Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.Round(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {data.Upgradecount}"; // 업그레이드 단계 표시

        upgradeBtn.onClick.AddListener(() => Upgrade()); // 최대로 업그레이드 되지 않았다면 업그레이드 가능
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < Mathf.RoundToInt(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1) // 돈이 적으면 취소
        {
            return;
        }

        GameManager.Instance.AddMoney(-Mathf.RoundToInt(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1);
        ++data.Upgradecount;
        data.Yield += long.Parse(Mathf.RoundToInt(data.Yield * 0.05f).ToString());
        timer.UpdateData(data.Yield);
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        return data.Isunlocked;
    }

}
