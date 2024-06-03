using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortKeyMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject PortKey1;
    public GameObject PortKey2;
    public GameObject PortKeyPassword;
    public InputField PortKeyPassword_text;

    private bool isMoving = false;

    void Start()
    {
        PortKeyPassword.SetActive(false);
    }

    void Update()
    {
        if (isMoving && !Physics.CheckBox(PortKey1.transform.position, PortKey1.transform.localScale / 2, Quaternion.identity))
        {
            PortKeyPassword.SetActive(false);
            isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PortKeyPassword.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PortKeyPassword.SetActive(false);
        }
    }

    public void OnPortKeyPassword()
    {
        if (PortKeyPassword_text.text == "PortKeyPassword")
        {
            Collider[] colliders = Physics.OverlapBox(PortKey1.transform.position, PortKey1.transform.localScale / 2, Quaternion.identity);

            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {
                    CharacterController characterController = collider.GetComponent<CharacterController>();
                    if (characterController != null)
                    {
                        characterController.enabled = false;
                        collider.transform.position = PortKey2.transform.position;
                        characterController.enabled = true;
                    }
                    else
                    {
                        collider.transform.position = PortKey2.transform.position;
                    }
                }
            }

            isMoving = true;
        }
    }

    // 엔터 키 입력 시 호출되는 함수
    public void CheckForEnterKey()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnPortKeyPassword();
        }
    }
}