using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RhythmManager : MonoBehaviour
{
    class HitEffectColors
    {
        public Color perfectColor = new Color(1, 0.8874891f, 0, 1);
        public Color goodColor = new Color(0.6792453f, 0.9150943f, 0.06030021f, 1);
        public Color missColor = new Color(0.6792453f, 0, 0.06030021f, 1);
    }

    [Header("오디오 관련")]
    [SerializeField] AudioSource audioSource;

    [Header("노트 관련")]
    [SerializeField] GameObject notePrefab;
    [SerializeField] public GameObject noteLine;
    [SerializeField] Transform noteMakeTr;
    [SerializeField] string parsingSongName = ""; // for test
    [SerializeField] int poolingMax = 5;
    [SerializeField] float noteEndXOffset = 0f;

    private List<GameObject> notesforPooling = new List<GameObject>();

    private int indexforNotePooling = 0;
    private int noteMakeIndex = 0;
    private int noteCheckIndex = 0;

    private float noteTimer = 0;
    private float noteLineDistance = 0f;


    [Header("콤보 이펙트 관련")]
    [SerializeField] Transform hitEffectTransform;
    [SerializeField] GameObject hitEffectPrefab;

    private List<GameObject> effects = new List<GameObject>();
    private HitEffectColors hitEffectColors = new HitEffectColors();
    private int indexforEffectPooling = 0;

    private bool isPlayingNote = false;
    private bool isBossScene = false;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BossScene")
        {
            BossSceneManager.Instance.progressBar.maxValue = GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects.Count;
            isBossScene = true;
            Animator.StringToHash("Attack1");
        }

        noteLineDistance = Vector2.Distance(noteMakeTr.position, noteLine.transform.position);
        Invoke("StartStopSong", 2f);
    }

    private void Update()
    {
        if(isBossScene)
        {

        }


        if (GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects.Count > noteMakeIndex)
        {
            if (isPlayingNote && noteTimer >= GameManager.Instance.parsingManager.BeatmapData[parsingSongName].HitObjects[noteMakeIndex].Time - 1000)
            {
                noteMakeIndex++;
                CrateNote();
            }
        }
        else
        {
            FinishRhythm();
            return;
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
        if(isBossScene)
        {
            BossSceneManager.Instance.FinishFight();
        }
    }

    void CrateNote()
    {
        if (isBossScene)
        {
            int randNum = Random.Range(1, 3);
            switch (randNum)
            {
                case 1:
                    BossSceneManager.Instance.bossAnimator.SetTrigger(BossSceneManager.Instance.bossAttack1Hash);
                    break;
                case 2:
                    BossSceneManager.Instance.bossAnimator.SetTrigger(BossSceneManager.Instance.bossAttack2Hash);
                    break;
                default:
                    break;
            }
        }

        if (notesforPooling.Count < poolingMax)
        {
            notesforPooling.Add(Instantiate(notePrefab, transform));
        }
        else
        {
            notesforPooling[indexforNotePooling].gameObject.SetActive(true);
        }


        Transform noteTr = notesforPooling[indexforNotePooling].transform;
        noteTr.position = noteMakeTr.position;
        noteTr.GetComponent<NoteScript>().SetRhythmManager(this);
        noteTr.DOMoveX(
            noteLine.transform.position.x - noteEndXOffset,
            -(noteLineDistance / (noteLine.transform.position.x - noteEndXOffset))
            ).SetEase(Ease.Linear);

        indexforNotePooling++;
        if (indexforNotePooling == poolingMax)
        {
            indexforNotePooling = 0; // indexforNotePooling가 최대치라면 다시 초기화
        }

    }

    public void CrateEffect(int n) // Perfect = 1, Good = 2, Miss = 3
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
        switch (n)
        {
            case 1:
                {
                    nowEffect.text.color = hitEffectColors.perfectColor;
                    GameManager.Instance.Combo += 2;
                    nowEffect.text.text = $"PERFECT X {GameManager.Instance.Combo}";

                    if (isBossScene)
                    {
                        BossSceneManager.Instance.progressBar.value += 2;
                        BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);
                    }

                    break;
                }
            case 2:
                {
                    nowEffect.text.color = hitEffectColors.goodColor;
                    ++GameManager.Instance.Combo;
                    nowEffect.text.text = $"GOOD X {GameManager.Instance.Combo}";

                    if (isBossScene)
                    {
                        BossSceneManager.Instance.progressBar.value++;
                        BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerAttackHash);
                    }

                    break;
                }
            case 3:
                {
                    nowEffect.text.color = hitEffectColors.missColor;
                    GameManager.Instance.Combo = 0;
                    nowEffect.text.text = "MISS";

                    if (isBossScene)
                    {
                        BossSceneManager.Instance.progressBar.value--;
                        BossSceneManager.Instance.playerAnimator.SetTrigger(BossSceneManager.Instance.playerHitHash);
                    }

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
        
        if ((hit % 4) > 0) // None이 아닌 경우
        {
            //Camera.main.transform.DOShakePosition(shakeTime, shakePower / (hit / 4));
            CrateEffect(hit);
            DeleteNote(item.gameObject);
            noteCheckIndex++; // 다음 노트를 검사하게 Index++;
            if (noteCheckIndex == poolingMax)
                noteCheckIndex = 0;
        }
    }

}
