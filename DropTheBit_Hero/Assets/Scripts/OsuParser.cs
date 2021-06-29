using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class OsuParser : MonoBehaviour
{
    public void Parsing(string name)
    {
        Debug.Log(name);
        Debug.Log($"{Application.dataPath}/Resources/Notes/{name}.osu");
        GameManager.Instance.beatMap.beatmaps.Add(new Beatmap($"{Application.dataPath}/Resources/Notes/{name}.osu"));
    }
}
