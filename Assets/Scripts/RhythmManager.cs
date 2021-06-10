using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RhythmManager : MonoBehaviour
{
    [SerializeField] GameObject notePrefab;
    [SerializeField] GameObject noteLine;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] int poolingMax = 5;
    [SerializeField] float noteOffset = 2f;
    [SerializeField] float shakePower = 0f;
    [SerializeField] float shakeTime = 0f;

    public List<GameObject> notesforPooling = new List<GameObject>();
    private int index = 0;

    private float noteTimer = 0;

    private bool isPlayingNote = false;

    private int noteMakeIndex = 0;
    private int noteCheckIndex = 0;

    private void Start()
    {
        Invoke("StartStopSong", 2f);
    }

    private void Update()
    {
        if(isPlayingNote && noteTimer >= GameManager.Instance.parsingManager.BeatmapData["So_Happy"].HitObjects[noteMakeIndex].Time - 1000)
        {
            if (GameManager.Instance.parsingManager.BeatmapData["So_Happy"].HitObjects.Count < noteMakeIndex)
                return;

            noteMakeIndex++;
            CrateNote();
        }

        if (isPlayingNote)
        {
            noteTimer += Time.deltaTime * 1000;
        }

        if(Input.GetMouseButtonDown(0))
        {
            CheckNote();
        }
    }

    public void StartStopSong()
    {
        if (!isPlayingNote)
            audioSource.clip = GameManager.Instance.GetMusic();

        isPlayingNote = !isPlayingNote;

        if(isPlayingNote)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    void FinishRhythm()
    {
        isPlayingNote = false;
    }

    void CrateNote()
    {
        if (notesforPooling.Count < poolingMax)
        {
            notesforPooling.Add(Instantiate(notePrefab, transform));
        }
        else
        {
            notesforPooling[index].gameObject.SetActive(true);
        }
        notesforPooling[index].transform.position = noteMakeTr.position;
        notesforPooling[index].transform.DOMoveX(noteLine.transform.position.x, 1f).SetEase(Ease.Linear);
        index++;
        if (index == poolingMax)
        {
            index = 0; // index가 최대치라면 다시 초기화
        }

    }

    private void DeleteNote(GameObject note)
    {
        note.SetActive(false);
        DOTween.Complete(note);
    }

    private void CheckNote()
    {
        var item = notesforPooling[noteCheckIndex].GetComponent<NoteScript>();

        if (!item.gameObject.activeSelf) // 만약 비활성화 되있다면 다음으로 넘어가서 재시작!
        {
            noteCheckIndex++;

        if (noteCheckIndex == poolingMax)
            noteCheckIndex = 0;
            
            CheckNote();
        }

        int hit = item.isHit(noteLine.transform.position);
        if ((hit % 4) > 0)
        {
            Camera.main.transform.DOShakePosition(shakeTime, shakePower / (hit % 4));
            DeleteNote(item.gameObject);
        }
    }

}
