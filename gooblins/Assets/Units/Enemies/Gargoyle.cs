using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gargoyle : BaseEnemy
{
    public int partrol_length;


    public override void UnitAction(BaseEnemy enemy, BaseHero hero)
    {
        if (hero.CurrentHealth > 0) hero.CurrentHealth -= Damage;
        if (hero.CurrentHealth <= 0) Die(hero);
    }

    //public override void SecondaryUnitAction(BaseEnemy enemy, BaseHero hero)
    //{
    //   //panic roar
    //}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
