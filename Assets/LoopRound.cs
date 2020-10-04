using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopRound
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
    public int rounds_number { get; protected set; }

    public int hand_hp = 3;
    public int head_hp = 3;
    public int enemy_number = 3;

    public LoopRound(int attack_rounds, int attack_rounds_sequence, IntRange free_spots, int _enemy_number, int _hand_hp, int _head_hp)
    {
        Random.State prev_state = Random.state;
        rounds_number = attack_rounds;
        // seed here
        for (int round = 0; round < attack_rounds; round++)
        {
            Queue<List<Pair<int, int>>> q = new Queue<List<Pair<int, int>>>();
            for (int single_attack = 0; single_attack < attack_rounds_sequence; single_attack++)
            {
                q.Enqueue(GetToBeAttacked(free_spots));
            }
            rounds_queue.Enqueue(q);
        }
        hand_hp = _hand_hp;
        head_hp = _head_hp;
        enemy_number = _enemy_number;
        Random.state = prev_state;
        NextSequence();
    }

    public void NextSequence()
    {
        if (current_rounds_queue.Count == 0)
        {
            current_rounds_queue = new Queue<Queue<List<Pair<int, int>>>>(rounds_queue);
            GM.ui.current_round = 0;
        }
        attacked_positions_queue = new Queue<List<Pair<int, int>>>(current_rounds_queue.Dequeue());
    }

    public List<Pair<int, int>> DequeueAttackedPositions()
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
