using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField]
    List<NamedEffect> effects = new List<NamedEffect>();

    Dictionary<string, List<Effect>> instances = new Dictionary<string, List<Effect>>();

    public Effect this[string n]{
        get{
            Effect inst = null;
            if (instances.ContainsKey(n))
            {
                foreach(Effect e in instances[n])
                {
                    if (!e.is_playing)
                    {
                        inst = e;
                        break;
                    }
                }
            }
            if(inst == null)
            {
                inst = Instantiate(effects.Find(delegate (NamedEffect ne) { return ne.first == n; }).second);
                inst.transform.SetParent(transform);
                if (!instances.ContainsKey(n))
                {
                    instances.Add(n, new List<Effect>());
                }
                if (inst.reusable)
                {
                    instances[n].Add(inst);
                }
                
            }
            

            return inst; 
        }
    }
}
[System.Serializable]
public class NamedEffect: Pair<string, Effect>
{
    public NamedEffect(string s, Effect e) : base(s, e) { }
}