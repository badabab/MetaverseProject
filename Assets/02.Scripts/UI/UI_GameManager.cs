using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UI_GameManager : MonoBehaviour
{
    public static UI_GameManager Instance;
    public GameObject GM_UI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GM_UI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleUI();
        }
    }

    private void ToggleUI()
    {
        bool isActive = !GM_UI.activeSelf;
        GM_UI.SetActive(isActive);
        UnityEngine.Cursor.visible = isActive;
        UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;

        if (isActive)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            GameManager.Instance.Continue();
        }
    }

    public void OnClickQuit()
    {
        GM_UI.SetActive(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.Continue();
    }

    public void OnClickReplay()
    {
        GM_UI.SetActive(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.Continue();
    }

    public void OnClickVillige()
    {
        GM_UI.SetActive(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.BackToVillage();
    }

    public void OnClickGameQuitButton()
    {
        GM_UI.SetActive(false);
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.GameOver();
    }
}
