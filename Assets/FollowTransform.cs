using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    public Transform to_follow;
    [SerializeField]
    public bool auto_offset = true;
    public Vector2 offset = Vector2.zero;
    [SerializeField]
    public float x_multiplier = 1f, y_multiplier = 1f;

    Vector2 to_follow_pos_mod
    {
        get
        {
            if(to_follow == null)
            {
                //return Vector2.zero;
            }
            Vector2 ret = to_follow.position;
            ret.x *= x_multiplier;
            ret.y *= y_multiplier;
            return ret;
        }
    }
    bool initialized = false;

    public virtual void Awake()
    {

        Initialize();
    }

    public virtual void Initialize()
    {
        if (initialized)
        {
            return;
        }
        initialized = true;
        
        offset = auto_offset ? transform.position.Vector2() - to_follow_pos_mod : offset;
    }

    protected void LateUpdate()
    {
        if (!initialized || to_follow == null)
        {

            return;
        }
        if (to_follow == null)
        {
            Destroy(gameObject);
            return;
        }
        transform.position = offset + to_follow_pos_mod;
    }

}
