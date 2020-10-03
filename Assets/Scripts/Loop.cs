using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loop : MonoBehaviour
{
    [SerializeField]
    Enemy enemy_prefab;
    [SerializeField]
    ParticleSystem moving_stars, bg_stars;
    
    public List<Enemy> all_enemies = new List<Enemy>();
    
    public void Initialize()
    {
        StartCoroutine(EnemyLoop());
    }

    IEnumerator WatchAlive()
    {
        while(all_enemies.Count > 0)
        {
            all_enemies.RemoveAll(delegate (Enemy e) { return !e.is_alive; });
            yield return null;
        }
    }
    Queue<LoopRound> rounds = new Queue<LoopRound>();
    
    public void CreateEnemies()
    {
        for (int i = 0; i < 3; i++)
        {
            Enemy new_enemy = Instantiate(enemy_prefab);
            all_enemies.Add(new_enemy);
            new_enemy.position = i;
            new_enemy.Initialize();
        }
        
    }
    [SerializeField]
    float bg_movement_duration = 2f;
    IEnumerator MoveToNextNode()
    {
        moving_stars.Play();
        yield return StartCoroutine(BGMovementStep());
        
        moving_stars.Stop();
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

    
}
