using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [Header("Perfect�� �� �Ÿ�")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect �������� ����� Good�ΰ�?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good �������� ����� Miss�ΰ�?")] [SerializeField] float whereIsMiss = 0f;

    [SerializeField] Transform myTailTr = null; // ������ Transform
    [SerializeField] LineRenderer lineRenderer = null; // LineRenderer

    public RhythmManager rhythmManager = null; // RhythmManager�� ������ �ֵ��� ��
    float length = 0; // �ճ�Ʈ�� ���� (ms)
    float startTiming = 0; // ��Ʈ�� ������ �ð� (ms)
    float myTimer = 0; // �ճ�Ʈ ��ü�� Ÿ�̸� (ms)
    float tailMoveTime = 0f;
    float moveSpeed = 0f;

    int firstHit = 0; // ó�� ����

    bool isMoveTail = false; // ������ �������� �ϴ���
    bool ishit = false; //������ ������ True�� ����
    bool isActFalse = false; // ���� �̽���� ��Ȱ��ȭ ���Ѿ� �ϴϱ� True�� �ٲ�

    Vector2 checkLinePos = Vector2.zero;
    Vector2 startPos = Vector2.zero;

    /// <summary>
    /// �ճ�Ʈ�� �ʱ�ȭ �մϴ�. 
    /// myLength�� ����, startTimeing�� ���� �ð��� �ǹ��մϴ� ( ms ���� )
    /// checkLinePos�� �������� ��ġ�� �ǹ��մϴ�.
    /// startPos�� ��Ʈ ������ġ�� �ǹ��մϴ�.
    /// movetime�� �������ߵǴ� �ð��� �ǹ��մϴ�.
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
        if(ishit == false) // ���� ������ ���� �ʾ�����
        {
            transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0));

            Debug.Log(checkLinePos);
            Debug.Log(rhythmManager);

            if (Vector2.Distance(checkLinePos, this.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x <= rhythmManager.noteLine.transform.position.x)
            {
                rhythmManager.CrateEffect(3); // Miss ����Ʈ ���


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
            if(Input.GetMouseButton(0)) // ������ �ִ��� �˻��Ұ�
            {
                if (myTimer / 1000 == 0)
                {
                    rhythmManager.CrateEffect(firstHit);
                }
            }
            else //���� ������ ������
            {
                isActFalse = true;
            }
        }

        if (myTimer - 1000 > length) // ���� ��Ʈ ���� ���� �ð��� 1�� ������ ���̰� �پ��� ( ������ ������ �ϸ� )
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

        myTimer += Time.deltaTime * 1000; // ��ü������ ����ؼ� �ð��� ���� / ms ������ X 1000
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
