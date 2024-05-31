using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTilePlayer : MonoBehaviourPunCallbacks
{
    private CharacterController _characterController;
    public int MyNum;
    private void Start()
    {
        if (!photonView.IsMine) return;
        _characterController = GetComponent<CharacterController>();
        if (SceneManager.GetActiveScene().name != "BattleTileScene")
        {
            this.enabled = false;
            return;
        }

        MyNum = BattleTileManager.Instance.PlayerNumber;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerNumber", MyNum } });
        GameObject startpoint = GameObject.Find($"Start{MyNum}");
        Teleport(startpoint.transform);
    }

    private void Teleport(Transform startpoint)
    {
        _characterController.enabled = false;
        transform.position = startpoint.position;
        _characterController.enabled = true;
    }
}
