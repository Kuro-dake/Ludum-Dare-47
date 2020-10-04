using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{

    bool _shielded = false;
    public bool shielded { 
        get {
            return _shielded;
        }  
        set {
            _shielded = value;
            if(shield == null)
            {
                shield = GM.effects["shield"];
                shield.GetComponent<FollowTransform>().to_follow = transform;
            }
            if (value)
            {
                shield.Play(transform.position);

            }
            else
            {
                shield.Stop();
            }
            
        }
    }
    public bool head = false;

    public override void Initialize()
    {
        base.Initialize();
        target = GM.player;
        
        orig_pos = transform.position;
        StartCoroutine(RandomMovement());
    }
    Vector2 orig_pos;
    Effect shield = null;
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

    protected override void Die()
    {
        base.Die();
        shield.Stop();
    }
}
