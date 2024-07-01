using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class BattleTilePlayer : MonoBehaviourPunCallbacks
{
    public bool isReady = false;
    public int MyNum;
    private TileScore _tileScore;
    private bool _isFinished = false;
    private Particles _particlesController;
    private GameObject _startpoint;

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name != "BattleTileScene")
        {
            this.enabled = false;
            return;
        }
        if (!photonView.IsMine) return;
    }

    private void Start()
    {
        MyNum = GetUniqueRandomNumber();
        PhotonNetwork.LocalPlayer.SetCustomProperties
            (new Hashtable { { "PlayerNumber", MyNum }, { "PlayerTileNumber", MyNum } });
        _startpoint = GameObject.Find($"Start{MyNum}");
        MoveStartPosition();
        _particlesController = FindObjectOfType<Particles>();
    }

    public void MoveStartPosition()
    {     
        this.transform.position = _startpoint.transform.position;
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            SetReadyStateOnInput();
        }
        if (BattleTileManager.Instance.CurrentGameState == GameState.Loading)
        {
            MoveStartPosition();
        }
        if (BattleTileManager.Instance.CurrentGameState == GameState.Go
            || BattleTileManager.Instance.CurrentGameState == GameState.Over)
        {
            switch (MyNum)
            {
                case 1:
                    _particlesController.SetParticlePosition(1, transform.position);
                    break;
                case 2:
                    _particlesController.SetParticlePosition(2, transform.position);
                    break;
                case 3:
                    _particlesController.SetParticlePosition(3, transform.position);
                    break;
                case 4:
                    _particlesController.SetParticlePosition(4, transform.position);
                    break;
            }
        }
    }

    private int GetUniqueRandomNumber()
    {
        /*int randomNum;
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
        return randomNum;*/
        int num = PhotonNetwork.CurrentRoom.PlayerCount;
        Debug.Log($"playernum = {num}");
        return num;
    }

    private void SetReadyStateOnInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_RButton);
            isReady = !isReady;
            UpdateReadyState(isReady);
        }
    }

    private void UpdateReadyState(bool readyState)
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "IsReady_BattleTile", readyState } });
    }

    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        string firstPlayerName = (string)PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerName"];
        if (firstPlayerName != null)
        {
            if (BattleTileManager.Instance.CurrentGameState == GameState.Over)
            {
                if (!_isFinished)
                {
                    Animator animator = GetComponent<Animator>();
                    if (photonView.IsMine)
                    {
                        if (firstPlayerName == photonView.Owner.NickName)
                        {
                            UI_GameOver.Instance.CheckFirst();
                            animator.SetBool("Win", true);
                        }
                        else
                        {
                            UI_GameOver.Instance.CheckLast();
                            animator.SetBool("Sad", true);
                        }
                    }
                    _isFinished = true;
                }
            }
        }
        else
        {
            return;
        }
    }
}
