using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public GameObject ruby;
    RubyController player;
    public AudioClip projectileClip;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        ruby = GameObject.FindWithTag("Player");
        player = ruby.GetComponent<RubyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.magnitude > 50.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Prohectile Collision with " + other.gameObject);
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Fix();
        }
        Destroy(gameObject);
    }

    public void Launch (Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
        player.PlaySound(projectileClip);
    }
}
