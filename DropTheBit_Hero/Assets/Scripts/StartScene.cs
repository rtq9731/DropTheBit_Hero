using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    [SerializeField] Text title1;
    [SerializeField] Text title2;
    [SerializeField] Button btnStart;

    private void Start()
    {
        GameManager.Instance.CallGameManager();
        Sequence seq = DOTween.Sequence();

        seq.Append(title1.GetComponent<RectTransform>().DOAnchorPosY(0, 2f).SetEase(Ease.OutElastic));
        seq.Append(title2.GetComponent<RectTransform>().DOAnchorPosY(-300, 2f).SetEase(Ease.OutElastic));
        seq.Join(btnStart.GetComponent<RectTransform>().DOAnchorPosY(250, 2f).SetEase(Ease.InOutBack).OnComplete(() => btnStart.onClick.AddListener(() => SceneManager.LoadScene("MainScene"))));
    }
}
