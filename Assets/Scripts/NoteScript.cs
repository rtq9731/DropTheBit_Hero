using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteScript : MonoBehaviour
{
    [Header("Perfect가 뜰 최소 사이즈")] [SerializeField] float minPerfectSize = 0f;
    [Header("Perfect가 뜰 최대 사이즈")] [SerializeField] float maxPerfectSize = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가")] [SerializeField] float whereIsGood = 0f;
    [SerializeField] [Header("초기 크기 배율")] float noteScale = 2;
     
    void OnEnable()
    {
        this.gameObject.transform.localScale = Vector3.one * noteScale;
    }

    public int isHit() // None = 0 , Perfect = 1, Good = 2, Miss = 3
    {
        if (this.transform.localScale.x > maxPerfectSize + whereIsGood) // 멀어서 판정이 안갈 때
        {
            return 0;
        }
        
        if (this.transform.localScale.x <= maxPerfectSize + whereIsGood && this.transform.localScale.x > maxPerfectSize) // 살짝 빨라서 Good
        {
            MainSceneManager.Instance.DoHit();
            return 2;
        }
       
        if (this.transform.localScale.x <= maxPerfectSize && this.transform.localScale.x >= minPerfectSize) // Perfect 판정
        {
            MainSceneManager.Instance.DoHit();
            return 1;
        }

        if (this.transform.localScale.x < minPerfectSize && this.transform.localScale.x >= minPerfectSize - whereIsGood) // 살짝 늦어서 Good
        {
            MainSceneManager.Instance.DoHit();
            return 2;
        }

        if (this.transform.localScale.x < minPerfectSize - whereIsGood) // 너무 늦어서 Miss
        {
            return 3;
        }

        return 3;
    }
}
