using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : BaseEnemy
{
    public int partrol_length;
    private Vector2 direction = new Vector2(1,0);
    public override void Patrol()
    {
        //find tile one right of the time we're currently at
        Debug.Log($"direction= {direction}");
        Vector2 adjacent_tile_v2 = new Vector2(
            Mathf.Clamp(this.OccupiedTile._position.x + direction.x* Movement,1,GridManager._width-1),
            Mathf.Clamp(this.OccupiedTile._position.y + direction.y*Movement, 1,GridManager._height-1));
        Debug.Log($"new taget tile{ adjacent_tile_v2}");
        Tile adjacent_tile = GridManager._tiles[adjacent_tile_v2];
        Debug.Log(adjacent_tile);

    Debug.Log(adjacent_tile);
        if (-1 == UnitManager.Instance.MoveUnit(this, adjacent_tile)) //flip if run into a wall (next time go the other way)
        {
            direction = new Vector2(direction.y,-1 * direction.x);

        }

    }

    public override void UnitAction(BaseEnemy enemy, BaseHero hero)
    {
        if (hero.CurrentHealth > 0)
        {
            AudioManager.Instance.Play("DogBark");
            hero.CurrentHealth -= Damage;
            AudioManager.Instance.Play("GooblinDamage");
        }
        if (hero.CurrentHealth <= 0)
        {
            Die(hero);
            AudioManager.Instance.Play("GooblinDie");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
