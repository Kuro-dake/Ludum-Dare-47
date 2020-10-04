using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OutroScreen : MonoBehaviour
{

    [SerializeField]
    Text title, subtitle;

    [SerializeField]
    TextWriter[] intro_texts;
    [SerializeField]
    Image curtain;

    static OutroScreen inst;
    public static AudioManager sound { get { return inst.GetComponent<AudioManager>(); } }

    // Start is called before the first frame update
    void Start()
    {
        inst = this;
        StartCoroutine(IntroStep());
    }
    [SerializeField]
    AudioSource music;
    Color orig_color;
    [SerializeField]
    float appearance_duration = .3f, y_diff = 50f;
    IEnumerator IntroStep()
    {
        curtain.color = Color.clear;

        yield return new WaitForSeconds(.6f);
        yield return intro_texts[0].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[1].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[2].Write();

        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }

        for (int i = 0; i < 3; i++)
        {
            intro_texts[i].FadeOut();
            yield return new WaitForSeconds(.3f);
        }

        yield return new WaitForSeconds(.6f);
        yield return intro_texts[3].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[4].Write();
        yield return new WaitForSeconds(.2f);
        yield return intro_texts[5].Write();

        while (!Input.GetKeyDown(KeyCode.Space) && !Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;
        }

        sound.FadeOutSource(music, 3f);

        while (curtain.color.a < 1f)
        {
            curtain.color += Color.black * Time.deltaTime * .5f;
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
