using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class OsuParser : MonoBehaviour
{
    private Dictionary<string, Beatmap> beatmapData = new Dictionary<string, Beatmap>();
    public Dictionary<string, Beatmap> BeatmapData
    {
        get { return beatmapData; }
        private set { beatmapData = value; }
    }

    public void Parsing(string name)
    {
        if(beatmapData.TryGetValue(name, out Beatmap beatMap))
        {
            return;
        }

        beatMap = new Beatmap($"{Application.dataPath}/Resources/Notes/{name}.osu");
        beatmapData.Add(name, beatMap);
    }
}
