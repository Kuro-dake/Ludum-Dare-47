using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    Transform target;
    bool done = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    Rigidbody2D rb2d { get { return GetComponent<Rigidbody2D>(); } }
    [SerializeField]
    float gravity_force = 200f, initial_force = 3f, distance_modifier = 5f, max_force = 15f, close_velocity_change_speed = 10f;

    [SerializeField]
    float gravity_time_modifier = 2f;
    // Update is called once per frame

    float velocity_magnitude = 0f;
    float lifetime = 0f;
    void Update()
    {
        if (done)
        {
            return;
        }
        lifetime += Time.deltaTime;
        rb2d.AddForce((target.position - transform.position).normalized * Time.deltaTime * gravity_time_modifier * lifetime);
        /*float distance = Vector2.Distance(transform.position, target.position);
        if (distance > 5f)
        {
            rb2d.AddForce((target.position - transform.position).normalized * Time.deltaTime * gravity_force * Mathf.Clamp(max_force - distance, 1f, distance_modifier));
            velocity_magnitude = rb2d.velocity.magnitude;
        }
        else
        {
            
            rb2d.velocity = Vector2.MoveTowards(rb2d.velocity, (target.position - transform.position).normalized * velocity_magnitude, Time.deltaTime * close_velocity_change_speed);
        }*/
        Collider2D c2d = Physics2D.OverlapCircle(transform.position, .2f, GM.controls.enemy_mask);
        if (c2d != null && c2d.transform == target)
        {
            Destroy(rb2d);
            done = true;
        }
        
    }

    public void Initialize(Transform _target)
    {
        rb2d.velocity = Random.insideUnitCircle.normalized * initial_force;
        target = _target;
        //StartCoroutine(AutoDestruct());
    }

    IEnumerator AutoDestruct()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }


}
