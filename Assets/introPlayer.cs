using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class introPlayer : MonoBehaviour
{

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public Coroutine Play()
    {
        return StartCoroutine(PlayStep());
    }

    IEnumerator PlayStep()
    {
        transform.localScale = Vector3.zero;
        Vector3 orig_scale = Vector3.zero;
        Vector3 target_scale = Vector3.one;
        float current = 0f;
        float pi_half = Mathf.PI * .5f;

        float duration_inverse = 1f / 3f;

        while ((current += Time.deltaTime * duration_inverse) < 1f)
        {
            current = Mathf.Clamp(current + Time.deltaTime, 0f, 1f);
            float multiplier = (Mathf.Sin(current * Mathf.PI - pi_half) + 1f) * .5f;
            transform.localScale = Vector3.Lerp(orig_scale, target_scale, multiplier);
            yield return null;
        }

        yield return null;
    }

}
