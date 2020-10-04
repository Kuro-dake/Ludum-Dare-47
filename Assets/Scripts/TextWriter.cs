using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWriter : MonoBehaviour
{
    [SerializeField]
    float write_delay = .1f;
    [SerializeField]
    float erase_delay = .03f;

    public string text;
    int index = 0;
    Text ui_text
    {
        get
        {
            return GetComponent<Text>();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        text = ui_text.text;
        ui_text.text = "";
        //gameObject.SetActive(false);
    }
    int cursor = 0;
    // Update is called once per frame
    Coroutine routine = null;
    void StopAll()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
        }
    }
    public Coroutine Write(bool erase = true, bool instant = false)
    {
        cursor = 0;
        StopAll();
        if (instant)
        {
            cursor = erase ? 0 : text.Length;
            ui_text.text = text.Substring(0, cursor);
            return null;
        }
        return routine = StartCoroutine(WriteStep());
    }
    IEnumerator WriteStep(bool instant = false)
    {

        for (; cursor <= text.Length; cursor++)
        {

            if (cursor != text.Length && (new List<string>() { "\n", " " }).Contains(text.Substring(cursor, 1)))
            {
                continue;
            }
            ui_text.text = text.Substring(0, cursor);
            sound.PlaySound("type", .2f);
            //yield return null;
            yield return new WaitForSeconds(Random.Range(write_delay, write_delay + write_delay / 2f));
        }
        cursor -= 1;
        routine = null;
    }
    public bool is_working
    {
        get
        {
            
            return routine != null;
        }
    }
    public Coroutine Erase()
    {
        StopAll();
        return routine = StartCoroutine(EraseStep());
    }
    AudioManager sound { get { return IntroScreen.sound != null ? IntroScreen.sound : OutroScreen.sound; } }
    IEnumerator EraseStep()
    {

        for (; cursor >= 0; cursor -= Mathf.Clamp(erase_delay < .01f ? 8 : 1, 0, int.MaxValue))
        {
            ui_text.text = text.Substring(0, cursor);
            
            sound.PlaySound("type", .2f, new FloatRange(.4f, .6f));
            yield return new WaitForSeconds(Random.Range(erase_delay / 2f, erase_delay));
        }
        ui_text.text = "";
        routine = null;
    }
    bool _shaking = false;
    public bool shaking
    {
        get
        {
            return _shaking;
        }
        set
        {
            _shaking = value;
            if (shaking)
            {
                if (shaking_routine != null)
                {
                    return;
                }
                shaking_routine = StartCoroutine(ShakingStep());
            }
            else
            {
                StopShaking();
            }
        }
    }
    Coroutine shaking_routine = null;
    Vector2 orig_pos;
    IEnumerator ShakingStep()
    {

        orig_pos = rt.anchoredPosition;
        
        while (true)
        {
            rt.anchoredPosition = orig_pos + Random.insideUnitCircle.normalized * 3f;
            yield return null;
        }
    }
    void StopShaking()
    {
        if (shaking_routine != null)
        {
            rt.anchoredPosition = orig_pos;
            StopCoroutine(shaking_routine);
        }
    }
    public Coroutine FadeOut()
    {
        return ChangeColor(Color.clear);
    }
    public Coroutine ChangeColor(Color c, float duration = 1f)
    {
        return StartCoroutine(ChangeColorStep(c, duration));
    }
    IEnumerator ChangeColorStep(Color to_color, float duration = 1f)
    {

        Color c = ui_text.color;
        duration = 1f / duration;
        float current = 0f;
        while(current < 1f)
        {
            current += Time.deltaTime * duration;
            ui_text.color = Color.Lerp(c, to_color, current);
            yield return null;
        }
        

    }
    RectTransform rt { get { return GetComponent<RectTransform>(); } }
}
