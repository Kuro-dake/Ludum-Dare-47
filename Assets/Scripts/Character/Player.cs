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
    public override void Initialize()
    {
        pos_x = pos_y = 1;
        base.Initialize();
    }
    public override Vector2 world_position { get
        {
            return GetPlayerGridWorldPosition(pos_x, pos_y);
        } 
    }
    public static Vector2 GetPlayerGridWorldPosition(int x, int y)
    {
        return new Vector2(-5.51f + x * 2f, -1.35f + y * 2f);
    }

    protected override void Die()
    {
        gameObject.SetActive(false);
        GM.enemy_attack.StopAttacking();
    }
}
