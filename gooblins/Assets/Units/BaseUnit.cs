using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {
    public string UnitName;
    public Tile OccupiedTile;
    public Faction Faction;
    public int MaxMovement;
    public int Movement;
    public int MaxHealth;
    public int CurrentHealth;
    public int Damage;

    //declare action

    public virtual void UnitAction(BaseEnemy enemy, BaseHero hero)
    {
        //set damage that hero does
        //update enemy health
        //destroy if health is zero
    }

    public static void Die(BaseUnit enemy)
    {
        //called in UnitAction when enemy health == 0
        if (enemy.Faction == Faction.Hero)
        {
            UnitManager.Instance.heros.Remove((BaseHero)enemy);
        }
        if (enemy.Faction == Faction.Enemy)
        {
            UnitManager.Instance.enemies.Remove((BaseEnemy)enemy);
        }

        Destroy(enemy.gameObject);
    }
}
