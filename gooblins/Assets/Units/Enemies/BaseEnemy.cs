using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : BaseUnit
{
    public int alertness;

    public enum Alertness 
    {
        Normal = 1,
        On_Edge = 2,
        In_Combat = 3
    }

    public virtual void Patrol()
    {
        //bool around bruh
    }

}
