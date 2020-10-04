using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayParticleSystemEffect : Effect
{
    public override bool is_playing { get
        {
            return ps.isPlaying;
        } 
    }
    ParticleSystem ps { get { return GetComponent<ParticleSystem>(); } }

    public override void Play(Vector2 position)
    {
        base.Play(position);
        ps.Play();
    }

    public override void Stop()
    {
        base.Stop();
        ps.Stop();
    }
}
