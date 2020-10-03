using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    public override void Initialize()
    {
        base.Initialize();
        target = GM.player;
        
        orig_pos = transform.position;
        StartCoroutine(RandomMovement());
    }
    Vector2 orig_pos;
    IEnumerator RandomMovement()
    {

        while (true)
        {
            Vector2 target = orig_pos + Random.insideUnitCircle * .5f;
            float speed = Random.Range(.8f, 1f) * .25f;
            while(Vector2.Distance(transform.position, target) > .1f)
            {
                
                transform.position = Vector2.MoveTowards(transform.position, target, Time.deltaTime * speed);
                yield return null;

            }
            
        }
    }

    protected override void Update()
    {
        base.Update();

    }
}
