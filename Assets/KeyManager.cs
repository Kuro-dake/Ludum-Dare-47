using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    [SerializeField]
    float letter_size = .5f, pop_letter_size = 1f, pop_letter_duration = .2f;
    public Letter this[char c]
    {
        get
        {
            FollowTransform ft = transform.Find(c.ToString()).GetComponent<FollowTransform>();

            Letter ret = ft.GetComponent<Letter>();
            if (ret == null)
            {
                ret = ft.gameObject.AddComponent<Letter>();
            }
            return ret;
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
            this[kv.Key].ft.to_follow = null;
            //this[kv.Key].transform.position = kv.Value;
            this[kv.Key].gameObject.SetActive(true);
        }
    }

    Dictionary<char, Coroutine> pop_routines = new Dictionary<char, Coroutine>();
    Dictionary<KeyCode, char> charmap = new Dictionary<KeyCode, char>()
    {
        {KeyCode.W, 'w' },
        {KeyCode.A, 'a' },
        {KeyCode.S, 's' },
        {KeyCode.D, 'd' },
        {KeyCode.Q, 'q' },
    };
    private void Update()
    {
        foreach(KeyValuePair<KeyCode, char> kv in charmap)
        {
            if (Input.GetKeyDown(kv.Key))
            {
                char c = kv.Value;
                if(pop_routines.ContainsKey(c) && pop_routines[c] != null)
                {
                    StopCoroutine(pop_routines[c]);
                    
                }
                pop_routines[c] = StartCoroutine(PopLetterStep(c));
            }
        }
    }
    [SerializeField]
    public FloatRange rotation_range = new FloatRange(-10f, 10f);

    IEnumerator PopLetterStep(char c)
    {
        Letter l = this[c];
        float current = 0f;
        float speed = 1f / pop_letter_duration;
        l.RandomRotation();
        while(current < 1f)
        {
            current += Time.deltaTime * speed;
            l.transform.localScale = Vector3.Lerp(Vector3.one * pop_letter_size, Vector3.one * letter_size, current);
            yield return null;
        }
        pop_routines[c] = null;
    }
}

public class Letter: MonoBehaviour
{
    public void RandomRotation()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, GM.keys.rotation_range);
    }
    public bool active
    {
        get { return gameObject.activeSelf; }
        set { gameObject.SetActive(value); }
    }

    public FollowTransform ft { get { return GetComponent<FollowTransform>(); } }
}