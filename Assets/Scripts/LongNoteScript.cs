using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [SerializeField] LineRenderer line;

    [Header("Perfect�� �� �Ÿ�")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect �������� ����� Good�ΰ�?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good �������� ����� Miss�ΰ�?")] [SerializeField] float whereIsMiss = 0f;

    private float length; // �� �ڽ��� ���� (ms)
    private float moveSpeed = 0;

    private Vector2 noteLinePos;

    bool isMoving = true; // �������� �ϴ� Ÿ�̹�����.

    public void InitLongNote(Vector2 noteLinePos, float speed)
    {
        this.noteLinePos = noteLinePos;
        moveSpeed = speed;
        isMoving = true;
    }


    private void Update()
    {
        if(isMoving)
        {
            transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // �������� �� �� �����̱�.
        }

        if(transform.position.x <= noteLinePos.x && Vector2.Distance(transform.position, noteLinePos) >= whereIsPerfect + whereIsGood + whereIsMiss) // �ʹ� �ڷ� ������.
        {
            Delete();
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
