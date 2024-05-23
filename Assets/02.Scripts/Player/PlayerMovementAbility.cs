using Photon.Voice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class PlayerMovementAbility : MonoBehaviour
{
    public float MoveSpeed = 5f; // 캐릭터의 이동 속도
    public float TurnSpeed = 360f; // 캐릭터의 회전 속도
    public LayerMask GroundLayer;
    private Rigidbody _rb;
    private Animator _animator;
    private Vector3 _movement;
    private Quaternion _rotation = Quaternion.identity;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _rb.useGravity = true;

    }
    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        _movement.Set(h, 0f, v);
        _movement.Normalize();

        bool isWalking = h != 0f || v != 0f;
        _animator.SetBool("IsWalking", isWalking);

        {
            Quaternion targetRotation = Quaternion.LookRotation(_movement);
            _rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, TurnSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        // 캐릭터 이동
        Vector3 desiredMove = _movement * MoveSpeed * Time.deltaTime;
        _rb.MovePosition(_rb.position + desiredMove);

        // 캐릭터 회전
        _rb.MoveRotation(_rotation);

        if (!IsGrounded())
        {
            _rb.AddForce(Physics.gravity, ForceMode.Acceleration);
        }
    }
    // 캐릭터가 지면에 있는지 확인하는 함수
    bool IsGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.1f, GroundLayer))
        {
            return true;
        }
        return false;
    }
}
