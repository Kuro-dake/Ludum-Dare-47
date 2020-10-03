using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EnemyAttack : MonoBehaviour
{
    Coroutine attack_routine;
    Queue<LoopRound> rounds;
    public void Initialize()
    {
        
        LoopRound.InitializeStatic();

        List<LoopRound> rq = new List<LoopRound>() {
            new LoopRound(1,3,new IntRange(6,8)),
            new LoopRound(2,3,new IntRange(6,8)),
            new LoopRound(5,4,new IntRange(4,5))
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

    public void StartAttacking()
    {
        StopAttacking();
        if(rounds.Count > 0)
        {
            GM.loop.CreateEnemies(); 
            current_loop_round = rounds.Dequeue();
            attack_routine = StartCoroutine(AttackStep());
        }
        else
        {
            Debug.Log("victory");
        }
    }
    [SerializeField]
    float wait_phase, after_wait_phase;
    
    List<Pair<int, int>> to_be_attacked = new List<Pair<int, int>>();
    List<GameObject> attack_markers = new List<GameObject>();
    
    void CheckAttack()
    {
        foreach(Pair<int,int> p in to_be_attacked)
        {
            
            if(p.first == GM.player.pos_x && p.second == GM.player.pos_y)
            {
                Debug.Log(p.first + " " + p.second);
                GM.loop.all_enemies[0].Attack(GM.player);
                break;
            }
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
            can_attack_indicator.gameObject.SetActive(value);
        } }
    LoopRound current_loop_round;
    IEnumerator AttackStep() {
        while (true)
        {
            while(current_loop_round.are_positions_queued)
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
                yield return new WaitForSeconds(after_wait_phase);
            }
            
            can_attack = true;
            yield return new WaitForSeconds(wait_phase);
            can_attack = false;
            yield return new WaitForSeconds(after_wait_phase);
            current_loop_round.NextSequence();
        }
    }
}

class LoopRound
{
    Queue<Queue<List<Pair<int, int>>>> current_rounds_queue = new Queue<Queue<List<Pair<int, int>>>>();
    Queue<Queue<List<Pair<int, int>>>> rounds_queue = new Queue<Queue<List<Pair<int, int>>>>();
    Queue<List<Pair<int, int>>> attacked_positions_queue = new Queue<List<Pair<int, int>>>();
    static List<Pair<int, int>> to_be_attacked_template = new List<Pair<int, int>>();
    
    static List<Pair<int, int>> GetToBeAttacked(int free_spots)
    {
        List<Pair<int, int>> to_be_attacked = new List<Pair<int, int>>();
        to_be_attacked.Clear();
        to_be_attacked_template.ForEach(delegate (Pair<int, int> p) { to_be_attacked.Add(p); });
        for (int i = 0; i < free_spots; i++)
        {
            to_be_attacked.Remove(to_be_attacked[Random.Range(0, to_be_attacked.Count)]);
        }
        return to_be_attacked;
    }

    public LoopRound(int attack_rounds, int attack_rounds_sequence, IntRange free_spots)
    {
        Random.State prev_state = Random.state;
        // seed here
        for(int round = 0; round < attack_rounds; round++)
        {
            Queue<List<Pair<int, int>>> q = new Queue<List<Pair<int, int>>>();
            for (int single_attack = 0; single_attack < attack_rounds_sequence; single_attack++)
            {
                q.Enqueue(GetToBeAttacked(free_spots));
            }
            rounds_queue.Enqueue(q);
        }
        Random.state = prev_state;
        NextSequence();
    }

    public void NextSequence() {
        if(current_rounds_queue.Count == 0)
        {
            current_rounds_queue = new Queue<Queue<List<Pair<int, int>>>>(rounds_queue);
        }
        attacked_positions_queue = new Queue<List<Pair<int, int>>>(current_rounds_queue.Dequeue());
    }

    public List<Pair<int,int>> DequeueAttackedPositions()
    {
        return attacked_positions_queue.Dequeue();
    }

    public bool are_positions_queued
    {
        get
        {
            return attacked_positions_queue.Count > 0;
        }
    }

    public static void InitializeStatic()
    {
        
        for (int x = 0; x <= GM.player.max_pos; x++)
        {
            for (int y = 0; y <= GM.player.max_pos; y++)
            {
                to_be_attacked_template.Add(new Pair<int, int>(x, y));
            }
        }
    }
}
