using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal;

public class EnemyAttack : MonoBehaviour
{
    Coroutine attack_routine;
    public Queue<LoopRound> rounds { get; protected set; }
    public void Initialize()
    {

        LoopRound.InitializeStatic();

        List<LoopRound> rq = new List<LoopRound>() {
            
            new LoopRound(2, 2, new IntRange(5,7), 1 ,1,5),
            new LoopRound(3, 2, new IntRange(4,6), 2, 3,6),
            new LoopRound(4, 3, new IntRange(3,5), 3, 4, 7),
            new LoopRound(5, 4, new IntRange(2,5), 4, 4, 7),
            new LoopRound(6, 5, new IntRange(1,3), 5, 6, 10),
            
            
        };

        rounds = new Queue<LoopRound>(rq);
    }

    public void StopAttacking()
    {
        if (attack_routine != null)
        {
            StopCoroutine(attack_routine);
            attack_routine = null;
        }
    }

    public bool StartAttacking(bool dequeue = true)
    {
        StopAttacking();
        if (rounds.Count > 0 || !dequeue)
        {
            
            if (dequeue)
            {
                current_loop_round = rounds.Dequeue();
                current_loop_round_full = current_loop_round.clone;
            }
            else
            {
                current_loop_round = current_loop_round_full.clone;
            }

            

            GM.loop.CreateEnemies();
            attack_routine = StartCoroutine(AttackStep());
            return true;
        }
        else
        {
            
            return false;
            
        }
    }
    [SerializeField]
    float wait_phase, after_wait_phase;

    List<Pair<int, int>> to_be_attacked = new List<Pair<int, int>>();
    List<GameObject> attack_markers = new List<GameObject>();

    
    void CheckAttack()
    {
        float volume = .3f / to_be_attacked.Count;
        Transform head = GM.loop.head.transform;
        Transform eye1 = head.Find("someone").Find("eyeglow");
        Transform eye2 = head.Find("someone").Find("eyeglow (1)");

        Effect e = GM.effects["explosion"];
        e.transform.localScale = Vector3.one * .5f;
        e.Play(eye1.position);
        Destroy(e.GetComponent<Light2D>());
        Destroy(e.GetComponent<LightGlow>());;
        e = GM.effects["explosion"];
        e.transform.localScale = Vector3.one * .5f;
        e.Play(eye2.position);

        Destroy(e.GetComponent<Light2D>());
        Destroy(e.GetComponent<LightGlow>()); ;

        foreach (Pair<int, int> p in to_be_attacked)
        {
            Effect ex = GM.effects["explosion"];

            ex.GetComponent<ParticleSystemRenderer>().sortingOrder = p.second * -10;
            ex.Play(Player.GetPlayerGridWorldPosition(p.first, p.second));
            if (p.first == GM.player.pos_x && p.second == GM.player.pos_y)
            {
                Debug.Log(p.first + " " + p.second);
                GM.loop.all_enemies[0].Attack(GM.player);
                GM.ShakeScreen();
                GM.sound.PlayResource("hit", 1f, new FloatRange(1f, 1.2f));
                GM.effects["hit"].Play(GM.player.transform.position + Random.insideUnitCircle.Vector3() * .5f + Vector3.up * 1.5f);
            }
            
            GM.sound.PlayResource("explode", volume, new FloatRange(.5f, .7f));
        }
    }
    void ClearMarkers()
    {
        attack_markers.ForEach(delegate (GameObject go) { Destroy(go); });
        to_be_attacked.Clear();
    }
    [SerializeField]
    int avoid_phase_rounds = 3;
    [SerializeField]
    Image can_attack_indicator;
    bool _can_attack = false;
    public bool can_attack { get {
            return _can_attack;
        }
        set {
            _can_attack = value;
            //can_attack_indicator.gameObject.SetActive(value);
        } }
    public LoopRound current_loop_round;
    public LoopRound current_loop_round_full;
    void SetShields(bool shields)
    {
        if (!shields)
        {
            GM.loop.all_enemies.ForEach(delegate (Enemy e)
            {
                e.shielded = false;
            });
        }
        else
        {
            GM.loop.all_enemies.ForEach(delegate (Enemy e)
            {
                e.shielded = false;
            });
            List<Enemy> hands = GM.loop.all_enemies.FindAll(delegate (Enemy e)
            {
                return !e.head;
            });
            if(hands.Count > 0)
            {
                hands[Random.Range(0, hands.Count)].shielded = true;
            }
            else
            {
                GM.loop.all_enemies[0].shielded = true;
            }
        }
    }

    IEnumerator AttackStep() {
        SetShields(false);
        GM.ui.max_rounds = current_loop_round.rounds_number;
        GM.ui.current_round = 0;
        while (true)
        {
            while (current_loop_round.are_positions_queued)
            {
                foreach (Pair<int, int> p in current_loop_round.DequeueAttackedPositions())
                {
                    GameObject marker = GM.CreateTarget().gameObject;
                    to_be_attacked.Add(p);
                    marker.transform.position = Player.GetPlayerGridWorldPosition(p.first, p.second);
                    marker.GetComponent<SpriteRenderer>().sortingLayerName = "AboveGround";
                    attack_markers.Add(marker);
                    marker.AddComponent<Pulsar>();
                }
                yield return new WaitForSeconds(wait_phase);
                CheckAttack();
                ClearMarkers();
                /*if (!current_loop_round.are_positions_queued)
                {
                    can_attack = true;
                    SetShields(true);
                }*/
                
                yield return new WaitForSeconds(after_wait_phase);
                
            }
            
            can_attack = true;
            SetShields(true);
            yield return new WaitForSeconds(wait_phase);
            SetShields(false);
            can_attack = false;
            yield return new WaitForSeconds(after_wait_phase);
            GM.ui.current_round++;
            current_loop_round.NextSequence();
            
        }
    }
}

