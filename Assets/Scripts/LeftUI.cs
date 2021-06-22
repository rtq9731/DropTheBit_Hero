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
    /// 보스로 향하는 버튼을 활성화 시켜주는 메소드
    /// </summary>
    public void SetActiveTrueBtnBoss()
    {

        noticePanel.SetActive(true);

        btnBoss.interactable = true;

        btnBoss.onClick.AddListener(() => 
        {
            MainSceneManager.Instance.InputPanel.InitInputPanel("보스에게 도전하시겠습니까?");
            MainSceneManager.Instance.InputPanel.GetBtnOk().onClick.AddListener(() => GameManager.Instance.ChangeSceneToBossScene());
            MainSceneManager.Instance.InputPanel.gameObject.SetActive(true);
        });
    }
}
