using Cinemachine;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerRotateAbility : PlayerAbility
{
    public PhotonView PhotonView { get; private set; }

    public Transform CameraRoot;
    public float RotationSpeed = 2;

    private bool _isTowerClimbScene = false;

    private float _mx;
    private float _my;

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // 씬이 로드될 때 호출될 메서드 등록
        CheckSceneAndEnable();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 씬 로드 이벤트 해제
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndEnable();
    }

    private void CheckSceneAndEnable()
    {
        _isTowerClimbScene = SceneManager.GetActiveScene().name == "TowerClimbScene";

        if (!_isTowerClimbScene)
        {
            this.enabled = false; // TowerClimbScene이 아닌 경우 스크립트를 비활성화
            return;
        }
        else
        {
            this.enabled = true; // TowerClimbScene인 경우 스크립트를 활성화
        }

        if (_owner.PhotonView.IsMine && _isTowerClimbScene)
        {
            Cursor.lockState = CursorLockMode.Locked;
            GameObject followCamera = GameObject.FindWithTag("FollowCamera");
            if (followCamera != null)
            {
                CinemachineVirtualCamera cinemachineVirtualCamera = followCamera.GetComponent<CinemachineVirtualCamera>();
                if (cinemachineVirtualCamera != null)
                {
                    cinemachineVirtualCamera.Follow = CameraRoot;
                    //cinemachineVirtualCamera.LookAt = CameraRoot;
                }
                else
                {
                    Debug.LogError("CinemachineVirtualCamera component not found on FollowCamera.");
                }
            }
            else
            {
                Debug.LogError("FollowCamera not found.");
            }
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

        _my = Mathf.Clamp(_my, -50f, 25f);

        transform.eulerAngles = new Vector3(0, _mx, 0);
        CameraRoot.localEulerAngles = new Vector3(-_my, 0, 0);

        // Calculate the Y position based on the X rotation
        float yPos = Mathf.Lerp(0f, 1.65f, Mathf.InverseLerp(-50f, 25f, -_my));
        // Calculate the Z position based on the X rotation from 0 to 50
        float zPos = Mathf.Lerp(0f, 2f, Mathf.InverseLerp(0f, 50f, -_my));

        Vector3 cameraRootPosition = CameraRoot.localPosition;
        cameraRootPosition.y = yPos;
        cameraRootPosition.z = zPos;
        CameraRoot.localPosition = cameraRootPosition;
    }
}
