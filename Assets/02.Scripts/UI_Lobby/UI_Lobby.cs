using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class UI_Lobby : MonoBehaviour
{
    public InputField NicknameInputFieldUI;
    public Button NextButtonUI;
    public Button FemaleButtonUI;
    public Button MaleButtonUI;
    public GameObject Metaverse1;
    public GameObject Metaverse2;

    private string selectedGender;
    private const int MaxPlayers = 15;  // 최대 플레이어 수

    void Start()
    {
        NextButtonUI.onClick.AddListener(OnClickNextButton);
        FemaleButtonUI.onClick.AddListener(OnClickFemaleButton);
        MaleButtonUI.onClick.AddListener(OnClickMaleButton);
    }

    public void OnClickNextButton()
    {
        string nickname = NicknameInputFieldUI.text;

        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("Please enter a nickname");
            return;
        }

        PhotonNetwork.NickName = nickname;

        // Metaverse1 비활성화, Metaverse2 활성화
        Metaverse1.SetActive(false);
        Metaverse2.SetActive(true);
    }

    public void OnClickMaleButton()
    {
        selectedGender = "Male";
        SaveSelectionAndLoadScene();
    }

    public void OnClickFemaleButton()
    {
        selectedGender = "Female";
        SaveSelectionAndLoadScene();
    }

    private void SaveSelectionAndLoadScene()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= MaxPlayers)
        {
            // 선택한 성별 정보를 저장 (예: 포톤 커스텀 속성으로 저장)
            Hashtable customProperties = new Hashtable();
            customProperties.Add("Gender", selectedGender);
            PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);

            // 로딩 씬으로 이동
            SceneManager.LoadScene("LoadingScene");
        }
        else
        {
            Debug.Log("The room is full. Cannot join more players.");
            // 추가적으로 UI를 통해 사용자에게 방이 가득 찼음을 알리는 방법을 구현할 수 있습니다.
        }
    }
}
