using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    // Start is called before the first frame update
    public int enemy_mask { get; protected set; }
    void Start()
    {
        enemy_mask = LayerMask.GetMask(new string[] { "Enemy" });
    }

    Dictionary<KeyCode, Vector2Int> dirs = new Dictionary<KeyCode, Vector2Int>() {
        {KeyCode.W, Vector2Int.up },
        {KeyCode.S, Vector2Int.down },
        {KeyCode.A, Vector2Int.left },
        {KeyCode.D, Vector2Int.right },
    };

    // Update is called once per frame
    void Update()
    {
        if (!GM.player.is_alive)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && GM.enemy_attack.can_attack)
        {
            Collider2D col = Physics2D.OverlapCircle(GM.cursor.mouse_world_position, .1f, enemy_mask);
            if(col != null)
            {
                if (col.GetComponent<Enemy>().shielded)
                {
                    
                    GM.player.Attack(col.GetComponent<Enemy>());
                    //GM.enemy_attack.can_attack = false;
                    // attack enemy
                    GM.mirage.Play(col.transform.position + Vector3.down * .5f + Vector3.left * .5f);
                    GM.player.mirage.Play(GM.player.transform.position, true);
                    GM.sound.PlayResource("move", .05f, new FloatRange(3.1f, 3.3f));
                    GM.sound.PlayResource("hit", .5f, new FloatRange(1f, 1.2f));
                }
                
            }
        }
        foreach(KeyValuePair<KeyCode, Vector2Int> kv in dirs)
        {
            if (Input.GetKeyDown(kv.Key))
            {
                GM.player.position += kv.Value;
                GM.sound.PlayResource("move", .05f, new FloatRange(3.1f,3.3f));
            }
        }
    }

    
}
