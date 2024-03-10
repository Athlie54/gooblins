using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseHero : BaseUnit
{
    //public static BaseHero Instance;
    public int treasure;
    public bool actionReady;

    // Start is called before the first frame update
    void Start()
    {
        treasure = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
