using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    protected virtual CircularIndicator hp_indicator { get { return GetComponentInChildren<CircularIndicator>(); } }
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

    public virtual Vector2 world_position
    {
        get
        {
            if (position == 2)
            {
                return new Vector2(8.56f, 1.09f);
            }
            return new Vector2(position == 1 ? 3.13f : 5.78f, 1.43f + position * -2.49f);
        }
    }
}
