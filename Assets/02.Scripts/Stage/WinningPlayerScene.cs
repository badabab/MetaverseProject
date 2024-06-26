using ExitGames.Client.Photon;
using MongoDB.Driver;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;

public class WinningPlayerScene : MonoBehaviour
{
    private string _firstPlayerName;

    public Transform PlayerSpawn;
    public TextMeshProUGUI WinningName;
    public GameObject winningPlayer;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("FirstPlayerName"))
        {
            _firstPlayerName = (string)PhotonNetwork.CurrentRoom.CustomProperties["FirstPlayerName"];

            // 첫 번째 플레이어만 생성
            if (PhotonNetwork.LocalPlayer.NickName == _firstPlayerName)
            {
                int characterIndex = PersonalManager.Instance.CheckCharacterIndex();
                string firstCharacter = $"Player {characterIndex}";

                if (SceneManager.GetActiveScene().name == "FallGuysWinScene")
                {
                    winningPlayer = PhotonNetwork.Instantiate(firstCharacter, PlayerSpawn.position, PlayerSpawn.rotation);
                    Debug.Log($"{winningPlayer.transform.rotation}");
                }

                else
                {
                    winningPlayer = PhotonNetwork.Instantiate(firstCharacter, PlayerSpawn.position, Quaternion.Euler(0, -180, 0));
                }
                WinningName.text = _firstPlayerName;
                Debug.Log($"{_firstPlayerName} 님이 이겼습니다.");
                Animator animator = winningPlayer.GetComponent<Animator>();
                animator.SetBool("Win", true);

                // 카메라 설정
                //SetupCameraForPlayer(winningPlayer);
            }
        }
        else
        {
            // 첫 번째 플레이어가 아닌 경우 카메라 설정
            //FindAndSetupCameraForFirstPlayer();
        }

        StartCoroutine(AllPlayerOut_Coroutine());
    }

    private void SetupCameraForPlayer(GameObject player)
    {
        Transform cameraRoot = player.transform.Find("CameraRoot");
        if (cameraRoot != null)
        {
            TPSCamera tpsCamera = FindObjectOfType<TPSCamera>();
            if (tpsCamera != null)
            {
                tpsCamera.target = cameraRoot;
                
            }
            else
            {
                Debug.LogError("TPSCamera not found in scene.");
            }
        }
        else
        {
            Debug.LogError("CameraRoot not found on player: " + player.name);
        }
    }

    private void FindAndSetupCameraForFirstPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PhotonView photonView = player.GetComponent<PhotonView>();
            if (photonView != null && photonView.Owner.NickName == _firstPlayerName)
            {
                SetupCameraForPlayer(player);
                break;
            }
        }
    }

    public IEnumerator AllPlayerOut_Coroutine()
    {
        yield return new WaitForSeconds(5);
        //PhotonNetwork.LoadLevel("VillageScene");
        //PhotonNetwork.LeaveRoom();
        PhotonManager.Instance.LeaveAndLoadRoom("Village");
        //PhotonNetwork.LeaveRoom();
    }
}
