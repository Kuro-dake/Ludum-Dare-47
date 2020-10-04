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
                
                letter_follow.to_follow = transform;
                letter_follow.gameObject.SetActive(true);
            }
            else
            {
                shield.Stop();
                letter_follow.gameObject.SetActive(false);
            }
            
        }
    }
    FollowTransform letter_follow
    {
        get
        {
            GM.keys[position_char].RandomRotation();
            return GM.keys[position_char].GetComponent<FollowTransform>();
        }
    }
    char position_char
    {
        get { 
            foreach(KeyValuePair<char,int> kv in enemy_positions)
            {
                if(kv.Value == position)
                {
                    return kv.Key;
                }
            }
            throw new UnityException("No char for this position.");
        }
    }
    public static Dictionary<char, int> enemy_positions = new Dictionary<char, int>() {
        {'q', 0 },
        {'a', 1 },
        {'d', 2 },
        {'s', 3 },
        {'w', 4 },

    };

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

    public override void Die()
    {

        letter_follow.gameObject.SetActive(false);
        base.Die();
        Destroy(hp_indicator.gameObject);
        GM.loop.ShowHideHeadHP();
        shield.Stop();
        
    }

    public override void ReceiveDamage(int damage = 1)
    {
        base.ReceiveDamage(damage);
        //letter_follow.gameObject.SetActive(false);
    }
}
