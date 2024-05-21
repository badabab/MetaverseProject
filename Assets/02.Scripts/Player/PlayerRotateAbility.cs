using Cinemachine;
using Photon.Pun;
using UnityEngine;

public class PlayerRotateAbility : PlayerAbility
{
    public PhotonView PhotonView { get; private set; }

    public Transform CameraRoot;
    public float RotationSpeed = 200;

    private float _mx;
    private float _my;

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        if (_owner.PhotonView.IsMine)
        {
            GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = CameraRoot;
        }
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -90f, 90f);

        transform.eulerAngles = new Vector3(0, _mx, 0);
        CameraRoot.localEulerAngles = new Vector3(_my, 0, 0);
    }
}
