using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [SerializeField] LineRenderer line;
    [SerializeField] Transform tailTr;

    [Header("Perfect�� �� �Ÿ�")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect �������� ����� Good�ΰ�?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good �������� ����� Miss�ΰ�?")] [SerializeField] float whereIsMiss = 0f;

    private float length; // �� �ڽ��� ���� (ms)
    private float timer = 0f; // ��ü Ÿ�̸� ( �ճ�Ʈ�� ������ ����, ������ ����;���. )
    private float moveSpeed = 0;

    private Vector2 noteStartPos;
    private Vector2 noteLinePos;

    bool isMoving = true; // �������� �ϴ� Ÿ�̹�����.
    bool isMovingTail = false; // ���� ������ ���������ϴ� Ÿ�̹�����

    public void InitLongNote(Vector2 noteLinePos, float speed, float length, Vector2 noteStartPos)
    {
        this.length = length;
        this.noteLinePos = noteLinePos;
        this.noteStartPos = noteStartPos;
        moveSpeed = speed;
        isMoving = true;
    }


    private void Update()
    {

        if(isMoving)
        {
            transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // �Ӹ��� �������� �� �� �����̱�.
            tailTr.position = noteStartPos; // ���� ��ġ�� ����
        }

        if(isMovingTail)
        {
            tailTr.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // ������ �������� �� �� �����̱�.
        }

        if(transform.position.x <= noteLinePos.x && Vector2.Distance(transform.position, noteLinePos) <= 0.001f) // ��Ʈ ���� ��ġ�� �Ӹ��� �������� ��
        {
            isMoving = false; // �Ӹ��� �׸� �������� ���ڸ��� ���߰� ��.
        }
        if(tailTr.position.x <= noteLinePos.x && Vector2.Distance(tailTr.position, noteLinePos) <= 0.001f) // ���� ��Ʈ ���� ��ġ�� ������ �����ϸ�
        {
            Delete(); // �����.
        }

        timer += Time.deltaTime * 1000;
        if(timer >= length - 1000) // ���� ������ 1�����̶�� ( ������ ��Ʈ ������ġ�� �����ϴµ� 1�ʰ� �ɸ��� ���� ) ( ������ ms�Դϴ�. )
        {
            isMovingTail = true;
        }

    }

    void Delete()
    {
        
        length = 0;
        gameObject.SetActive(false);
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
        else if (Vector2.Distance(linePos, this.transform.position) >= whereIsPerfect + whereIsGood + whereIsMiss && transform.position.x >= linePos.x)
        {
            return 4;
        }
        else
        {
            return 3;
        }
    }
}
