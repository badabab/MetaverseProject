using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirShipMovement : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> Fly;
    public float FlySpeed;
    public float minY = -5f; // 최소 Y 위치
    public float maxY = 110f; // 최대 Y 위치

    void Update()
    {
        FlyMovement();
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
}
