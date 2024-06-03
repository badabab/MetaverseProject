using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Description : MonoBehaviour
{
    public GameObject Description;

    // Update is called once per frame
    void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Go)
        {
            Description.SetActive(true);
        }
        else if (FallGuysManager.Instance._currentGameState != GameState.Go)
        { 
            Description.SetActive(false);
        }
    }
}
