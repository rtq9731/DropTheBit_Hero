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
    /// �޼������� ���ΰ�ħ ���ݴϴ�.
    /// </summary>
    public void Refresh(bool isUnlocked) // �����Լ�
    {

        if (data.Isunlocked && timer != null)
        {
            return;
        }

        lockPanel.SetActive(!isUnlocked); // isUnlocked�� �ݴ�� ����������.
        data.Isunlocked = isUnlocked;

        if (data.Isunlocked && timer == null)
        {
            timer = new WorkUI.WorkYieldTimer(data.Moneycool, data.Yield, ProgressBar);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.Round(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {data.Upgradecount}"; // ���׷��̵� �ܰ� ǥ��

        upgradeBtn.onClick.AddListener(() => Upgrade());
    }

    public void Refresh() // �����ε� �Լ�
    {
        if(data.Upgradecount >= data.Leastup) // �ּ� ���׷��̵� �̻� ���۵Ǹ�
        {
            MainSceneManager.Instance.workUI.GetWorkPanelByIndex(data.Index).Refresh(true);
        }

        upgradeBtn.onClick.RemoveAllListeners();

        this.upgradeCostText.text = $"���׷��̵� ��� : {Mathf.Round(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1}��"; // ù ���׷��̵尡 ���� ��� ���� * ���׷��̵� �ܰ�� ǥ��
        this.currentUpgradeText.text = $"���� ���׷��̵� �ܰ� : {data.Upgradecount}"; // ���׷��̵� �ܰ� ǥ��

        upgradeBtn.onClick.AddListener(() => Upgrade()); // �ִ�� ���׷��̵� ���� �ʾҴٸ� ���׷��̵� ����
    }

    private void Upgrade()
    {
        if (GameManager.Instance.GetMoney() < Mathf.RoundToInt(data.Upgradecost + (0.3f * data.Upgradecount - 1)) + 1) // ���� ������ ���
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
