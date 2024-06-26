using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorGameClosedSound : MonoBehaviour
{
    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            SoundManager.instance.PlayBgm(SoundManager.Bgm.HorrorGameClosed);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isPlayerInside)
            {
                isPlayerInside = false;
                SoundManager.instance.StopBgm(SoundManager.Bgm.HorrorGameClosed);
            }
        }
    }
}
