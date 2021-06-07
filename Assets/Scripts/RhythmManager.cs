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
    [SerializeField] int poolingMax = 5;
    [SerializeField] float noteOffset = 2f;
    [SerializeField] float shakePower = 0f;
    [SerializeField] float shakeTime = 0f;

    [Header("테스트용 변수")]
    [SerializeField] string noteSongName = "";

    public List<GameObject> notesforPooling = new List<GameObject>();
    private int index = 0;

    private float noteTimer = 0;

    Queue<float> notesQueue = new Queue<float>();

    private bool isPlayingNote = false;

    private void Start()
    {
        InitRhythm();
    }

    private void Update()
    {
        if(isPlayingNote && noteTimer - noteOffset >= notesQueue.Peek())
        {
            Debug.Log(notesQueue.Peek());
            if(notesQueue.Count == 1)
            {
                CrateNote();
                notesQueue.Dequeue();
                FinishRhythm();
                return;
            }
            CrateNote();
            notesQueue.Dequeue();
        }
        
        if (isPlayingNote)
        {
            noteTimer += Time.deltaTime;
        }

        if(Input.GetMouseButtonDown(0))
        {
            CheckNote();
        }
    }

    public void StartStopSong()
    {
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

    void InitRhythm()
    {
        foreach (var item in GameManager.Instance.GetSongNotesByName(noteSongName))
        {
            notesQueue.Enqueue(item);
        }
        isPlayingNote = true;
        audioSource.Play();
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
        notesforPooling[index].gameObject.transform.DOScale(0, noteOffset).SetEase(Ease.Linear);
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
        for (int i = 0; i < notesforPooling.Count; i++)
        {
            var item = notesforPooling[i].GetComponent<NoteScript>();
            if (!item.gameObject.activeSelf) // 만약 비활성화 되있다면 멈춰!
            {
                continue;
            }

            switch (item.isHit()) // 히트 검사
            {
                // None
                case 0:
                    {
                        break;
                    }
                // Perfect 
                case 1:
                    {
                        noteLine.GetComponent<RectTransform>().DOShakeAnchorPos(shakeTime, shakePower).OnComplete(() => noteLine.GetComponent<RectTransform>().anchoredPosition = Vector3.zero);
                        DeleteNote(item.gameObject);
                        GameManager.Instance.AddCombo(2);
                        break;
                    }
                // Good
                case 2:
                    {
                        noteLine.GetComponent<RectTransform>().DOShakeAnchorPos(shakeTime, shakePower / 2f).OnComplete(() => noteLine.GetComponent<RectTransform>().anchoredPosition = Vector3.zero);
                        DeleteNote(item.gameObject);
                        GameManager.Instance.AddCombo(1);
                        break;
                    }
                // Miss
                case 3:
                    {
                        noteLine.GetComponent<RectTransform>().DOShakeAnchorPos(shakeTime, shakePower / 3f).OnComplete(() => noteLine.GetComponent<RectTransform>().anchoredPosition = Vector3.zero);
                        DeleteNote(item.gameObject);
                        GameManager.Instance.BreakCombo();
                        break;
                    }
                default: break;
            }
        }
    }

}
