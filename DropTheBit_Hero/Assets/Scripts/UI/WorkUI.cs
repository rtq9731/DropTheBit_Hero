using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkUI : MonoBehaviour
{
    [SerializeField] GameObject workPanelPrefab;
    [SerializeField] Transform content;

    List<WorkPanel> panels = new List<WorkPanel>();
    List<WorkYieldTimer> timers = new List<WorkYieldTimer>();

    class WorkYieldTimer
    {
        public WorkYieldTimer(float cool, float timer, long yield)
        {
            this.cool = cool;
            this.timer = timer;
            this.yield = yield;
        }

        float cool = 0f;
        float timer = 0f;
        long yield = 0;

        public float Timer
        {
            get { return timer; }
            set
            {

                timer = value;
                if (timer >= cool)
                {
                    timer = 0;
                    GameManager.Instance.AddMoney(yield);
                }

            }
        }

        public void UpdateData(long yield)
        {
            this.yield = yield;
        }
    }

    private void Start()
    {
        //Invoke("MakeWorkPanels", 0.01f);
    }

    private void Update()
    {
        foreach (var item in timers)
        {
            item.Timer += Time.deltaTime;
        }
    }

    void MakeWorkPanels()
    {
        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            GameObject temp = Instantiate(workPanelPrefab, content);
            panels.Add(temp.GetComponent<WorkPanel>());
            panels[i].InitWorkPanel(i);
        }

        for (int i = 0; i < GameManager.Instance.Weapons.Count; i++)
        {
            panels[i].Refresh(GameManager.Instance.GetWeaponByIndex(i).Isunlocked);
        }
    }

    public short GetCurrentWorkIndex()
    {
        short temp = 0;
        for (int i = 0; i < panels.Count; i++)
        {
            if (panels[i].GetIsUnlocked())
                temp++;
        }
        return temp;
    }

    public WorkPanel GetWorkPanelByIndex(int index)
    {
        return panels[index];
    }

}
