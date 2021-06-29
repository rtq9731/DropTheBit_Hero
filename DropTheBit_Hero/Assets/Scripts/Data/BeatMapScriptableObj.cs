using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatMap Data", menuName = "Scriptable Object/BeatMap Data", order = int.MaxValue)]
public class BeatMapScriptableObj : ScriptableObject
{
    public List<Beatmap> beatmaps = new List<Beatmap>();

    [System.Serializable]
    public class VirtualBeatmaps
    {
        public VirtualBeatmaps(){}
        public VirtualBeatmaps(List<float> timings)
        {
            this.timings = timings;
        }


        public List<float> timings = new List<float>();
    }

    public List<VirtualBeatmaps> virtualBeatmaps = new List<VirtualBeatmaps>();

    public List<Beatmap> Beatmaps
    {
        get
        {
            return beatmaps;
        }
        set
        {
            beatmaps = value;
        }
    }

    public void SetVirtualList()
    {

        for (int i = 0; i < beatmaps.Count; i++)
        {
            List<float> list = new List<float>();
            for (int j = 0; j < beatmaps[i].HitObjects.Count; j++)
            {
                list.Add(beatmaps[i].HitObjects[j].Time);
            }

            virtualBeatmaps.Add(new VirtualBeatmaps(list));
        }
    }
}
