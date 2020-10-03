using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirage : MonoBehaviour
{
    // Start is called before the first frame update
    Vector2 orig_scale;
    void Awake()
    {
        orig_scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    Coroutine mirage_routine;
    public void Play(Vector2 position, bool reverse = false)
    {
        transform.position = position;
        if(mirage_routine != null)
        {
            StopCoroutine(mirage_routine);
        }
        mirage_routine = StartCoroutine(PlayStep(reverse));
    }
    [SerializeField]
    float duration = 1f, x_scale_target = .5f, y_scale_target = 2f;
    [SerializeField]
    Color start_color = Color.white, end_color = Color.clear;
    IEnumerator PlayStep(bool reverse)
    {
        
        float current = reverse ? 1f : 0f;
        float current_target = reverse ? 0f : 1f;
        float duration_inverse = 1f / duration;



        Vector2 target_scale = new Vector2(orig_scale.x * x_scale_target , orig_scale.y * y_scale_target);
        SpriteRenderer[] srs = GetComponentsInChildren<SpriteRenderer>();
        while(!Mathf.Approximately(current, current_target))
        {
            current = Mathf.MoveTowards(current, current_target, Time.deltaTime * duration_inverse);
            //current += Time.deltaTime * duration_inverse;
            foreach(SpriteRenderer sr in srs)
            {
                sr.color = Color.Lerp(start_color, end_color, current);
            }
            transform.localScale = Vector2.Lerp(orig_scale, target_scale, current);
            yield return null;
        }
        mirage_routine = null;
    }
}
