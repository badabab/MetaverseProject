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
            bool isActive = !GM_UI.activeSelf;
            GM_UI.SetActive(isActive);
            UnityEngine.Cursor.visible = isActive;
            UnityEngine.Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
            GameManager.Instance.Pause();
        }
    }

    public void OnClickQuit()
    {
        GM_UI.SetActive(false);
        GameManager.Instance.Continue();
    }

    public void OnClickReplay()
    {
        GameManager.Instance.Continue();
        GM_UI.SetActive(false);
    }

    public void OnClickVillige()
    {
        GameManager.Instance.BackToVillage();
        GM_UI.SetActive(false);
    }

    public void OnClickGameQuitButton()
    {
        GameManager.Instance.GameOver();
        GM_UI.SetActive(false);
    }
}
