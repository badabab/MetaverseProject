using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingAvatar : MonoBehaviourPunCallbacks
{
    public SphereCollider SphereCollider;
    public GameObject ChangeAvatarButton;
    public GameObject ChoicePopup;
    public List<GameObject> Avatars; // 아바타 프리팹 리스트
    private GameObject currentAvatar;
    private GameObject player;
    private int Coin100 = 100;
    private int Coin300 = 300;

    public GameObject ChangingName;
    public TMP_InputField InputFieldNameUI;
    private string _newName;

    public GameObject NoCoinAtAll;

    private void Start()
    {
        ChangeAvatarButton.SetActive(false);
        NoCoinAtAll.SetActive(false);
        ChangingName.SetActive(false);
        //  ChangeAvatarButton.GetComponent<Button>().onClick.AddListener(OnClickChanging);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            player = other.gameObject;
            ChangeAvatarButton.SetActive(true);
            ChoicePopup.SetActive(true);
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            ChangeAvatarButton.SetActive(false);
            ChoicePopup.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            player = null;
        }
    }

    public void OnClickChangingAvatars()
    {
        int coins = PersonalManager.Instance.CheckCoins();
        if (player != null && coins > Coin100)
        {
            if (currentAvatar != null)
            {
                PhotonNetwork.Destroy(currentAvatar);
            }

            int randomIndex = UnityEngine.Random.Range(0, Avatars.Count);
            Vector3 spawnPosition = player.transform.position;
            Quaternion spawnRotation = player.transform.rotation;

            currentAvatar = PhotonNetwork.Instantiate(Avatars[randomIndex].name, spawnPosition, spawnRotation, 0);
            currentAvatar.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            PersonalManager.Instance.UpdateCharacterIndex(randomIndex);
            PersonalManager.Instance.SpendCoins(Coin100);

            // 기존 플레이어 오브젝트를 제거
            PhotonNetwork.Destroy(player);

            ChangeAvatarButton.SetActive(false);
        }
        else
        {
            NoCoinAtAll.SetActive(true);
        }
    }

    public void OnClickNamePopup()
    {
        int coins = PersonalManager.Instance.CheckCoins();

        if (player != null & coins > Coin300)
        {
            bool isActive = ChangingName.activeSelf;

            // 새로운 활성 상태 설정
            ChangingName.SetActive(!isActive);
            ChoicePopup.SetActive(isActive);
        }
        else
        {
            NoCoinAtAll.SetActive(true);
            Debug.Log("돈 없음");
        }
    }
    public void OnClickGetNewNickName()
    {
        // InputFieldNameUI가 null이 아닌지 확인하고 값 설정
        if (InputFieldNameUI == null)
        {
            Debug.LogError("InputFieldNameUI가 null입니다.");
            return;
        }

        _newName = InputFieldNameUI.text;

        if (string.IsNullOrEmpty(_newName))
        {
            Debug.LogError("새 이름이 유효하지 않습니다.");
            return;
        }

        bool isChanged = PersonalManager.Instance.ChangingNickName(_newName);

        if (isChanged)
        {
            PersonalManager.Instance.SpendCoins(Coin300);
            //PlayerCanvasAbility.Instance.SetNickname(_newName);
            ChangingName.gameObject.SetActive(false);
            ChangeAvatarButton.gameObject.SetActive(false);
            Debug.Log("닉네임 변경 성공");
        }
        else
        {
            Debug.LogError("닉네임 변경 실패");
        }
    }
    public void OnClickXButton()
    {
        ChangeAvatarButton.SetActive(false);
    }
}
