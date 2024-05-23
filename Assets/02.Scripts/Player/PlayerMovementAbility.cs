using Photon.Voice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using static UnityEditor.FilePathAttribute;

public class PlayerMovementAbility : MonoBehaviour
{
    public Transform Hand; // 캐릭터 손 위치
    private GameObject _grabbedObject;
    private CharacterController _characterController;
    public Animator Animator;
    public float GrabDistance = 2.0f;
    public float MoveSpeed = 5.0f;
    public float RotationSpeed = 720.0f; // 초당 회전 속도

    void Start()
    {
        _characterController = GetComponent<CharacterController>();

    }
    // Update is called once per frame
    void Update()
    {

       HandleMovement();
        HandleGrab();

    }
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Vector3.ClampMagnitude(direction, 1);

        if (direction.magnitude > 0)
        {
            Vector3 rotatedDirection = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * direction;
            _characterController.SimpleMove(rotatedDirection * MoveSpeed);

            Quaternion targetRotation = Quaternion.LookRotation(rotatedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, RotationSpeed * Time.deltaTime);

            Animator.SetBool("IsWalking", true);
        }
        else
        {
            Animator.SetBool("IsWalking", false);
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
            Animator.SetTrigger("Release");
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
                _grabbedObject.transform.SetParent(Hand);
                _grabbedObject.transform.localPosition = Vector3.zero;

                // 잡힌 객체의 Rigidbody를 비활성화
                Rigidbody grabbedRb = _grabbedObject.GetComponent<Rigidbody>();
                if (grabbedRb != null)
                {
                    grabbedRb.isKinematic = true;
                }
            }
        }
    }

    void ReleaseGrab()
    {
        if (_grabbedObject != null)
        {
            // 잡힌 객체의 Rigidbody를 다시 활성화
            Rigidbody grabbedRb = _grabbedObject.GetComponent<Rigidbody>();
            if (grabbedRb != null)
            {
                grabbedRb.isKinematic = false;
            }

            _grabbedObject.transform.SetParent(null);
            _grabbedObject = null;
        }
    }

}
