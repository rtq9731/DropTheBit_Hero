using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class MonsterData
{
  [SerializeField]
  short index;
  public short Index { get {return index; } set { this.index = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  float hp;
  public float HP { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  long cost;
  public long Cost { get {return cost; } set { this.cost = value;} }
  
  [SerializeField]
  string animatorcontrollerpath;
  public string Animatorcontrollerpath { get {return animatorcontrollerpath; } set { this.animatorcontrollerpath = value;} }
  
}