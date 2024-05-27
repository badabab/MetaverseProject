using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Male,
    Female,
}

public class UI_Lobby : MonoBehaviour
{
    public TMPro.TMP_InputField TMP_InputField;
    // public InputField NicknameInputFieldUI;
    public Button NextButtonUI;
    public Button FemaleButtonUI;
    public Button MaleButtonUI;
    public GameObject Metaverse1;
    public GameObject Metaverse2;

    public string RoomID = "testRoom";
    public static PlayerType SelectedType = PlayerType.Male;
    public GameObject Male;
    public GameObject Female;

    private void Start()
    {
        Male.SetActive(false);
        Female.SetActive(false);
    }

    public void OnClickNextButton()
    {
        string nickname = TMP_InputField.text;

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

    public void OnClickStartButton()
    {
        string nickname = TMP_InputField.text;
        if (string.IsNullOrEmpty(nickname))
        {
            Debug.Log("Please enter a nickname");
            return;
        }
        PhotonNetwork.NickName = nickname;

        // [ 룸 옵션 설정 ]
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 20;             // 입장 가능한 최대 플레이어 수
        roomOptions.IsVisible = true;            // 로비에서 방 목록에 노출할 것인가?
        roomOptions.IsOpen = true;               // 방이 열려있는 상태인가?
        roomOptions.EmptyRoomTtl = 1000 * 20;    // 비어있는 방 살아있는 시간(TimeToLive)

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable()  // 룸 커스텀 프로퍼티 (플레이어 커스텀 프로퍼티)
        {
            {"MasterNickname", nickname}
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "MasterNickname" };

        //SceneManager.LoadScene("LoadingScene");
        PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);   // 방이 있다면 입장하고 없다면 만드는 것
    }
    public void OnClickMaleButton() => OnClickPlayerTypeButton(PlayerType.Male);
    public void OnClickFemaleButton() => OnClickPlayerTypeButton(PlayerType.Female);

    private void OnClickPlayerTypeButton(PlayerType Ptype)
    {
        SelectedType = Ptype;
        Male.SetActive(SelectedType == PlayerType.Male);
        Female.SetActive(SelectedType == PlayerType.Female);
    }

    public void OnNicknameValueChanged(string newValue)
    {
        if (string.IsNullOrEmpty(newValue))
        {
            return;
        }
        PhotonNetwork.NickName = newValue;
    }
}
