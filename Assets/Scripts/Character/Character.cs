using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    CircularIndicator _hp_indicator;
    protected virtual CircularIndicator hp_indicator { get { return _hp_indicator; } }
    // Start is called before the first frame update

    public Character target;


    protected float _attack_timeout = 0f;
    [SerializeField]
    protected float attack_timeout = .3f;

    // Update is called once per frame
    protected virtual void Update()
    {
        //UpdateAttack();
    }
    public virtual void RefreshPosition()
    {
        transform.position = world_position;
    }
    public virtual void Initialize()
    {
        RefreshPosition();
        
        _hp_indicator = Instantiate(GM.indicator_prefab);
        FollowTransform ft = _hp_indicator.GetComponent<FollowTransform>();
        ft.to_follow = transform;
        ft.offset = Vector2.down * 2.2f;
        
        hp_indicator.SetNumber(hp);
    }

    public void Attack(Character target)
    {
        target.ReceiveDamage(1);
    
        /*Missile m = Instantiate(GM.missile_prefab);
        m.transform.position = transform.position;
        m.Initialize(target);*/
    }

    /*void UpdateAttack()
    {
        if ((_attack_timeout += Time.deltaTime) >= attack_timeout)
        {
            if (target != null)
            {
                Attack();
                _attack_timeout = 0f;
            }

        }
    }*/

    [SerializeField]
    int hp = 10;
    public void ReceiveDamage(int damage = 1)
    {
        if (!is_alive)
        {
            return;
        }
        hp -= damage;
        hp_indicator.ModifyNumber(hp);
        if (!is_alive)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool is_alive
    {
        get
        {
            return hp > 0;
        }
    }

    public int position;
    static Dictionary<int, Vector2> positions = new Dictionary<int, Vector2>() {
        {0, new Vector2(5.78f, 1.43f) },
        {1, new Vector2(3.13f, -1.06f) },
        {2, new Vector2(8.56f, 1.09f) },
        {3, new Vector2(6.05f, -2.07f) },
        {4, new Vector2(4.85f, 6.48f) },
    };
    public virtual Vector2 world_position
    {
        get
        {
            return positions[position];
        }
    }
}
