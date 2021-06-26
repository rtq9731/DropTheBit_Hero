using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanelPrefab;
    [SerializeField] Transform content;

    List<UpgradePanel> panels = new List<UpgradePanel>();

    private void Start()
    {
        Invoke("MakeUpgradePanels", 0.01f);
    }

    void MakeUpgradePanels()
    {
        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            GameObject temp = Instantiate(upgradePanelPrefab, content);
            panels.Add(temp.GetComponent<UpgradePanel>());
            panels[i].InitUpgradePanel(i);
        }

        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            panels[i].Refresh(GameManager.Instance.GetWeaponByIndex(i).Isunlocked);
        }
    }

    public short GetCurrentWeaponIndex()
    {
        short temp = 0;
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i].GetIsUnlocked())
                temp++;
        }
        return temp;
    }

    public UpgradePanel GetUpgradePanelByIndex(int index)
    {
        return panels[index];
    }

}
