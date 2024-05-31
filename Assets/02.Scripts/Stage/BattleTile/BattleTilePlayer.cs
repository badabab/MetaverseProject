using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTilePlayer : MonoBehaviourPunCallbacks
{
    public bool isReady = false;
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

        if (photonView.IsMine)
        {
            SetReadyStateOnInput();
        }
        MyNum = GetUniqueRandomNumber();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerNumber", MyNum } });
        GameObject startpoint = GameObject.Find($"Start{MyNum}");
        Teleport(startpoint.transform);
    }

    private int GetUniqueRandomNumber()
    {
        int randomNum;
        bool isUnique;
        do
        {
            randomNum = Random.Range(1, 5);
            isUnique = true;
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.CustomProperties.ContainsKey("PlayerNumber") && (int)player.CustomProperties["PlayerNumber"] == randomNum)
                {
                    isUnique = false;
                    break;
                }
            }
        } while (!isUnique);
        Debug.Log(randomNum);
        return randomNum;
    }

    private void Teleport(Transform startpoint)
    {
        _characterController.enabled = false;
        transform.position = startpoint.position;
        _characterController.enabled = true;
    }

    private void SetReadyStateOnInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isReady = !isReady;
            UpdateReadyState(isReady);
        }
    }

    private void UpdateReadyState(bool readyState)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady", readyState } });
    }
}
