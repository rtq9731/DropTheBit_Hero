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
    /// �޼������� ���ΰ�ħ ���ݴϴ�.
    /// </summary>
    public void Refresh(bool isUnlocked) // �����Լ�
    {

        if (GameManager.Instance.GetWorkDataByindex(index).Isunlocked && timer != null)
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked�� �ݴ�� ����������.
        GameManager.Instance.GetWorkDataByindex(index).Isunlocked = isUnlocked;

        if (GameManager.Instance.GetWorkDataByindex(index).Isunlocked && timer == null)
        {
            timer = new WorkUI.WorkYieldTimer(GameManager.Instance.GetWorkDataByindex(index).Moneycool, GameManager.Instance.GetWorkDataByindex(index).Yield, ProgressBar);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.Round(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {GameManager.Instance.GetWorkDataByindex(index).Upgradecount}"; // ���׷��̵� �ܰ� ǥ��

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    public void Refresh() // �����ε� �Լ�
    {
        if(GameManager.Instance.GetWorkDataByindex(index).Upgradecount >= GameManager.Instance.GetWorkDataByindex(index).Leastup) // �ּ� ���׷��̵� �̻� ���۵Ǹ�
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(GameManager.Instance.GetWorkDataByindex(index).Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.Round(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {GameManager.Instance.GetWorkDataByindex(index).Upgradecount}"; // ���׷��̵� �ܰ� ǥ��

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < Mathf.RoundToInt(GameManager.Instance.GetWorkDataByindex(index).Upgradecost + (0.3f * GameManager.Instance.GetWorkDataByindex(index).Upgradecount - 1)) + 1) // ���� ������ ���
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
