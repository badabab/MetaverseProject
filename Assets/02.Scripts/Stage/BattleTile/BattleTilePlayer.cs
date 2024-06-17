using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleTilePlayer : MonoBehaviourPunCallbacks
{
    public bool isReady = false;
    private CharacterController _characterController;
    public int MyNum;
    private TileScore _tileScore;

    private void Awake()
    {
        if (!photonView.IsMine) return;
        if (SceneManager.GetActiveScene().name != "BattleTileScene")
        {
            this.enabled = false;
            return;
        }
    }

    private void Start()
    {       
        _characterController = GetComponent<CharacterController>();
        MyNum = GetUniqueRandomNumber();        
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "PlayerNumber", MyNum }, { "PlayerTileNumber", MyNum } });
        GameObject startpoint = GameObject.Find($"Start{MyNum}");
        this.transform.position = startpoint.transform.position;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            SetReadyStateOnInput();
        }
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
        return randomNum;
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
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady_BattleTile", readyState } });
    }
}
