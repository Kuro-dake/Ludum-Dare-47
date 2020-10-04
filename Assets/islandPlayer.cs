using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class islandPlayer : MonoBehaviour
{

    private void Awake()
    {
        transform.localScale = Vector3.zero;
        orig_light_intensity = light.intensity;
        orig_light_radius = light.pointLightOuterRadius;
        light.intensity = 0f;
        light.pointLightOuterRadius = 0f;
    }

    public Coroutine Play(bool appear = true)
    {
        return StartCoroutine(PlayStep(appear));
    }
    [SerializeField]
    Light2D light;
    [SerializeField]
    float orig_light_radius, orig_light_intensity;
    IEnumerator PlayStep(bool appear)
    {
        
        Vector3 orig_scale = appear ? Vector3.zero : Vector3.one;
        Vector3 target_scale = appear ? Vector3.one : Vector3.zero;
        float current = 0f;
        float pi_half = Mathf.PI * .5f;

        float start_intensity = 0f;
        float target_intensity = orig_light_intensity;

        float start_radius = 0f;
        float target_radius = orig_light_radius;

        

        if (!appear)
        {
            float ftemp = start_intensity;
            start_intensity = target_intensity;
            target_intensity = ftemp;

            ftemp = start_radius;
            start_radius = target_radius;
            target_radius = ftemp;
        }

        transform.localScale = orig_scale;

        float duration_inverse = 1f / 3f;

        while (current < 1f)
        {
            current = Mathf.Clamp(current + Time.deltaTime * duration_inverse, 0f, 1f);
            float multiplier = (Mathf.Sin(current * Mathf.PI - pi_half) + 1f) * .5f;
            transform.localScale = Vector3.Lerp(orig_scale, target_scale, multiplier);

            light.intensity = Mathf.Lerp(start_intensity, target_intensity, multiplier);
            light.pointLightOuterRadius = Mathf.Lerp(start_radius, target_radius, multiplier);

            yield return null;
        }

        yield return null;
    }

}
