using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : BaseEnemy
{
    public int partrol_length;
    private int direction = 1;
    public override void Patrol()
    {
        //find tile one right of the time we're currently at
        Vector2 adjacent_tile_v2 = new Vector2(this.OccupiedTile._position.x * direction, this.OccupiedTile._position.y);
        Tile adjacent_tile = GridManager._tiles[adjacent_tile_v2];


        Debug.Log(adjacent_tile);
        if (-1 == UnitManager.Instance.MoveUnit(this, adjacent_tile)) //flip if run into a wall (next time go the other way)
        {
            direction *= -1;
        }

    }

    public override void UnitAction(BaseEnemy enemy, BaseHero hero)
    {
        if (hero.CurrentHealth > 0)
        {
            AudioManager.Instance.Play("KnightSound");
            hero.CurrentHealth -= Damage;
            AudioManager.Instance.Play("GooblinDamage");
        }
        if (hero.CurrentHealth <= 0)
        {
            Die(hero);
            AudioManager.Instance.Play("GooblinDie");
        }
    }
}
