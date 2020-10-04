using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Effect : MonoBehaviour
{
    public abstract bool is_playing { get; }
    [SerializeField]
    public bool reusable;
    public virtual void Play(Vector2 position)
    {
        transform.position = position;
    }

    public virtual void Stop() { }
}
