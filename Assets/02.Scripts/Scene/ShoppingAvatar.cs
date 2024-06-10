using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingAvatar : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI PopUpText;
    public SphereCollider SphereCollider;
    public GameObject ChangeAvatarButton;
    public List<GameObject> Avatars; // 아바타 프리팹 리스트
    private GameObject currentAvatar;
    private GameObject player;
    private int Coin100 = 100;
    //private int Coin150 = 150;
    //private int Coin200 = 200;


    private void Start()
    {
        PopUpText.gameObject.SetActive(false);
        ChangeAvatarButton.SetActive(false);
      //  ChangeAvatarButton.GetComponent<Button>().onClick.AddListener(OnClickChanging);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            player = other.gameObject;
            PopUpText.gameObject.SetActive(true);
            ChangeAvatarButton.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            PopUpText.gameObject.SetActive(false);
            ChangeAvatarButton.SetActive(false);
            player = null;
        }
    }

    public void OnClickChanging()
    {
        int coins = PersonalManager.Instance.CheckCoins();
        if (player != null && coins > Coin100)
        {
            if (currentAvatar != null)
            {
                PhotonNetwork.Destroy(currentAvatar);
            }

            int randomIndex = Random.Range(0, Avatars.Count);
            Vector3 spawnPosition = player.transform.position;
            Quaternion spawnRotation = player.transform.rotation;

            currentAvatar = PhotonNetwork.Instantiate(Avatars[randomIndex].name, spawnPosition, spawnRotation, 0);
            currentAvatar.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            PersonalManager.Instance.UpdateCharacterIndex(randomIndex);
            PersonalManager.Instance.SpendCoins(Coin100);

            // 기존 플레이어 오브젝트를 제거
            PhotonNetwork.Destroy(player);

            // UI 숨김
            PopUpText.gameObject.SetActive(false);
            ChangeAvatarButton.SetActive(false);
        }
    }
    public void OnClickXButton()
    {
        ChangeAvatarButton.SetActive(false);
    }
}
