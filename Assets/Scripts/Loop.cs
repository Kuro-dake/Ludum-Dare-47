using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    [SerializeField]
    Enemy head_prefab, hand_prefab;
    [SerializeField]
    ParticleSystem moving_stars, bg_stars;
    
    public List<Enemy> all_enemies = new List<Enemy>();
    public List<Enemy> alive_enemies { get
        {
            return all_enemies.FindAll(delegate (Enemy e)
            {
                return e.is_alive;
            });
        } }
    public void Initialize()
    {
        StartEnemyLoop();
    }

    public void PlayerDeath()
    {
        StartCoroutine(PlayerDeathStep());
    }
    IEnumerator PlayerDeathStep()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        GM.player.Ressurect();
        all_enemies.ForEach(delegate (Enemy e) { e.Die(); });
        all_enemies.Clear();
        
        GM.controls.active = true;
        GM.enemy_attack.StartAttacking(false);
    }
    Coroutine enemy_loop;
    public void StartEnemyLoop()
    {

        enemy_loop = StartCoroutine(EnemyLoop());
    }
    
    IEnumerator EnemyLoop()
    {
        while (true)
        {

            GM.enemy_attack.StartAttacking();
    
            yield return WatchAlive();
            GM.enemy_attack.StopAttacking();
            yield return MoveToNextNode();

        }
    }
    IEnumerator WatchAlive()
    {
        while(all_enemies.Count > 0)
        {
            all_enemies.RemoveAll(delegate (Enemy e) { return !e.is_alive; });
            yield return null;
        }
        GM.enemy_attack.can_attack = false;
    }
    
    public Enemy head;
    public void ShowHideHeadHP() {
        head.hp_indicator.show = alive_enemies.Count < 2;
    }
    Enemy CreateEnemy(int position, int hand_hp, int head_hp)
    {
        Enemy new_enemy = null;
        switch(position){

            case 0:
                new_enemy = Instantiate(head_prefab);
                new_enemy.head = true;
                head = new_enemy;
                break;
            
            case 1:
                new_enemy = Instantiate(hand_prefab);
                new_enemy.GetComponent<Animator>().SetBool("hand", true);
                
                break;
            
            case 2:
                new_enemy = Instantiate(hand_prefab);
                Vector3 ls = new_enemy.transform.localScale;
                ls.x *= -1;
                new_enemy.transform.localScale = ls;
                break;

            case 3:
                new_enemy = Instantiate(hand_prefab);
                break;
            case 4:

                new_enemy = Instantiate(hand_prefab);
                new_enemy.GetComponent<Animator>().SetBool("hand", true);
            
                break;
            
        }

        all_enemies.Add(new_enemy);
        new_enemy.position = position;
        new_enemy.hp = position == 0 ? head_hp : hand_hp;
        new_enemy.Initialize();

        return new_enemy;
    }
    public void CreateEnemies()
    {
        LoopRound current = GM.enemy_attack.current_loop_round;
        for (int i = 0; i< current.enemy_number; i++)
        {
            CreateEnemy(i, current.hand_hp, current.head_hp);
        }
        ShowHideHeadHP();

    }
    [SerializeField]
    float bg_movement_duration = 2f;
    [SerializeField]
    AudioSource music;
    [SerializeField]
    UnityEngine.UI.Image curtain;
    IEnumerator Win()
    {
        yield return new WaitForSeconds(1f);
        GM.sound.FadeOutSource(music,3f);

        while(curtain.color.a < 1f)
        {
            curtain.color += Color.black * Time.deltaTime * .5f;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(2);
    }
    IEnumerator MoveToNextNode()
    {
        GM.player.hp = GM.player.orig_hp;
        GM.player.hp_indicator.SetNumber(GM.player.hp);
        GM.player.hp_indicator.show = false;
        
        
        GM.controls.active = false;
        yield return new WaitForSeconds(.5f);
        GM.player.Disappear();
        yield return GM.island.Play(false);
        yield return new WaitForSeconds(.2f);

        if(GM.enemy_attack.rounds.Count == 0)
        {
            yield return StartCoroutine(Win());
        }

        moving_stars.Play();
        yield return StartCoroutine(BGMovementStep());
        
        moving_stars.Stop();

        yield return new WaitForSeconds(1f);
        yield return GM.island.Play();

        GM.player.SetPosition(new Vector2Int(1, 1));
        GM.player.Appear();

        GM.controls.active = true;
        GM.player.hp_indicator.show = true;
        yield return new WaitForSeconds(1f);
    }
    [SerializeField]
    float bg_star_motion_speed = -10f;
    IEnumerator BGMovementStep()
    {
        float duration_inverse = 1f / bg_movement_duration * 2f;
        float current = 0f;
        float orig_x = 0f;
        
        ParticleSystem.VelocityOverLifetimeModule volm = bg_stars.velocityOverLifetime;
        ParticleSystem.MinMaxCurve mmc = volm.x;
        while(current < 1f)
        {
            current += Time.deltaTime * duration_inverse;
            mmc.constantMin = Mathf.Lerp(orig_x, bg_star_motion_speed, current);
            mmc.constantMax = mmc.constantMin * 2f;
            volm.x = mmc;
            yield return null;
        }
        while (current > 0f)
        {
            current -= Time.deltaTime * duration_inverse;
            mmc.constantMin = Mathf.Lerp(orig_x, bg_star_motion_speed, current);
            mmc.constantMax = mmc.constantMin * 2f;
            volm.x = mmc;
            yield return null;
        }
        
    }
    

    public Enemy GetEnemyAtPosition(int position)
    {
        return all_enemies.Find(delegate (Enemy e)
        {
            return e.position == position;
        });
    }

    
}
