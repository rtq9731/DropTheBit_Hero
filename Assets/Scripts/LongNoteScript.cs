using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Transform headTr;
    [SerializeField] Transform tailTr;

    [Header("Perfect가 뜰 거리")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good 판정에서 몇까지 Miss인가?")] [SerializeField] float whereIsMiss = 0f;

    private float length; // 내 자신의 길이 (ms)
    private float timer = 0f; // 자체 타이머 ( 롱노트가 끝나기 전에, 꼬리를 끌고와야함. )
    private float moveSpeed = 0;

    private Vector2 noteStartPos;
    private Vector2 noteLinePos;

    bool isMovingHead = true; // 머리가 움직여야 하는 타이밍인지.
    bool isMovingTail = false; // 만약 꼬리가 움직여야하는 타이밍인지

    public void InitLongNote(Vector2 noteLinePos, float speed, float length, Vector2 noteStartPos)
    {
        this.length = length;
        this.noteLinePos = noteLinePos;
        this.noteStartPos = noteStartPos;
        moveSpeed = speed;
        isMovingHead = true;
    }


    private void Update()
    {

        if(isMovingHead)
        {
            headTr.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // 머리가 움직여야 할 때 움직이기.
            tailTr.position = noteStartPos; // 시작 위치에 고정
        }

        if(isMovingTail)
        {
            tailTr.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // 꼬리가 움직여야 할 때 움직이기.
        }

        if(Vector2.Distance(headTr.position, noteLinePos) <= 0.01f) // 노트 판정 위치에 머리가 도착했을 때
        {
            isMovingHead = false; // 머리가 그만 움직여서 그자리에 엄추게 함.
        }
        if(tailTr.position.x <= noteLinePos.x && Vector2.Distance(tailTr.position, noteLinePos) >= whereIsPerfect + whereIsGood + whereIsMiss) // 만약 노트 판정 위치를 지나 에 꼬리가 도착하면
        {
            Debug.Log("꼬리가 도착했답니다!");
            Delete(); // 지우기. (테스트 용이고, 판정 계산할 것임.
        }

        timer += Time.deltaTime * 1000;
        if(timer >= length - 1000) // 만약 끝나기 1초전이라면 ( 꼬리가 노트 판정위치에 도달하는데 1초가 걸리기 때문 ) ( 단위는 ms입니다. )
        {
            isMovingTail = true;
        }

        line.SetPosition(0, headTr.position);
        line.SetPosition(1, tailTr.position);

    }

    void Delete()
    {
        
        length = 0;
        gameObject.SetActive(false);
    }


    public int isHit(Vector2 linePos, Transform tr) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
        if (Vector2.Distance(linePos, tr.position) <= whereIsPerfect)
        {
            return 1;
        }
        else if (Vector2.Distance(linePos, tr.position) <= whereIsPerfect + whereIsGood)
        {
            return 2;
        }
        else if (Vector2.Distance(linePos, tr.position) <= whereIsPerfect + whereIsGood + whereIsMiss)
        {
            return 3;
        }
        else if (Vector2.Distance(linePos, tr.position) >= whereIsPerfect + whereIsGood + whereIsMiss && tr.position.x >= linePos.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
