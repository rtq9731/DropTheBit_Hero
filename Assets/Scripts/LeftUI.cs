using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LeftUI : MonoBehaviour
{
    [SerializeField] Button btnBoss;
    [SerializeField] GameObject noticePanel;

    /// <summary>
    /// ������ ���ϴ� ��ư�� Ȱ��ȭ �����ִ� �޼ҵ�
    /// </summary>
    public void SetActiveTrueBtnBoss()
    {

        noticePanel.SetActive(true);

        btnBoss.interactable = true;

        btnBoss.onClick.AddListener(() => 
        {
            MainSceneManager.Instance.InputPanel.InitInputPanel("�������� �����Ͻðڽ��ϱ�?");
            MainSceneManager.Instance.InputPanel.GetBtnOk().onClick.AddListener(() => GameManager.Instance.ChangeSceneToBossScene());
            MainSceneManager.Instance.InputPanel.gameObject.SetActive(true);
        });
    }
}
