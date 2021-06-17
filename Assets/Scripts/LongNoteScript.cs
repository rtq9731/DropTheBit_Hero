using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [Header("Perfect가 뜰 거리")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good 판정에서 몇까지 Miss인가?")] [SerializeField] float whereIsMiss = 0f;

    [SerializeField] Transform myTailTr = null; // 꼬리의 Transform
    [SerializeField] LineRenderer lineRenderer = null; // LineRenderer

    public RhythmManager rhythmManager = null; // RhythmManager를 가지고 있도록 함
    float length = 0; // 롱노트의 길이 (ms)
    float startTiming = 0; // 노트가 시작한 시간 (ms)
    float myTimer = 0; // 롱노트 자체의 타이머 (ms)
    float tailMoveTime = 0f;
    float moveSpeed = 0f;

    int firstHit = 0; // 처음 판정

    bool isMoveTail = false; // 꼬리가 움직여야 하는지
    bool ishit = false; //판정에 맞으면 True로 변경
    bool isActFalse = false; // 만약 미스라면 비활성화 시켜야 하니까 True로 바꿈

    Vector2 checkLinePos = Vector2.zero;
    Vector2 startPos = Vector2.zero;

    /// <summary>
    /// 롱노트를 초기화 합니다. 
    /// myLength는 길이, startTimeing은 시작 시간을 의미합니다 ( ms 단위 )
    /// checkLinePos는 판정선의 위치를 의미합니다.
    /// startPos는 노트 생성위치를 의미합니다.
    /// movetime은 움직여야되는 시간을 의미합니다.
    /// </summary>
    /// <param name="myLength"></param>
    /// <param name="startTiming"></param>1
    public void InitLongNote(float myLength, float startTiming, Vector2 checkLinePos, Vector2 startPos, RhythmManager rhythmManager, float moveTime)
    {
        this.length = myLength;
        this.startTiming = startTiming;
        this.checkLinePos = checkLinePos;
        this.startPos = startPos;
        this.rhythmManager = rhythmManager;
        this.myTailTr.position = startPos;
        this.isMoveTail = false;
        this.tailMoveTime = moveTime;
        ishit = false;
        isActFalse = false;
        isMoveTail = false;
    }
    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        if(ishit == false) // 아직 판정이 나지 않았을때
        {
            transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0));

            Debug.Log(checkLinePos);
            Debug.Log(rhythmManager);

            if (Vector2.Distance(checkLinePos, this.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x <= rhythmManager.noteLine.transform.position.x)
            {
                rhythmManager.CrateEffect(3); // Miss 이펙트 출력


            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            switch (isHit(checkLinePos)) // Perfect = 1, Good = 2, Miss = 3, None = 4
            {
                case 1:
                    rhythmManager.CrateEffect(1);
                    ishit = true;
                    firstHit = 1;
                    break;
                case 2:
                    rhythmManager.CrateEffect(2);
                    ishit = true;
                    firstHit = 2;
                    break;
                case 3:
                    rhythmManager.CrateEffect(3);
                    ishit = true;
                    isActFalse = true;
                    break;
            }
        }

        if (!isActFalse && ishit)
        {
            if(Input.GetMouseButton(0)) // 누르고 있는지 검사할것
            {
                if (myTimer / 1000 == 0)
                {
                    rhythmManager.CrateEffect(firstHit);
                }
            }
            else //만약 누르다 떼지면
            {
                isActFalse = true;
            }
        }

        if (myTimer - 1000 > length) // 만약 노트 꼬리 생성 시간인 1초 전보다 길이가 줄어들면 ( 꼬리를 만들어야 하면 )
        {
            myTailTr.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0));
            isMoveTail = true;
        }
        else
        {
            myTailTr.position = startPos;
        }

        lineRenderer.SetPosition(1, myTailTr.position);
        lineRenderer.SetPosition(0, this.transform.position);

        myTimer += Time.deltaTime * 1000; // 자체적으로 계속해서 시간을 재줌 / ms 단위라서 X 1000
    }

    public int isHit(Vector2 linePos) // Perfect = 1, Good = 2, Miss = 3, None = 4
    {
        if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect)
        {
            return 1;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect + whereIsGood)
        {
            return 2;
        }
        else if (Vector2.Distance(linePos, this.transform.position) <= whereIsPerfect + whereIsGood + whereIsMiss)
        {
            return 3;
        }
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x >= rhythmManager.noteLine.transform.position.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
