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
                float newY = Mathf.PingPong(Time.time * FlySpeed, maxY - minY) + minY;

                Vector3 newPosition = new Vector3(fly.transform.position.x, newY, fly.transform.position.z);

                if (newY >= maxY || newY <= minY)
                {
                    FlySpeed *= -1; 
                }

                fly.transform.position = newPosition;
            }
        }
    }
}
