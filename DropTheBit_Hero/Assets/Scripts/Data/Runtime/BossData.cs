using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class BossData
{
  [SerializeField]
  short index;
  public short Index { get {return index; } set { this.index = value;} }
  
  [SerializeField]
  string bossname;
  public string Bossname { get {return bossname; } set { this.bossname = value;} }
  
  [SerializeField]
  string songname;
  public string Songname { get {return songname; } set { this.songname = value;} }
  
  [SerializeField]
  string datapath;
  public string Datapath { get {return datapath; } set { this.datapath = value;} }
  
  [SerializeField]
  bool iscleared;
  public bool Iscleared { get {return iscleared; } set { this.iscleared = value;} }
  
}