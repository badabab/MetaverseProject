using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class FallGuysPlayer : MonoBehaviourPunCallbacks
{
    private bool _isReady = false;
    private Vector3 _currentCheckpoint;

    private GameObject _testPosition;

    private void Awake()
    {
        _testPosition = GameObject.Find("End3");
    }
    private void Start()
    {
        if (!photonView.IsMine) return;
        if (SceneManager.GetActiveScene().name != "FallGuysScene")
        {
            this.enabled = false; // 씬이 "FallGuysScene"이 아니면 스크립트를 비활성화
            return;
        }

        _currentCheckpoint = _testPosition.transform.position;
        //_currentCheckpoint = new Vector3(500, 2, 80); // Start1 위치
        this.transform.position = _currentCheckpoint;
    }

    private void Update()
    {
        if (!photonView.IsMine) return;
        ReadyPlayer();
        if (FallGuysManager.Instance._currentGameState == GameState.Over) 
        {
            ShowLose();
        }
    }

    void ReadyPlayer()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
        {
            _isReady = !_isReady; // 레디 상태 토글
            Hashtable props = new Hashtable { { "IsReady_FallGuys", _isReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            Debug.Log("레디 버튼 누름: " + _isReady);
            FallGuysManager.Instance.SetPlayerReadyVFX(_isReady, transform.position);
        }
    }

    public void ShowLose()
    {
        string firstPlayerName = (string)PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerName"];
        if (firstPlayerName != photonView.Owner.NickName) 
        {
            Animator animator = GetComponent<Animator>();
            animator.SetBool("Sad", true);
            UI_GameOver.Instance.CheckLast();
        }
    }

    [PunRPC]
    public void UpdateCheckpoint(Vector3 checkpoint)
    {
        _currentCheckpoint = checkpoint;
    }
    [PunRPC]
    public void MovePlayer(Vector3 newPosition)
    {
        this.transform.position = newPosition;
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
            this.transform.position = _currentCheckpoint;
            ParticleSystem particle = FallGuysManager.Instance.WaterParticle;
            particle.transform.localScale *= 0.5f;
            Instantiate(particle, transform.position + Vector3.up, Quaternion.identity);
        }
    }
}
