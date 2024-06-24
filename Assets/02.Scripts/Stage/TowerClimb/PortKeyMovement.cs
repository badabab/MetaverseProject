using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortKeyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject PortKey1;
    [SerializeField]
    private GameObject PortKey2;
    [SerializeField]
    private InputField InputField;

    private bool isPlayerInside = false;

    private void Start()
    {
        InputField.gameObject.SetActive(false);
        InputField.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputField.gameObject.SetActive(true);
            InputField.ActivateInputField();
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputField.gameObject.SetActive(false);
            InputField.text = string.Empty;
            isPlayerInside = false;
        }
    }

    private void OnInputFieldEndEdit(string input)
    {
        if (isPlayerInside && input == "ABCD")
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Vector3 newPosition = PortKey2.transform.position;
                player.transform.position = newPosition;
            }
            InputField.gameObject.SetActive(false);
            InputField.text = string.Empty;
        }
    }
}