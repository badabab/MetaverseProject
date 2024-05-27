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
    public List<GameObject> Fly;
    public float FlySpeed;
    public float minY = -5f; // 최소 Y 위치
    public float maxY = 110f; // 최대 Y 위치

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

    public void FlyMovement()
    {
        foreach (GameObject fly in Fly)
        {
            if (fly != null)
            {
                // 현재 위치에서 부드러운 이동
                float newY = Mathf.PingPong(Time.time * FlySpeed, maxY - minY) + minY;

                // 각 Fly 오브젝트의 위치를 새로운 Y 값으로 설정
                Vector3 newPosition = new Vector3(fly.transform.position.x, newY, fly.transform.position.z);

                // maxY에 도달하면 minY로 이동하고, minY에 도달하면 maxY로 이동하도록 설정
                if (newY >= maxY || newY <= minY)
                {
                    FlySpeed *= -1; // 속도의 부호를 바꿔 반대 방향으로 이동
                }

                fly.transform.position = newPosition;
            }
        }
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
