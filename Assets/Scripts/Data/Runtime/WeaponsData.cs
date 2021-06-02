using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class WeaponsData
{
  [SerializeField]
  short index;
  public short Index { get {return index; } set { this.index = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  short upgradecount;
  public short Upgradecount { get {return upgradecount; } set { this.upgradecount = value;} }
  
  [SerializeField]
  int upgradecost;
  public int Upgradecost { get {return upgradecost; } set { this.upgradecost = value;} }
  
  [SerializeField]
  bool isunlocked;
  public bool Isunlocked { get {return isunlocked; } set { this.isunlocked = value;} }
  
}