using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerType
{
    Male,
    Female,
}

public class UI_Lobby : MonoBehaviour
{
    public TMP_InputField TMP_InputFieldId;
    public TMP_InputField TMP_InputFieldPw;
    public Toggle RememberToggle;
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
        AutoLogin();

        string loggedInUser = PlayerPrefs.GetString("LoggedInUser", null);
        string loggedInPassword = PlayerPrefs.GetString("LoggedInPassword", null);
        if (!string.IsNullOrEmpty(loggedInUser))
        {
            TMP_InputFieldId.text = $"{loggedInUser}";
        }
        else if (!string.IsNullOrEmpty(loggedInPassword)) 
        {
            TMP_InputFieldPw.text = $"{loggedInPassword}";
        }
    }

    public void OnClickNextButton()
    {
        string nickname = TMP_InputField.text;
        string password = TMP_InputFieldPw.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(password))
        {
            Debug.Log("아이디, 비밀번호 둘 다 입력해주세요");
            return;
        }


        RememberID rememberID = RememberToggle.isOn ? RememberID.Remember : RememberID.Nope;


        if (rememberID == RememberID.Remember)
        {
            if (!PersonalManager.Instance.CheckUser(nickname, password))
            {
                PersonalManager.Instance.JoinList(nickname, password);
                Debug.Log("New user registered.");
            }

            var user = PersonalManager.Instance.Login(nickname, password);
            if (user != null)
            {
                PlayerPrefs.SetString("LoggedInUser", nickname);
                PlayerPrefs.SetString("LoggedInPassword", password);
                Debug.Log("Login successful, user remembered.");
                PhotonNetwork.NickName = nickname;
            }
        }

        else if (rememberID == RememberID.Nope)
        {
            var user = PersonalManager.Instance.Login(nickname, password);
            if (user != null)
            {
                Debug.Log("Login successful.");
                PhotonNetwork.NickName = nickname;
 
            }
            else
            {
                Debug.Log("Login failed.");
            }
        }

        PhotonNetwork.NickName = nickname;

        // Metaverse1 비활성화, Metaverse2 활성화
        Metaverse1.SetActive(false);
        Metaverse2.SetActive(true);
    }
    private void AutoLogin()
    {
        string loggedInUser = PlayerPrefs.GetString("LoggedInUser", null);
        string loggedInPassword = PlayerPrefs.GetString("LoggedInPassword", null);

        if (!string.IsNullOrEmpty(loggedInUser) || !string.IsNullOrEmpty(loggedInPassword))
        {
            TMP_InputFieldId.text = loggedInUser; 
            TMP_InputFieldPw.text = loggedInPassword;
            var user = PersonalManager.Instance.Login(loggedInUser, loggedInPassword);
            if (user != null) 
            {
                PhotonNetwork.NickName = loggedInUser;
            }
        }
    }

    public void OnClickStartButton()
    {
        // Metaverse1 비활성화, Metaverse2 활성화
        Metaverse1.SetActive(false);
        Metaverse2.SetActive(true);

        // Photon 룸에 입장
        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true,
            EmptyRoomTtl = 1000 * 20,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable { { "MasterNickname", PhotonNetwork.NickName } },
            CustomRoomPropertiesForLobby = new string[] { "MasterNickname" }
        };

        PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
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
