using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UI_Personal : MonoBehaviour
{
    public Toggle RememberToggle;
    public TMP_InputField InputFieldId;
    public TMP_InputField InputFieldPw;

    public void OnClickLogin()
    {
        RememberID rememberID = RememberToggle.isOn ? RememberID.Remember : RememberID.Nope;
        string name = InputFieldId.text;
        string password = InputFieldPw.text;
        if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
        {
            Debug.Log("ID와 Password 둘 다 입력을 해주세요.");
            return;
        }

        if (rememberID == RememberID.Remember)
        {
            if (!PersonalManager.Instance.CheckUser(name, password)) 
            {
                PersonalManager.Instance.JoinList(name, password);
            }
            var user = PersonalManager.Instance.Login(name, password);
            if (user != null)
            {
                PlayerPrefs.SetString("LoggedInUser", name);
                Debug.Log("Login successful, user remembered.");
                // Load the lobby screen here
            }
        }

        else if (rememberID == RememberID.Nope)
        {
            var user = PersonalManager.Instance.Login(name, password);
            if (user != null)
            {
                Debug.Log("Login successful.");
                // Load the lobby screen here
            }
            else
            {
                Debug.Log("Login failed.");
            }
        }
    }
        private void Start()
    {
        AutoLogin();
    }

    private void AutoLogin()
    {
        string loggedInUser = PlayerPrefs.GetString("LoggedInUser", null);
        if (!string.IsNullOrEmpty(loggedInUser))
        {
            // Assuming the password is not stored and user must re-enter it
            Debug.Log($"Welcome back, {loggedInUser}!");
            // Load the lobby screen here
        }
    }
}
