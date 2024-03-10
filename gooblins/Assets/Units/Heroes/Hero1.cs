using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero1 : BaseHero
{
    public override void UnitAction(BaseEnemy enemy, BaseHero hero)
    {
        hero.Damage = 3;
        if (enemy.CurrentHealth > 0) enemy.CurrentHealth -= hero.Damage;
        if (enemy.CurrentHealth <= 0) Die(enemy);
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
