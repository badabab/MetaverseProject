using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody rb;
    public float windForce = 0.5f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Rigidbody 설정
        rb.mass = 0.1f;
        rb.drag = 0.5f;
        rb.angularDrag = 0.5f;
        rb.useGravity = true;

        PhysicMaterial bouncyMaterial = new PhysicMaterial();
        bouncyMaterial.bounciness = 1;
        bouncyMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.material = bouncyMaterial;
        }

        rb.AddForce(Vector3.up * windForce, ForceMode.Impulse);
    }

    void Update()
    {
        Vector3 randomWind = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * windForce;

        rb.AddForce(randomWind * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 currentVelocity = rb.velocity;
            Vector3 normal = collision.contacts[0].normal;

            Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, normal);

            rb.velocity = reflectedVelocity;
        }
    }
}
