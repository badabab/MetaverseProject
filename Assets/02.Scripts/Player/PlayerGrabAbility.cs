using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEditor;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public Transform hand; // 캐릭터 손 위치
    private GameObject grabbedObject; // 잡힌 객체를 참조하기 위한 변수
    private ConfigurableJoint configurableJoint; // 잡힌 객체에 적용할 ConfigurableJoint를 참조하기 위한 변수
    public Animator animator; // 애니메이터 컴포넌트를 참조하기 위한 변수
    public float grabDistance = 2.0f; // 잡을 수 있는 최대 거리

    private bool Grabed = false;
    private float GrabTime;
    public float GrabbingTimer = 4f;

    void Update()
    {
        if (!photonView.IsMine) // 이 클라이언트의 로컬 플레이어인지 확인
            return;

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭으로 잡기
        {
            TryGrab(); // 잡기 시도
            animator.SetBool("Grab", true); // Grab 애니메이션 실행
        }

        if (Grabed)
        {
            GrabTime += Time.deltaTime;
            if (GrabTime >= GrabbingTimer)
            {
                ReleaseGrab(); // 잡기 해제
                animator.SetBool("isGrabbing", false); // Release 애니메이션 실행
                Grabed = false;
            }
        }

        if (grabbedObject == null && !Input.GetMouseButton(0))
        {
            animator.SetBool("Grab", false);
            Grabed = false;
        }

        if (Input.GetMouseButtonUp(0) && grabbedObject != null) // 마우스 왼쪽 버튼 떼기 및 객체가 잡힌 상태
        {
            ReleaseGrab(); // 잡기 해제
            animator.SetBool("isGrabbing", false); // Release 애니메이션 실행
            Grabed = false;
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(hand.position, hand.forward); // 손 위치에서 전방으로 레이 생성
        RaycastHit hit; // 레이캐스트 결과를 저장할 변수

        if (Physics.Raycast(ray, out hit, grabDistance)) // 특정 거리 내에 있는 오브젝트 감지
        {
            if (hit.collider.CompareTag("Grabbable")) // Grabbable 태그가 붙은 오브젝트만 잡기
            {
                photonView.RPC("RPC_TryGrab", RpcTarget.AllBuffered, hit.collider.gameObject.GetComponent<PhotonView>().ViewID);
            }
            
        }
    }
    // 기즈모를 그리는 메서드
    private void OnDrawGizmos()
    {
        if (hand != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(hand.position, hand.forward * grabDistance);
        }
    }

    [PunRPC]
    void RPC_TryGrab(int viewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            grabbedObject = targetPhotonView.gameObject; // 잡힌 객체를 grabbedObject에 저장
            animator.SetBool("isGrabbing", true);
            Grabed = true;
            GrabTime = 0f; // 잡기 시작 시 타이머 초기화

            configurableJoint = grabbedObject.AddComponent<ConfigurableJoint>(); // 잡힌 객체에 ConfigurableJoint 컴포넌트 추가
            configurableJoint.connectedBody = null; // 연결된 바디 없음

            // ConfigurableJoint 설정
            configurableJoint.xMotion = ConfigurableJointMotion.Locked; // X축 이동 잠금
            configurableJoint.yMotion = ConfigurableJointMotion.Locked; // Y축 이동 잠금
            configurableJoint.zMotion = ConfigurableJointMotion.Locked; // Z축 이동 잠금
            configurableJoint.angularXMotion = ConfigurableJointMotion.Free; // X축 회전 자유
            configurableJoint.angularYMotion = ConfigurableJointMotion.Free; // Y축 회전 자유
            configurableJoint.angularZMotion = ConfigurableJointMotion.Free; // Z축 회전 자유

            // Anchor와 연결점 설정
            configurableJoint.anchor = Vector3.zero; // 앵커를 객체의 중심에 설정
            configurableJoint.autoConfigureConnectedAnchor = false; // 연결된 앵커 자동 설정 해제
            configurableJoint.connectedAnchor = hand.position; // 연결된 앵커를 손 위치로 설정

            // Break force와 torque 설정
            configurableJoint.breakForce = 2000f; // 최대 파괴 힘 설정
            configurableJoint.breakTorque = 2000f; // 최대 파괴 토크 설정

            // 객체의 중력을 끄고 손 위치로 이동시키기
            Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>(); // 잡힌 객체의 Rigidbody 가져오기
            grabbedRb.useGravity = false; // 중력 해제
            StartCoroutine(MoveObjectToHand(grabbedRb)); // 객체를 손으로 이동시키는 코루틴 시작
        }
    }

    void ReleaseGrab()
    {
        if (configurableJoint != null) // configurableJoint가 존재하는 경우
        {
            Rigidbody grabbedRb = grabbedObject.GetComponent<Rigidbody>(); // 잡힌 객체의 Rigidbody 가져오기
            grabbedRb.useGravity = true; // 중력 활성화
            Destroy(configurableJoint); // ConfigurableJoint 파괴
            photonView.RPC("RPC_ReleaseGrab", RpcTarget.AllBuffered);
        }
        grabbedObject = null; // grabbedObject 변수 초기화
    }

    [PunRPC]
    void RPC_ReleaseGrab()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null); // 부모 관계 해제
            grabbedObject = null; // grabbedObject 변수 초기화
        }
    }

    IEnumerator MoveObjectToHand(Rigidbody grabbedRb)
    {
        while (grabbedObject != null) // 잡힌 객체가 존재하는 동안
        {
            Vector3 direction = (hand.position - grabbedObject.transform.position).normalized; // 손 위치와 객체 위치 사이의 방향 벡터 계산
            grabbedRb.velocity = direction * 10f; // 객체를 손으로 이동시키는 속도 설정
            yield return null; // 다음 프레임까지 대기
        }
    }
}
