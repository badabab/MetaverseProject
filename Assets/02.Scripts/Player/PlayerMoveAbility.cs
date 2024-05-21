using UnityEngine;

public class PlayerMoveAbility : PlayerAbility
{
    public float MoveSpeed = 5f;
    private CharacterController _characterController;

    private float _gravity = -20f;
    private float _yVelocity = 0f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        _yVelocity += _gravity * Time.deltaTime;
        dir.y = _yVelocity;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);       
    }
}
