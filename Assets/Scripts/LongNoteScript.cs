using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LongNoteScript : MonoBehaviour
{
    [SerializeField] LineRenderer line;

    [Header("Perfect가 뜰 거리")] [SerializeField] float whereIsPerfect = 0f;
    [Header("Perfect 판정에서 몇까지 Good인가?")] [SerializeField] float whereIsGood = 0f;
    [Header("Good 판정에서 몇까지 Miss인가?")] [SerializeField] float whereIsMiss = 0f;

    private float length; // 내 자신의 길이 (ms)
    private float moveSpeed = 0;

    private Vector2 noteLinePos;

    bool isMoving = true; // 움직여야 하는 타이밍인지.

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
            transform.Translate(new Vector3(-Time.deltaTime * moveSpeed, 0, 0)); // 움직여야 할 때 움직이기.
        }

        if(transform.position.x <= noteLinePos.x && Vector2.Distance(transform.position, noteLinePos) >= whereIsPerfect + whereIsGood + whereIsMiss) // 너무 뒤로 갔을때.
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
