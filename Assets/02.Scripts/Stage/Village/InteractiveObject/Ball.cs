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
        rb.useGravity = true; // 필요에 따라 설정

        // 물리 재질을 스크립트에서 설정할 필요는 없지만, 그래도 설정 방법을 보여드리겠습니다.
        PhysicMaterial bouncyMaterial = new PhysicMaterial();
        bouncyMaterial.bounciness = 1;
        bouncyMaterial.bounceCombine = PhysicMaterialCombine.Maximum;

        // SphereCollider에 물리 재질 적용
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.material = bouncyMaterial;
        }

        // 초기 힘을 가하여 공이 움직이기 시작합니다.
        rb.AddForce(Vector3.up * windForce, ForceMode.Impulse);
    }

    void Update()
    {
        // 바람 효과를 주기 위해 임의의 힘을 추가합니다.
        Vector3 randomWind = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ) * windForce;

        rb.AddForce(randomWind * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 충돌한 오브젝트가 Player 태그를 가지고 있는지 확인
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌한 반대 방향으로 이동 방향을 바꿉니다.
            Vector3 currentVelocity = rb.velocity;
            Vector3 normal = collision.contacts[0].normal;

            // 반사 벡터 계산
            Vector3 reflectedVelocity = Vector3.Reflect(currentVelocity, normal);

            // 속도를 반사된 벡터로 설정
            rb.velocity = reflectedVelocity;
        }
    }
}
