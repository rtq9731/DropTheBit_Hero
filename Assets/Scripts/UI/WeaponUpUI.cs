using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpUI : MonoBehaviour
{
    [SerializeField] GameObject upgradePanelPrefab;
    [SerializeField] GameObject lockObject;

    List<UpgradePanel> panels = new List<UpgradePanel>();

    private void Start()
    {
        MakeUpgradePanels();
    }

    void MakeUpgradePanels()
    {
        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            GameObject temp = Instantiate(upgradePanelPrefab, transform);
            panels.Add(temp.GetComponent<UpgradePanel>());
        }

        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            panels[i].InitUpgradePanel(i);
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
