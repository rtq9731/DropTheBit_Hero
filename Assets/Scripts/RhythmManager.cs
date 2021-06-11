using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RhythmManager : MonoBehaviour
{
    class HitEffectColors
    {
        public Color perfectColor = new Color(1, 0.8874891f, 0, 1);
        public Color goodColor = new Color(0.6792453f, 0.9150943f, 0.06030021f, 1);
        public Color missColor = new Color(0.6792453f, 0, 0.06030021f, 1);
    }


    [SerializeField] GameObject notePrefab;
    [SerializeField] public GameObject noteLine;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] AudioSource audioSource;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] Transform hitEffectTransform;
    [SerializeField] int poolingMax = 5;
    [SerializeField] float noteOffset = 2f;
    [SerializeField] float shakePower = 0f;
    [SerializeField] float shakeTime = 0f;
    [SerializeField] float noteEndXOffset = 0f;

    private float noteLineDistance = 0f;

    private HitEffectColors hitEffectColors = new HitEffectColors();

    public List<GameObject> notesforPooling = new List<GameObject>();
    private List<GameObject> effects = new List<GameObject>();

    private int indexforEffectPooling = 0;
    private int indexforNotePooling = 0;
    private int noteMakeIndex = 0;
    private int noteCheckIndex = 0;

    private float noteTimer = 0;

    private bool isPlayingNote = false;



    private void Start()
    {
        noteLineDistance = Vector2.Distance(noteMakeTr.position, noteLine.transform.position);
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
            notesforPooling[indexforNotePooling].gameObject.SetActive(true);
        }

        notesforPooling[indexforNotePooling].transform.position = noteMakeTr.position;
        notesforPooling[indexforNotePooling].GetComponent<NoteScript>().SetRhythmManager(this);
        notesforPooling[indexforNotePooling].transform.DOMoveX(noteLine.transform.position.x - noteEndXOffset, -(noteLineDistance / (noteLine.transform.position.x - noteEndXOffset))).SetEase(Ease.Linear);

        indexforNotePooling++;
        if (indexforNotePooling == poolingMax)
        {
            indexforNotePooling = 0; // indexforNotePooling가 최대치라면 다시 초기화
        }

    }
    public void CrateEffect(string text)
    {
        if (effects.Count < poolingMax)
        {
            effects.Add(Instantiate(hitEffectPrefab, hitEffectTransform));
        }
        else
        {
            effects[indexforEffectPooling].gameObject.SetActive(true);
        }

        NoteHitEffect nowEffect = effects[indexforEffectPooling].GetComponent<NoteHitEffect>();
        switch (text)
        {
            case "PERFECT":
                {
                    nowEffect.text.color = hitEffectColors.perfectColor;
                    nowEffect.text.text = "PERFECT";
                    break;
                }
            case "GOOD":
                {
                    nowEffect.text.color = hitEffectColors.goodColor;
                    nowEffect.text.text = "GOOD";
                    break;
                }
            case "MISS":
                {
                    nowEffect.text.color = hitEffectColors.missColor;
                    nowEffect.text.text = "MISS";
                    break;
                }
        }

        nowEffect.transform.position = hitEffectTransform.position;

        Sequence seq = DOTween.Sequence();
        seq.OnStart(() =>{
            nowEffect.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
            });
        seq.Append(nowEffect.GetComponent<RectTransform>().DOAnchorPosY(50, 1).SetEase(Ease.InBack));
        seq.Join(nowEffect.text.DOFade(0, 1));
        seq.OnComplete(() => nowEffect.gameObject.SetActive(false));

        indexforEffectPooling++;
        if (indexforEffectPooling == poolingMax)
        {
            indexforEffectPooling = 0; // indexforNotePooling가 최대치라면 다시 초기화
        }

    }

    private void DeleteNote(GameObject note)
    {
        note.SetActive(false);
        DOTween.Complete(note);
    }

    public void CheckNote()
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
            //Camera.main.transform.DOShakePosition(shakeTime, shakePower / (hit / 4));
            DeleteNote(item.gameObject);
            noteCheckIndex++; // 다음 노트를 검사하게 Index++;
            if (noteCheckIndex == poolingMax)
                noteCheckIndex = 0;
        }
    }

}
