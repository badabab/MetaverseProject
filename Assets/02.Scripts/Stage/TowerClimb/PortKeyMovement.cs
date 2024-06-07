using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortKeyMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject PortKey1;
    public GameObject PortKey2;
    public InputField InputField;

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPosition = PortKey2.transform.position; 
            other.transform.position = newPosition; 
        }
    }
}