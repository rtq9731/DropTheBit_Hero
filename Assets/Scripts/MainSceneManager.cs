using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneManager : MonoBehaviour
{
    public static MainSceneManager Instance;
    [SerializeField] BackGroundMove backGround;
    [SerializeField] Player player;

    private Dictionary<string, GameObject> enemyPrefabs = new Dictionary<string, GameObject>();

    private void Awake()
    {
        Instance = this;    
    }
    void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        enemyPrefabs.Add("Skeleton", Resources.Load("Enemies/Skeleton") as GameObject);
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void CallNextEnmey()
    {
        GameObject temp = enemyPrefabs["Skeleton"];
        Instantiate(temp, new Vector2(8, 1), Quaternion.identity);
    }

    public void ScrollingBackground()
    {
        backGround.isScroll = !backGround.isScroll;
    }
}
