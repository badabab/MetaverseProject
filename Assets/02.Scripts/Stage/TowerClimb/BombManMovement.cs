using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManMovement : MonoBehaviour
{
    [SerializeField]
    public GameObject RepetitionFly;
    public float RepetitionFlySpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        RepetitionFlyMovement();
    }

    public void RepetitionFlyMovement()
    {
        float newY = Mathf.PingPong(Time.time * RepetitionFlySpeed, 1) + 545;

        Vector3 newPosition = new Vector3(RepetitionFly.transform.position.x, newY, RepetitionFly.transform.position.z);
        RepetitionFly.transform.position = newPosition;
    }
}
