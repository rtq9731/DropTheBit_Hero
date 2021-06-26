using UnityEngine;
using System.Collections;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class WeaponData
{
  [SerializeField]
  short index;
  public short Index { get {return index; } set { this.index = value;} }
  
  [SerializeField]
  string image_path;
  public string Image_Path { get {return image_path; } set { this.image_path = value;} }
  
  [SerializeField]
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  short upgradecount;
  public short Upgradecount { get {return upgradecount; } set { this.upgradecount = value;} }
  
  [SerializeField]
  long upgradecost;
  public long Upgradecost { get {return upgradecost; } set { this.upgradecost = value;} }
  
  [SerializeField]
  bool isunlocked;
  public bool Isunlocked { get {return isunlocked; } set { this.isunlocked = value;} }
  
}