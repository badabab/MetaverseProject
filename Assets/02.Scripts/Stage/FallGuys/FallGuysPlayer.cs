using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FallGuysPlayer : MonoBehaviourPunCallbacks
{
    private bool _isReady = false;
    private Vector3 _currentCheckpoint;
    private CharacterController _characterController;

    private void Start()
    {
        if (!photonView.IsMine) return;
        _characterController = GetComponent<CharacterController>();
        if (SceneManager.GetActiveScene().name != "FallGuysScene")
        {
            this.enabled = false; // 씬이 "FallGuysScene"이 아니면 스크립트를 비활성화
            return;
        }

        _currentCheckpoint = new Vector3(500, 2, 80); // Start1 위치
        Teleport(_currentCheckpoint);
    }

    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log($"{photonView.name}: ready {_isReady}");
            SetReady(!_isReady);
        }
    }

    public void SetReady(bool ready)
    {
        _isReady = ready;
        Hashtable props = new Hashtable { { "IsReady", _isReady} };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    [PunRPC]
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }
    [PunRPC]
    public void MovePlayer(Vector3 newPosition)
    {
        Teleport(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        if (other.CompareTag("Checkpoint"))
        {
            Vector3 newCheckpoint = other.transform.position;
            photonView.RPC("UpdateCheckpoint", RpcTarget.All, newCheckpoint);
        }
        else if (other.gameObject.name == "Respawn")
        {
            Teleport(_currentCheckpoint);
        }
    }

    public void Teleport(Vector3 position)
    {
        _characterController.enabled = false;
        transform.position = position;
        _characterController.enabled = true;
    }
}
