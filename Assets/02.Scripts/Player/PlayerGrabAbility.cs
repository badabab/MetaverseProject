using Photon.Pun;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public Transform hand; // 캐릭터 손 위치
    private GameObject grabbedObject; // 잡힌 객체를 참조하기 위한 변수
    private SpringJoint springJoint; // 잡힌 객체에 적용할 SpringJoint를 참조하기 위한 변수
    public Animator animator; // 애니메이터 컴포넌트를 참조하기 위한 변수
    public float grabDistance = 2.0f; // 잡을 수 있는 최대 거리

    private bool Grabed = false; // 객체가 잡혔는지 여부를 나타내는 플래그
    private float GrabTime; // 잡은 시간을 기록하는 변수
    public float GrabbingTimer = 4f; // 잡는 동작의 최대 시간
    private float sphereRadius = 2f; // SphereCast의 반경

    void Start()
    {
        // Animator 컴포넌트를 초기화
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator component is missing from this GameObject.");
            }
        }
    }

    void Update()
    {
        if (!photonView.IsMine) // 로컬 플레이어인지 확인
            return;

        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭으로 잡기 시도
        {
            TryGrab(); // 잡기 시도 함수 호출
            animator.SetBool("Grab", true); // 애니메이션 파라미터 설정
        }

        if (Grabed) // 객체를 잡고 있는 동안
        {
            GrabTime += Time.deltaTime; // 잡은 시간 누적
            if (GrabTime >= GrabbingTimer) // 잡은 시간이 최대 시간을 넘으면
            {
                ReleaseGrab(); // 잡기 해제 함수 호출
                animator.SetBool("isGrabbing", false); // 애니메이션 파라미터 설정
                Grabed = false; // 플래그 해제
            }
        }

        if (grabbedObject == null && !Input.GetMouseButton(0)) // 객체를 잡고 있지 않고 마우스 버튼이 눌리지 않았을 때
        {
            animator.SetBool("Grab", false); // 애니메이션 파라미터 설정
            Grabed = false; // 플래그 해제
        }

        if (Input.GetMouseButtonUp(0) && grabbedObject != null) // 마우스 왼쪽 버튼을 떼고 객체를 잡고 있을 때
        {
            ReleaseGrab(); // 잡기 해제 함수 호출
            animator.SetBool("isGrabbing", false); // 애니메이션 파라미터 설정
            Grabed = false; // 플래그 해제
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(hand.position, hand.forward); // 손 위치에서 전방으로 레이 생성
        RaycastHit hit; // 레이캐스트 결과를 저장할 변수

        if (Physics.SphereCast(ray, sphereRadius, out hit, grabDistance)) // 특정 거리 내에 있는 오브젝트 감지
        {
            if (hit.collider.CompareTag("Grabbable")) // Grabbable 태그가 붙은 오브젝트만 잡기
            {
                photonView.RPC("RPC_TryGrab", RpcTarget.AllBuffered, hit.collider.gameObject.GetComponentInParent<PhotonView>().ViewID); // RPC 호출하여 모든 클라이언트에 잡기 동작 동기화
            }
        }
    }

    [PunRPC]
    void RPC_TryGrab(int viewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID); // 해당 ViewID의 PhotonView를 찾음
        if (targetPhotonView != null) // PhotonView가 존재하면
        {
            grabbedObject = targetPhotonView.gameObject; // 잡힌 객체를 설정
            animator.SetBool("isGrabbing", true); // 애니메이션 파라미터 설정
            Grabed = true; // 플래그 설정
            GrabTime = 0f; // 잡기 시작 시 타이머 초기화

            springJoint = grabbedObject.AddComponent<SpringJoint>(); // 잡힌 객체에 SpringJoint 컴포넌트 추가
            springJoint.connectedBody = GetComponent<Rigidbody>(); // 현재 객체의 Rigidbody에 연결

            // SpringJoint 설정
            springJoint.spring = 50f; // 스프링 강도
            springJoint.damper = 5f; // 감쇠
            springJoint.tolerance = 0.1f; // 관용도
            springJoint.minDistance = 0.5f; // 최소 거리
            springJoint.maxDistance = 1.5f; // 최대 거리

            Rigidbody grabbedRb = grabbedObject.GetComponentInParent<Rigidbody>(); // 잡힌 객체의 Rigidbody 가져오기
            grabbedRb.useGravity = false; // 중력 해제

            photonView.RPC("RPC_UpdateSpringJoint", RpcTarget.OthersBuffered, springJoint.spring, springJoint.damper, springJoint.tolerance, springJoint.minDistance, springJoint.maxDistance);
        }
    }

    [PunRPC]
    void RPC_UpdateSpringJoint(float spring, float damper, float tolerance, float minDistance, float maxDistance)
    {
        if (springJoint != null)
        {
            springJoint.spring = spring;
            springJoint.damper = damper;
            springJoint.tolerance = tolerance;
            springJoint.minDistance = minDistance;
            springJoint.maxDistance = maxDistance;
        }
    }

    void ReleaseGrab()
    {
        if (springJoint != null) // SpringJoint가 존재하면
        {
            Rigidbody grabbedRb = grabbedObject.GetComponentInParent<Rigidbody>(); // 잡힌 객체의 Rigidbody 가져오기
            grabbedRb.useGravity = true; // 중력 활성화
            Destroy(springJoint); // SpringJoint 파괴
            photonView.RPC("RPC_ReleaseGrab", RpcTarget.AllBuffered); // RPC 호출하여 모든 클라이언트에 잡기 해제 동작 동기화
        }
        grabbedObject = null; // grabbedObject 변수 초기화
    }

    [PunRPC]
    void RPC_ReleaseGrab()
    {
        if (grabbedObject != null) // 잡힌 객체가 존재하면
        {
            grabbedObject.transform.SetParent(null); // 부모 관계 해제
            grabbedObject = null; // grabbedObject 변수 초기화
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red; // 기즈모 색상을 빨간색으로 설정
        Gizmos.DrawWireSphere(hand.position, sphereRadius); // 손 위치에 구를 그림
        Gizmos.DrawRay(hand.position, hand.forward * grabDistance); // 손 위치에서 전방으로 레이를 그림
        Gizmos.DrawWireSphere(hand.position + hand.forward * grabDistance, sphereRadius); // 레이 끝 위치에 구를 그림
    }
}
