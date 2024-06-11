using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRotateAbility : PlayerAbility
{
    public PhotonView PhotonView { get; private set; }

    public Transform CameraRoot;
    public float RotationSpeed = 200;

    private bool _isTowerClimbScene = false;

    private float _mx;
    private float _my;

    private void Start()
    {
        _isTowerClimbScene = SceneManager.GetActiveScene().name == "TowerClimbScene";
        
        if (_owner.PhotonView.IsMine && _isTowerClimbScene)
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineVirtualCamera>().Follow = CameraRoot;
            GameObject.FindWithTag("FollowCamera").GetComponent<CinemachineVirtualCamera>().LookAt = CameraRoot;
        }
    }

    private void Update()
    {
        if (!_owner.PhotonView.IsMine&& !_isTowerClimbScene)
        {
            return;
        }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _mx += mouseX * RotationSpeed * Time.deltaTime;
        _my += mouseY * RotationSpeed * Time.deltaTime;

        _my = Mathf.Clamp(_my, -135f, 135f);

        transform.eulerAngles = new Vector3(0, _mx, 0);
        CameraRoot.localEulerAngles = new Vector3(-_my, 0, 0);
    }
}
