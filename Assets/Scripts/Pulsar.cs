using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Pulsar : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Pulsate());
    }
    IEnumerator Pulsate()
    {
        yield return null;
        Vector3 localscale = transform.localScale;
        while (true)
        {
            transform.localScale = localscale * (1f + (Mathf.Sin(Time.realtimeSinceStartup * 8) + 2f) / 14f);
            yield return null;
        }
    }
}
