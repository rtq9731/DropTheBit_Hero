using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class OsuParser : MonoBehaviour
{

    [Header("ParsingSong (DataPath without .osu)")] [SerializeField] string songName = "";
    private Beatmap beatmap;

    public void SetSongName(string name)
    {
        songName = name;
    }

    public void Parsing()
    {
        beatmap = new Beatmap($"{Application.dataPath}/Resources/{songName}.osu");
    }

    public AudioClip GetMusic()
    {
        AudioClip temp = null;
        return temp = Resources.Load($"{Application.dataPath}/Resources/SongMP3/{songName}") as AudioClip;
    }
}
