using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Cannon;
    public GameObject CannonBody;
    public GameObject CannonBallFire;
    public List<GameObject> CannonFire;
    public List<GameObject> ShipFire;
    public List<GameObject> CannonWheels;
    public GameObject ShipDwon;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Cannon != null)
            {
                Cannon.SetActive(!Cannon.activeSelf);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            

        }
    }

    void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {

        }
    }
}
