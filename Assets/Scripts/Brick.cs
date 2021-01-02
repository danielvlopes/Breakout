using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{

    [SerializeField] private int hits = 1;
    [SerializeField] private int points = 100;
    [SerializeField] private Vector3 rotator;
    [SerializeField] private Material hitMaterial;

    private Material origMaterial;
    private new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(rotator * (transform.position.x + transform.position.y) * 0.1f);
        renderer = GetComponent<Renderer>();
        origMaterial = renderer.sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotator * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        hits--;
        if (hits <= 0)
        {
            GameManager.Instance.Score += points;
            Destroy(gameObject);
        }

        renderer.sharedMaterial = hitMaterial;
        Invoke("RestoreMaterial", 0.05f);
    }

    private void RestoreMaterial()
    {
        renderer.sharedMaterial = origMaterial;
    }
}
