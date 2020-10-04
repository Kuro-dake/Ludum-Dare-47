using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    
    public FollowTransform this[char c]
    {
        get
        {
            return transform.Find(c.ToString()).GetComponent<FollowTransform>();
        }
    }

    public void HideAll()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    Dictionary<char, Vector2> default_positions = new Dictionary<char, Vector2>()
    {
        {'w', new Vector2(-7.51f, -3.51f)},
        {'a', new Vector2(-9.92f, -5.44f)},
        {'s', new Vector2(-7.5f, -5.44f)},
        {'d', new Vector2(-5.17f, -5.3f)},

    };

    public void ShowControls()
    {
        HideAll();
        foreach(KeyValuePair<char, Vector2> kv in default_positions)
        {
            this[kv.Key].to_follow = null;
            this[kv.Key].transform.position = kv.Value;
            this[kv.Key].gameObject.SetActive(true);
        }
    }

}
