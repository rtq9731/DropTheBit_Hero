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
  string name;
  public string Name { get {return name; } set { this.name = value;} }
  
  [SerializeField]
  float hp;
  public float HP { get {return hp; } set { this.hp = value;} }
  
  [SerializeField]
  int cost;
  public int Cost { get {return cost; } set { this.cost = value;} }
    public void AddDamage(float damage)
    {
        this.hp -= damage;
    }
    public void InitData(float hp, int cost, string name)
    {
        this.HP = hp;
        this.Cost = cost;
        this.Name = name;
    }

}