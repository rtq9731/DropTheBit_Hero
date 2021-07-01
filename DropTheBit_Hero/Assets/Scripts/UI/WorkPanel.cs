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

    int index = 0;
    WorkUI.WorkYieldTimer timer = null;

    public void InitWorkPanel(int index)
    {
        this.index = index; 
        workImage.sprite = Resources.Load<Sprite>(GameManager.Instance.GetWorkDataByindex(index).Image_Path);
        workName.text = GameManager.Instance.GetWorkDataByindex(index).Name;
    }

    /// <summary>
    /// 메세지들을 새로고침 해줍니다.
    /// </summary>
    public void Refresh(bool isUnlocked) // 원본함수
    {

        if (GameManager.Instance.GetWorkDataByindex(index).Isunlocked && timer != null)
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked와 반대로 움직여야함.
        GameManager.Instance.GetWorkDataByindex(index).Isunlocked = isUnlocked;

        if (GameManager.Instance.GetWorkDataByindex(index).Isunlocked && timer == null)
        {
            timer = new WorkUI.WorkYieldTimer(GameManager.Instance.GetWorkDataByindex(index).Moneycool, GameManager.Instance.GetWorkDataByindex(index).Yield, ProgressBar);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.Round(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {GameManager.Instance.GetWorkDataByindex(index).Upgradecount}"; // 업그레이드 단계 표시

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    public void Refresh() // 오버로딩 함수
    {
        if(GameManager.Instance.GetWorkDataByindex(index).Upgradecount >= GameManager.Instance.GetWorkDataByindex(index).Leastup) // 최소 업그레이드 이상 업글되면
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(GameManager.Instance.GetWorkDataByindex(index).Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"업그레이드 비용 : {Mathf.Round(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1}원"; // 첫 업그레이드가 끝난 경우 원가 * 업그레이드 단계로 표시
        this.currentUpgradeText.text = $"현재 업그레이드 단계 : {GameManager.Instance.GetWorkDataByindex(index).Upgradecount}"; // 업그레이드 단계 표시

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < Mathf.RoundToInt(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1) // 돈이 적으면 취소
        {
            return;
        }

        GameManager.Instance.AddMoney(-Mathf.RoundToInt(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1);
        ++GameManager.Instance.GetWorkDataByindex(index).Upgradecount;
        GameManager.Instance.GetWorkDataByindex(index).Yield += long.Parse(Mathf.RoundToInt(GameManager.Instance.GetWorkDataByindex(index).Yield * 0.01f).ToString());
        timer.UpdateData(GameManager.Instance.GetWorkDataByindex(index).Yield);
        upgradeBtn.onClick.RemoveAllListeners();
        Refresh();
    }

    public bool GetIsUnlocked()
    {
        return GameManager.Instance.GetWorkDataByindex(index).Isunlocked;
    }

}
