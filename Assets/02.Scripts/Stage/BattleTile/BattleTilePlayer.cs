using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTilePlayer : MonoBehaviourPunCallbacks
{
    private CharacterController _characterController;

    private void Start()
    {
        if (!photonView.IsMine) return;
        _characterController = GetComponent<CharacterController>();
        if (SceneManager.GetActiveScene().name != "BattleTileScene")
        {
            this.enabled = false;
            return;
        }

        int uniqueNumber = GetUniqueRandomNumber();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerNumber", uniqueNumber } });
        GameObject startpoint = GameObject.Find($"Start{uniqueNumber}");
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
}
