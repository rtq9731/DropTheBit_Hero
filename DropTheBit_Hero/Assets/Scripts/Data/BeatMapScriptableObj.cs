using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "BeatMap Data", menuName = "Scriptable Object/BeatMap Data", order = int.MaxValue)]
public class BeatMapScriptableObj : ScriptableObject
{
    public List<Beatmap> beatmaps = new List<Beatmap>();

    public List<Beatmap> Beatmaps
    {
        get
        {
            for (int i = 0; i < beatmaps.Count; i++)
            {
                for (int j = 0; j < beatmaps.Count; j++)
                {
                    if (beatmaps[i] == beatmaps[j] && i != j)
                    {
                        RemoveSame(beatmaps[j]);
                    }
                }
            }

            return beatmaps;
        }
    }

    private void RemoveSame(Beatmap item)
    {
        beatmaps.Remove(item);
    }
}
