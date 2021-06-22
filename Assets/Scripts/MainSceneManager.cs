using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;

    void Awake()
    {
        Instance = this;
    }
    void OnDestroy()
    {
        Instance = null;
    }

    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;
    [SerializeField] public TopUI topUI;
    [SerializeField] public WeaponUpUI upgradeUI;
    [SerializeField] public AudioSource hitSound;
    [SerializeField] public InputPanel InputPanel;
    [SerializeField] public LeftUI leftUI;

    public Player Player { get { return player; } set { player = value; } }

    public void CallBoss(int killCount)
    {
        leftUI.SetActiveTrueBtnBoss();
    }

    public void CallNextEnmey()
    {
        Debug.Log(GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]);
        GameObject temp = GameManager.Instance.EnemyPrefabs[GameManager.Instance.EnemyNames[GameManager.Instance.NowEnemyIndex]];
        Instantiate(temp, new Vector2(8, 1), Quaternion.identity);
    }

    public void SceneChange()
    {
        GameManager.Instance.ChangeSceneToBossScene();
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
    }

    public void DoHit()
    {
        hitSound.Play();
    }
}
