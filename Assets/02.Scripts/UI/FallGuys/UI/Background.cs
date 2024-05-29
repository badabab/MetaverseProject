using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public Material MyMaterial;
    public float ScrollSpeed = 0.2f;

    private void Update()
    {
        Vector2 dir = Vector2.up;

        MyMaterial.mainTextureOffset += dir * ScrollSpeed * Time.deltaTime;
    }
}
