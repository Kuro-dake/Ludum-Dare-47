using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    // Update is called once per frame
    int _pos_x, _pos_y;
    public int max_pos { get; protected set; } = 2;
    public int pos_x
    {
        get
        {
            return _pos_x;
        }
        set
        {
            _pos_x = Mathf.Clamp(value, 0, 2);
        }
    }

    public int pos_y
    {
        get
        {
            return _pos_y;
        }
        set
        {
            _pos_y = Mathf.Clamp(value, 0, 2);
        }
    }
    public void SetPosition(Vector2Int pos)
    {
        pos_x = pos.x;
        pos_y = pos.y;
        RefreshPosition(false);
    }
    public Vector2Int position
    {
        get
        {
            return new Vector2Int(pos_x, pos_y);
        }
        set
        {
            pos_x = value.x;
            pos_y = value.y;
            RefreshPosition();
        }
    }
    public Mirage mirage { get { return GetComponent<Mirage>(); } }
    public override void RefreshPosition(bool play_mirage = true)
    {
        if (play_mirage)
        {
            GM.mirage.Play(transform.position);
        }
        
        base.RefreshPosition();
        mirage.Play(transform.position, true);
        foreach(SpriteRenderer sr in transform.GetComponentsInChildren<SpriteRenderer>(true))
        {
            sr.sortingOrder = position.y * -10 - 5;
            if(sr.name == "color Merged")
            {
                sr.sortingOrder -= 1;
            }
        }
        //hp_indicator.transform.position = transform.position + Vector3.down * 1.3f;
    }
    public int orig_hp { get; protected set; }
    
    public override void Initialize()
    {
        pos_x = pos_y = 1;
        base.Initialize();
        orig_hp = hp;
    }
    public override Vector2 world_position { get
        {
            return GetPlayerGridWorldPosition(pos_x, pos_y);
        } 
    }
    static Vector2 offset { get { return new Vector2(+1.0f, 0f); } }
    public static Vector2 GetPlayerGridWorldPosition(int x, int y)
    {
        return new Vector2(-7.92f + x * 2f, -1.35f + y * 1.3f) + offset;
    }

    public void Appear()
    {
        transform.Find("someone").Find("eyeglow").gameObject.SetActive(true);
        transform.Find("someone").Find("eyeglow (1)").gameObject.SetActive(true);
        mirage.Play(transform.position, true);
        GM.sound.PlayResource("move", .2f, new FloatRange(2.1f, 2.3f));
    }

    public void Disappear()
    {
        transform.Find("someone").Find("eyeglow").gameObject.SetActive(false);
        transform.Find("someone").Find("eyeglow (1)").gameObject.SetActive(false);
        mirage.Play(transform.position);
        GM.sound.PlayResource("move", .2f, new FloatRange(2.1f, 2.3f));
    }

    public void Ressurect()
    {
        hp = orig_hp;
        hp_indicator.ModifyNumber(hp);
        Appear();
        
    }

    public override void Die()
    {
        Disappear();
        GM.controls.active = false;
        GM.enemy_attack.StopAttacking();
        GM.loop.PlayerDeath();
    }
    /*[SerializeField]
    CircularIndicator _hp_indicator;
    protected override CircularIndicator hp_indicator
    {
        get
        {
            return _hp_indicator;
        }
    }*/
}
