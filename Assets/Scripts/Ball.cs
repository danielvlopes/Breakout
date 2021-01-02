using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private float speed = 28f;
    private new Rigidbody rigidbody;
    private Vector3 velocity;
    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        Invoke("Launch", 0.5f);
    }

    void Launch()
    {
        rigidbody.velocity = Vector3.down * speed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rigidbody.velocity = rigidbody.velocity.normalized * speed;
        velocity = rigidbody.velocity;

        if (!_renderer.isVisible)
        {
            GameManager.Instance.Balls--;
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        rigidbody.velocity = Vector3.Reflect(velocity, collision.contacts[0].normal);
    }

}
