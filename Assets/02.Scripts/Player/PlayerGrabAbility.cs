using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerGrabAbility : MonoBehaviourPunCallbacks
{
    public Transform hand;
    private GameObject grabbedObject;
    private ConfigurableJoint configurableJoint;
    public Animator animator;
    private PhotonAnimatorView photonAnimatorView; // PhotonAnimatorView 컴포넌트 참조 추가
    public float grabDistance = 2.0f;
    private bool Grabed = false;
    private float GrabTime;
    public float GrabbingTimer = 4f;
    private float sphereRadius = 2f;

    void Start()
    {
        animator = GetComponent<Animator>();
        photonAnimatorView = GetComponent<PhotonAnimatorView>(); // PhotonAnimatorView 컴포넌트 가져오기
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            TryGrab();
            animator.SetBool("Grab", true);
            photonAnimatorView.SetParameterSynchronized("Grab", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete); // 동기화 설정
        }

        if (Grabed)
        {
            GrabTime += Time.deltaTime;
            if (GrabTime >= GrabbingTimer)
            {
                ReleaseGrab();
                animator.SetBool("isGrabbing", false);
                Grabed = false;
            }
        }

        if (grabbedObject == null && !Input.GetMouseButton(0))
        {
            animator.SetBool("Grab", false);
            Grabed = false;
        }

        if (Input.GetMouseButtonUp(0) && grabbedObject != null)
        {
            ReleaseGrab();
            animator.SetBool("isGrabbing", false);
            Grabed = false;
        }
    }

    void TryGrab()
    {
        Ray ray = new Ray(hand.position, hand.forward);
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRadius, out hit, grabDistance))
        {
            if (hit.collider.CompareTag("Grabbable"))
            {
                photonView.RPC("RPC_TryGrab", RpcTarget.AllBuffered, hit.collider.gameObject.GetComponentInParent<PhotonView>().ViewID);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (hand != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(hand.position, sphereRadius);
            Gizmos.DrawRay(hand.position, hand.forward * grabDistance);
            Gizmos.DrawWireSphere(hand.position + hand.forward * grabDistance, sphereRadius);
        }
    }

    [PunRPC]
    void RPC_TryGrab(int viewID)
    {
        PhotonView targetPhotonView = PhotonView.Find(viewID);
        if (targetPhotonView != null)
        {
            grabbedObject = targetPhotonView.gameObject;
            animator.SetBool("isGrabbing", true);
            photonAnimatorView.SetParameterSynchronized("isGrabbing", PhotonAnimatorView.ParameterType.Bool, PhotonAnimatorView.SynchronizeType.Discrete); // 동기화 설정
            Grabed = true;
            GrabTime = 0f;

            configurableJoint = grabbedObject.AddComponent<ConfigurableJoint>();
            configurableJoint.connectedBody = null;

            configurableJoint.xMotion = ConfigurableJointMotion.Locked;
            configurableJoint.yMotion = ConfigurableJointMotion.Locked;
            configurableJoint.zMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularXMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularYMotion = ConfigurableJointMotion.Locked;
            configurableJoint.angularZMotion = ConfigurableJointMotion.Locked;

            configurableJoint.anchor = Vector3.zero;
            configurableJoint.autoConfigureConnectedAnchor = false;
            configurableJoint.connectedAnchor = hand.position;

            configurableJoint.breakForce = 2000f;
            configurableJoint.breakTorque = 2000f;

            Rigidbody grabbedRb = grabbedObject.GetComponentInParent<Rigidbody>();
            grabbedRb.useGravity = false;
            StartCoroutine(MoveObjectToHand(grabbedRb));
        }
    }

    void ReleaseGrab()
    {
        if (configurableJoint != null)
        {
            Rigidbody grabbedRb = grabbedObject.GetComponentInParent<Rigidbody>();
            grabbedRb.useGravity = true;
            Destroy(configurableJoint);
            photonView.RPC("RPC_ReleaseGrab", RpcTarget.AllBuffered);
        }
        grabbedObject = null;
    }

    [PunRPC]
    void RPC_ReleaseGrab()
    {
        if (grabbedObject != null)
        {
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }

    IEnumerator MoveObjectToHand(Rigidbody grabbedRb)
    {
        while (grabbedObject != null)
        {
            Vector3 direction = (hand.position - grabbedObject.transform.position).normalized;
            grabbedRb.velocity = direction * 10f;
            yield return null;
        }
    }
}
