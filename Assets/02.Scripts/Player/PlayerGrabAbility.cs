using Photon.Pun;
using UnityEngine;

public class PlayerGrabAbility : PlayerAbility
{

    public Transform Hand; // 캐릭터 손 위치
    private GameObject _grabbedObject;
    private CharacterController _characterController;
    public Animator Animator;
    public float GrabDistance = 2.0f;


    void Start()
    {
        _characterController = GetComponent<CharacterController>();


    }
    // Update is called once per frame
    void Update()
    {
        if (_owner.PhotonView.IsMine)
        {
            HandleGrab();
        }
    }


    void HandleGrab()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭으로 잡기
        {
            TryGrab();
            Animator.SetTrigger("Grab");
        }
        else if (Input.GetMouseButtonUp(0) && _grabbedObject != null) // 마우스 왼쪽 버튼 떼기
        {
            ReleaseGrab();
        }
        if (Input.GetMouseButton(1)) 
        {
            Animator.SetTrigger("Combo");
        }
    }
    void TryGrab()
    {
        Ray ray = new Ray(Hand.position, Hand.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, GrabDistance)) // 특정 거리 내에 있는 오브젝트 감지
        {
            if (hit.collider.CompareTag("Grabbable")) // Grabbable 태그가 붙은 오브젝트만 잡기
            {
                _grabbedObject = hit.collider.gameObject;
                _owner.PhotonView.RPC("RPC_TryGrab", RpcTarget.AllBuffered, _grabbedObject.GetComponent<PhotonView>().ViewID);
            }
        }
    }
    [PunRPC]
    void RPC_TryGrab(int viewID)
    {
        GameObject grabbedObj = PhotonView.Find(viewID).gameObject;
        grabbedObj.transform.SetParent(Hand);
        grabbedObj.transform.localPosition = Vector3.zero;

        // 잡힌 객체의 Rigidbody를 비활성화
        Rigidbody grabbedRb = grabbedObj.GetComponent<Rigidbody>();
        if (grabbedRb != null)
        {
            grabbedRb.isKinematic = true;
        }

        _grabbedObject = grabbedObj;
    }

    void ReleaseGrab()
    {
        if (_grabbedObject != null)
        {
            _owner.PhotonView.RPC("RPC_ReleaseGrab", RpcTarget.AllBuffered, _grabbedObject.GetComponent<PhotonView>().ViewID);
            _grabbedObject = null;
        }
    }
    [PunRPC]
    void RPC_ReleaseGrab(int viewID)
    {
        GameObject grabbedObj = PhotonView.Find(viewID).gameObject;

        // 잡힌 객체의 Rigidbody를 다시 활성화
        Rigidbody grabbedRb = grabbedObj.GetComponent<Rigidbody>();
        if (grabbedRb != null)
        {
            grabbedRb.isKinematic = false;
        }

        grabbedObj.transform.SetParent(null);
    }
}
