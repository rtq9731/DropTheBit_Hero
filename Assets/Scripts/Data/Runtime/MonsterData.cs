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
  public string Name { get {return name; } private set { this.name = value;} }
  
  [SerializeField]
  float hp;
  public float HP { get {return hp; } private set { this.hp = value;} }
  
  [SerializeField]
  int cost;
  public int Cost { get {return cost; } private set { this.cost = value;} }

    public void AddDamage(float damage)
    {
        this.hp -= damage;
    }

    public void InitData(float hp, int cost)
    {
        this.HP = hp;
        this.Cost = cost;
    }
}