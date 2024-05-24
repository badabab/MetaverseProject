using Photon.Pun;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public Transform Hand; // 캐릭터 손 위치
    private GameObject _grabbedObject;
    private CharacterController _characterController;
    public Animator Animator;
    public float GrabDistance = 2.0f;
    public LayerMask GrabbableLayer; // Grabbable 오브젝트가 속한 레이어

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (photonView.IsMine)
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
        Debug.Log("Hand position: " + Hand.position);
        Collider[] hitColliders = Physics.OverlapSphere(Hand.position, GrabDistance, GrabbableLayer);
        Debug.Log("Number of colliders found: " + hitColliders.Length);

        if (hitColliders.Length > 0)
        {
            foreach (var hitCollider in hitColliders)
            {
                Debug.Log("Hit collider: " + hitCollider.name);
                if (hitCollider.CompareTag("Grabbable")) // Grabbable 태그가 붙은 오브젝트만 잡기
                {
                    _grabbedObject = hitCollider.gameObject;
                    PhotonView objectPhotonView = _grabbedObject.GetComponent<PhotonView>();
                    if (_grabbedObject == null)
                    {
                        Debug.LogError("Grabbed object is null.");
                    }
                    else if (objectPhotonView == null)
                    {
                        Debug.LogError("Grabbed object does not have a PhotonView component.");
                    }
                    else
                    {
                        photonView.RPC("RPC_TryGrab", RpcTarget.AllBuffered, objectPhotonView.ViewID);
                        Animator.SetTrigger("Grab");
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.Log("No grabbable objects found within range.");
        }
    }


    [PunRPC]
    void RPC_TryGrab(int viewID)
    {
        PhotonView grabbedPhotonView = PhotonView.Find(viewID);
        if (grabbedPhotonView == null)
        {
            Debug.LogError("PhotonView with viewID " + viewID + " not found.");
            return;
        }

        GameObject grabbedObj = grabbedPhotonView.gameObject;
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
            photonView.RPC("RPC_ReleaseGrab", RpcTarget.AllBuffered, _grabbedObject.GetComponent<PhotonView>().ViewID);
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
