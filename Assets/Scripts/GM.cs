using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    
    static GM inst;
    
    
    private void Awake()
    {
        inst = this;
        Initialize();
    }

    void Initialize()
    {
        enemy_attack.Initialize();
        player.Initialize();
        loop.Initialize();
    }

    #region SingletonDefs
    
    [SerializeField]
    Player _player;
    public static Player player { get { return inst._player; } }

    [SerializeField]
    Cursor _cursor;
    public static Cursor cursor { get { return inst._cursor; } }
    [SerializeField]
    Missile _missile_prefab;
    public static Missile missile_prefab { get { return inst._missile_prefab; } }
    [SerializeField]
    Controls _controls;
    public static Controls controls { get { return inst._controls; } }
    [SerializeField]
    Sprite circle;
    public static GameObject CreateCircle()
    {
        GameObject ret = new GameObject("circle");
        ret.AddComponent<SpriteRenderer>().sprite = inst.circle;
        ret.GetComponent<SpriteRenderer>().material = new Material(Shader.Find("Sprites/Default"));
        return ret;
    }

    [SerializeField]
    Loop _loop;
    public static Loop loop { get { return inst._loop; } }

    [SerializeField]
    EnemyAttack _enemy_attack;
    public static EnemyAttack enemy_attack { get { return inst._enemy_attack; } }

    [SerializeField]
    Mirage _mirage;
    public static Mirage mirage { get { return inst._mirage; } }

    [SerializeField]
    Target target_prefab;

    public static Target CreateTarget()
    {
        Target ret = Instantiate(inst.target_prefab);
        return ret;
    }

    #endregion
}
