using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    public float MoveSpeed = 5f;
    private CharacterController _characterController;
    private Animator _animator;

    private float _gravity = -9.8f;
    private float _yVelocity = 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        /*if (!_owner.PhotonView.IsMine)
        {
            return;
        }*/

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        Vector3 unNormalizedDir = dir;
        dir.Normalize();
        dir = Camera.main.transform.TransformDirection(dir);

        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _animator.SetFloat("Move", unNormalizedDir.magnitude);
    }
}
