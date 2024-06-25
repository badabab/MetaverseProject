using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum PlayerType
{
    Male,
    Female,
}
public class UI_Lobby : MonoBehaviour
{

    public Image WhiteBack;
    public Image Logo;
    public TMP_InputField TMP_InputFieldId;
    public TMP_InputField TMP_InputFieldPw;
    public Toggle RememberToggle;
    public TMP_InputField TMP_InputField;
    public Button NextButtonUI;
    public Button FemaleButtonUI;
    public Button MaleButtonUI;
    public GameObject Metaverse1;
    public GameObject Metaverse2;

    //private string RoomID = "Village";
    public static PlayerType SelectedType = PlayerType.Male;

    public static UI_Lobby Instance;

    private void Start()
    {
        SoundManager.instance.PlayBgm(SoundManager.Bgm.LobbyScene);
        Metaverse1.SetActive(false);
        StartCoroutine(FadeOutWhiteBackAndExecute());
    }

    private IEnumerator FadeOutWhiteBackAndExecute()
    {
        float duration = 5.0f;
        float elapsedTime = 0.0f;
        float startAlpha = 200 / 255.0f; // 200을 0~1 범위로 변환      

        Color color = WhiteBack.color;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, 0, elapsedTime / duration);
            WhiteBack.color = new Color(color.r, color.g, color.b, newAlpha);
            yield return null;
        }

        WhiteBack.color = new Color(color.r, color.g, color.b, 0);

        yield return new WaitForSeconds(1);

        Logo.gameObject.SetActive(false);
        WhiteBack.gameObject.SetActive(false);
        Metaverse1.SetActive(true);
        LoadLoginInfo();
        //AutoLogin();
    }

    private void LoadLoginInfo()
    {
        string loggedInUser = PlayerPrefs.GetString("LoggedInId", string.Empty);
        string loggedInPassword = PlayerPrefs.GetString("LoggedInPassword", string.Empty);
        TMP_InputFieldId.text = loggedInUser;
        TMP_InputFieldPw.text = loggedInPassword;
    }

    public void OnClickNextButton()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
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

        Hashtable loginInfo = new Hashtable
        {
            { "LoggedInId", nickname },
            { "LoggedInPassword", password }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(loginInfo);

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
            //SceneManager.LoadScene("LoadingScene");
            SceneManager.LoadScene("VillageSceneTutorials");
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
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        SelectedType = Ptype;
        PlayerSelection.Instance.CharacterSelection(Ptype);
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
                    FemaleButtonUI.gameObject.SetActive(false);
                    MaleButtonUI.gameObject.SetActive(false);
                }
                else
                {
                    SelectCharacterBrowser();
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
}