using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class OsuParser : MonoBehaviour
{

    // [Header("ParsingSong (DataPath without .osu)")] [SerializeField] string songName = "";
    private Beatmap beatmap;

    private Dictionary<string, Beatmap> beatmapData = new Dictionary<string, Beatmap>();
    public Dictionary<string, Beatmap> BeatmapData
    {
        get { return beatmapData; }
        private set { beatmapData = value; }
    }

    public void Parsing()
    {
        for (int i = 0; i < GameManager.Instance.noteSheet.dataArray.Length; i++)
        {
            beatmap = new Beatmap($"{Application.dataPath}/Resources/{GameManager.Instance.noteSheet.dataArray[i].Datapath}.osu");
            beatmapData.Add(GameManager.Instance.noteSheet.dataArray[i].Songname, beatmap);
        }
    }
}
