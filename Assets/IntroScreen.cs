using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    [SerializeField]
    Text title, subtitle;

    [SerializeField]
    TextWriter[] intro_texts;
    [SerializeField]
    Image curtain;

    [SerializeField]
    AudioSource music;

    static IntroScreen inst;
    public static AudioManager sound { get { return inst != null ? inst.GetComponent<AudioManager>() : null; } }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        inst = this;
        StartCoroutine(IntroStep());
    }
    Color orig_color;
    [SerializeField]
    float appearance_duration = .3f, y_diff = 50f;
    IEnumerator IntroStep()
    {
        curtain.color = Color.black;
        yield return new WaitForSeconds(.5f);

        Text[] texts = new Text[] { title, subtitle };

        orig_color = title.color;
        Color cc = orig_color;
        cc.a = 0f;

        foreach (Text t in texts)
        {
            t.color = cc;
            t.GetComponent<RectTransform>().anchoredPosition += Vector2.up * y_diff;
        }

        while (curtain.color.a > 0f)
        {
            curtain.color -= Color.black * Time.deltaTime * .5f;
            yield return null;
        }
        

        

        yield return new WaitForSeconds(.5f);

        float current = 0f;
        float dur_inv = 1f / appearance_duration;

        foreach (Text t in texts)
        {
            current = 0f;
            
            RectTransform rt = t.GetComponent<RectTransform>();
            Vector2 orig_pos = rt.anchoredPosition;
            Vector2 target_position = rt.anchoredPosition + Vector2.down * y_diff;
            while(current < 1f)
            {
                current += Time.deltaTime * dur_inv;

                rt.anchoredPosition = Vector2.Lerp(orig_pos, target_position, current);
                t.color = Color.Lerp(cc, orig_color, current);

                yield return null;

            }
            
        }

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }


        foreach (Text t in texts)
        {
            current = 0f;

            RectTransform rt = t.GetComponent<RectTransform>();
            Vector2 orig_pos = rt.anchoredPosition;
            Vector2 target_position = rt.anchoredPosition + Vector2.down * y_diff;
            while (current < 1f)
            {
                current += Time.deltaTime * dur_inv;

                rt.anchoredPosition = Vector2.Lerp(orig_pos, target_position, current);
                t.color = Color.Lerp(orig_color, cc, current);

                yield return null;

            }

        }
        yield return new WaitForSeconds(.3f);

        yield return intro_texts[0].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[1].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[2].Write();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        for(int i = 0; i < 3; i++)
        {
            intro_texts[i].FadeOut();
            yield return new WaitForSeconds(.3f);
        }

        sound.FadeOutSource(music, 3f);

        while(curtain.color.a < 1f)
        {
            curtain.color += Color.black * Time.deltaTime * .5f;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        Application.LoadLevel(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
