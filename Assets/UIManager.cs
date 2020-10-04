using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    Image round_indicator_prefab;
    [SerializeField]
    Transform queue_indicator;
    List<Image> round_indicators = new List<Image>();

    int _max_rounds;
    public int max_rounds
    {
        get
        {
            return _max_rounds;
        }
        set
        {
            if(_max_rounds != value)
            {
                SetRoundsNumber(value);
                _max_rounds = value;
            }
        }
    }
    int _current_round;
    public int current_round
    {
        get
        {
            return _current_round;
        }
        set
        {
            _current_round = value;
            if (current_round >= round_indicators.Count)
            {
                return;
            }
            for(int i=0; i < round_indicators.Count; i++)
            {
                round_indicators[i].rectTransform.localScale = Vector3.one * .8f;
                round_indicators[i].color = new Color(1f, 1f, 1f, .5f);
            }
            round_indicators[value].rectTransform.localScale = Vector3.one * 1.1f;
            round_indicators[value].color = Color.white;

        }
    }

    [SerializeField]
    float margin = 40f;
    void SetRoundsNumber(int num)
    {
        
        foreach(Image i in round_indicators)
        {
            Destroy(i.gameObject);
        }
        round_indicators.Clear();
        float offset = (margin * num) * -.5f;

        for(int i = 0; i<num; i++)
        {
            Image ni = Instantiate(round_indicator_prefab);
            ni.transform.SetParent(queue_indicator);
            RectTransform rt = ni.GetComponent<RectTransform>();
            Vector2 ap = rt.anchoredPosition;
            ap.x = offset + margin * i;
            ap.y = 0f;
            rt.anchoredPosition = ap;
            round_indicators.Add(ni);
        }
    }

}
