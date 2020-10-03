using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Initialize()
    {
        base.Initialize();
        target = GM.player;
    }
}
