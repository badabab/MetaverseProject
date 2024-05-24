using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MovementManager : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PortKey;
    public InputField PortKeyPassword_text;
    public float PortKeySpeed;
    public int PortKeyPassword = 4885;
    private int incorrectPasswordAttempts = 0;
    [SerializeField]
    public List<GameObject> JumpLancher;
    public float JumpSpeed;
    [SerializeField]
    public List<GameObject> PowerLancher;
    public float PowerJumpSpeed;
    [SerializeField]
    public List<GameObject> Fly;
    public float FlySpeed;
    [SerializeField]
    public List<GameObject> RandomFly;
    public float RandomFlySpeed;
    [SerializeField]
    public GameObject RepetitionFly;
    public float RepetitionFlySpeed = 1f;

    void Start()
    {
        
    }

    void Update()
    {
        PortKeyMovement();
        JumpLancherMovement();
        PowerLancherMovement();
        FlyMovement();
        RandomFlyMovement();
        RepetitionFlyMovement();
    }
    public void PortKeyMovement()
    {
        
    }
    public void OnPortKeyPassword()
    {
        int Password;
        bool isNumber = int.TryParse(PortKeyPassword_text.text, out Password);
        {
            if (isNumber && Password != PortKeyPassword)
            {
                incorrectPasswordAttempts++;
                Debug.Log("패스워드가 틀렸습니다.");
                if (incorrectPasswordAttempts == 3)
                {
                    Debug.Log("관계자가 아니시군요.");
                }
                else if (incorrectPasswordAttempts == 5)
                {
                    Debug.Log("잔꾀부리지 마세요.");
                }
                return;
            }
            incorrectPasswordAttempts = 0;
        }
    }
    public void JumpLancherMovement()
    {

    }

    public void PowerLancherMovement()
    {

    }
    public void FlyMovement()
    {
        float newY = Mathf.PingPong(Time.time * RepetitionFlySpeed, 115) + -5;

        Vector3 newPosition = new Vector3(RepetitionFly.transform.position.x, newY, RepetitionFly.transform.position.z);
        RepetitionFly.transform.position = newPosition;
    }
    public void RandomFlyMovement()
    {

    }
    public void RepetitionFlyMovement()
    {
        float newY = Mathf.PingPong(Time.time * RepetitionFlySpeed, 1) + 545;

        Vector3 newPosition = new Vector3(RepetitionFly.transform.position.x, newY, RepetitionFly.transform.position.z);
        RepetitionFly.transform.position = newPosition;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}
