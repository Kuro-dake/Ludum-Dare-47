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

        StartCoroutine(Appear());
    }

    IEnumerator Appear()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        yield return intro.Play();

        player.mirage.Play(player.transform.position, true);
        player.transform.Find("someone").Find("eyeglow").gameObject.SetActive(true);
        player.transform.Find("someone").Find("eyeglow (1)").gameObject.SetActive(true);
        GM.sound.PlayResource("move", .2f, new FloatRange(2.1f, 2.3f));
        /*enemy_attack.Initialize();
        player.Initialize();
        loop.Initialize();*/
    }

    public bool shake_screen = false;

    public float shake_screen_strength = .2f;
    Vector3 shake_center;
    IEnumerator ShakeScreenRoutine()
    {
        float shakescreen_inst = shake_screen_strength;
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = false;
        while (shakescreen_inst > .01f)
        {
            shakescreen_inst -= Time.fixedDeltaTime * 2;
            Camera.main.transform.position = shake_center + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0f) * shakescreen_inst;


            yield return null;
        }
        Camera.main.GetComponent<Cinemachine.CinemachineBrain>().enabled = true;
        Camera.main.transform.position = shake_center;
    }

    public static void ShakeScreen()
    {
        //GM.sound.PlayResource();

        inst.shake_center = Camera.main.transform.position;
        //inst.shake_screen_strength = strength;
        inst.StartCoroutine(inst.ShakeScreenRoutine());
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

    [SerializeField]
    AudioManager _sound;
    public static AudioManager sound { get { return inst._sound; } }

    [SerializeField]
    EffectsManager _effects;
    public static EffectsManager effects { get { return inst._effects; } }

    [SerializeField]
    UIManager _ui;
    public static UIManager ui
    {
        get { return inst._ui; }
    }

    [SerializeField]
    CircularIndicator _indicator_prefab;
    [SerializeField]
    introPlayer intro;
    public static CircularIndicator indicator_prefab
    {
        get { return inst._indicator_prefab; }
    }
    #endregion
}
