using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public TMP_InputField TMP_InputField;
    public Button NextButtonUI;
    public Button FemaleButtonUI;
    public Button MaleButtonUI;
    public GameObject Metaverse1;
    public GameObject Metaverse2;

    public string RoomID = "testRoom";
    public static PlayerType SelectedType = PlayerType.Male;

    public static UI_Lobby Instance;

    private void Start()
    {
        LoadLoginInfo();
        AutoLogin();
    }

    private void LoadLoginInfo()
    {
        string loggedInUser = PlayerPrefs.GetString("LoggedInId", string.Empty);
        string loggedInPassword = PlayerPrefs.GetString("LoggedInPassword", string.Empty);
        TMP_InputFieldId.text = loggedInUser;
        TMP_InputFieldPw.text = loggedInPassword;
    }

    private void AutoLogin()
    {
        string loggedInUser = PlayerPrefs.GetString("LoggedInId", string.Empty);
        string loggedInPassword = PlayerPrefs.GetString("LoggedInPassword", string.Empty);

        if (!string.IsNullOrEmpty(loggedInUser) && !string.IsNullOrEmpty(loggedInPassword))
        {
            var user = PersonalManager.Instance.Login(loggedInUser, loggedInPassword);
            if (user != null)
            {
                PhotonNetwork.NickName = loggedInUser;
                PlayerSelection.Instance.SelectedCharacterIndex = user.CharacterIndex;

                if (user.CharacterIndex != 0)
                {
                    PlayerSelection.Instance.ReloadCharacter();
                    SelectCharacterBrowser();
                }
                else
                {
                    Metaverse1.SetActive(false);
                    Metaverse2.SetActive(true);
                }
            }
            else
            {
                Metaverse1.SetActive(true);
            }
        }
        else
        {
            Metaverse1.SetActive(true);
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

        if (RememberToggle.isOn)
        {
            RememberUserInfo(nickname, password);
        }

        var user = PersonalManager.Instance.Login(nickname, password);
        if (user != null)
        {
            PhotonNetwork.NickName = nickname;
            PlayerSelection.Instance.SelectedCharacterIndex = user.CharacterIndex;
            OnClickStartButton();
        }
        else
        {
            Debug.Log("Login failed.");
        }
    }

    private void RememberUserInfo(string nickname, string password)
    {
        if (!PersonalManager.Instance.CheckUser(nickname, password))
        {
            int index = PlayerSelection.Instance.SelectedCharacterIndex;
            PersonalManager.Instance.JoinList(nickname, password, index);
            Debug.Log("New user registered.");
        }

        PlayerPrefs.SetString("LoggedInId", nickname);
        PlayerPrefs.SetString("LoggedInPassword", password);
        Debug.Log("Login successful, user remembered.");
    }

    public void SelectCharacterBrowser()
    {
        Metaverse1.SetActive(false);
        Metaverse2.SetActive(true);
    }

    public void OnClickStartButton()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 20,
                IsVisible = true,
                IsOpen = true,
                EmptyRoomTtl = 1000 * 20,
            };

            PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
            Debug.Log($"{RoomID}");
            SceneManager.LoadScene("LoadingScene"); 
        }
        else
        {
            Debug.LogWarning("PhotonNetwork is not ready.");
        }
    }

    public void OnClickMaleButton() => OnClickPlayerTypeButton(PlayerType.Male);
    public void OnClickFemaleButton() => OnClickPlayerTypeButton(PlayerType.Female);

    private void OnClickPlayerTypeButton(PlayerType Ptype)
    {
        SelectedType = Ptype;
        PlayerSelection.Instance.CharacterSelection(Ptype);
    }

    public void OnNicknameValueChanged(string newValue)
    {
        if (!string.IsNullOrEmpty(newValue))
        {
            PhotonNetwork.NickName = newValue;
        }
    }
}
