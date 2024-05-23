using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviour
{
    public Transform hand; // 캐릭터 손 위치
    private GameObject grabbedObject;
    private FixedJoint fixedJoint;
    public Animator Animator;
    private ConfigurableJoint configurableJoint;
    private float grabDistance = 2.0f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭으로 잡기
        {
            TryGrab();
            Animator.SetTrigger("Grab");
        }
        else if (Input.GetMouseButtonUp(0) && grabbedObject != null) // 마우스 왼쪽 버튼 떼기
        {
            ReleaseGrab();
            Animator.SetTrigger("Release");
        }
        if (Input.GetMouseButton(1)) 
        {
            Animator.SetTrigger("Combo");
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, grabDistance)) // 특정 거리 내에 있는 오브젝트 감지
        {
            if (hit.collider.CompareTag("Grabbable")) // Grabbable 태그가 붙은 오브젝트만 잡기
            {
                grabbedObject = hit.collider.gameObject;

                configurableJoint = grabbedObject.AddComponent<ConfigurableJoint>();
                configurableJoint.connectedBody = null;

                // ConfigurableJoint 설정
                configurableJoint.xMotion = ConfigurableJointMotion.Locked;
                configurableJoint.yMotion = ConfigurableJointMotion.Locked;
                configurableJoint.zMotion = ConfigurableJointMotion.Locked;
                configurableJoint.angularXMotion = ConfigurableJointMotion.Free;
                configurableJoint.angularYMotion = ConfigurableJointMotion.Free;
                configurableJoint.angularZMotion = ConfigurableJointMotion.Free;

                // Anchor와 연결점 설정
                configurableJoint.anchor = Vector3.zero;
                configurableJoint.autoConfigureConnectedAnchor = false;
                configurableJoint.connectedAnchor = hand.position;

                // Break force와 torque 설정
                configurableJoint.breakForce = 2000f;
                configurableJoint.breakTorque = 2000f;

                // 객체의 중력을 끄고 손 위치로 이동시키기
                Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
                grabbedRb.useGravity = false;
                StartCoroutine(MoveObjectToHand(grabbedRb));
            }
        }
    }
    void ReleaseGrab()
    {
        if (configurableJoint != null)
        {
            Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>();
            grabbedRb.useGravity = true;
            Destroy(configurableJoint);
        }
        grabbedObject = null;
    }
    IEnumerator MoveObjectToHand(Rigidbody grabbedRb)
    {
        while (grabbedObject != null)
        {
            Vector3 direction = (hand.position - grabbedObject.transform.position).normalized;
            grabbedRb.velocity = direction * 10f; // 객체를 손으로 이동시키는 속도
            yield return null;
        }
    }
}
