using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using UnityEngine.UI;
using System;
using MongoDB.Bson;

public class UI_Notice : MonoBehaviour
{
    public static implicit operator UI_Notice(UI_NoticeList v)
    {
        throw new NotImplementedException();
    }
}
