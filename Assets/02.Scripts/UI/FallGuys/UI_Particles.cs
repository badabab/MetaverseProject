using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Particles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FadeOut();
    }

    public IEnumerator FadeOut()
    {
        gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        gameObject.SetActive(false);   
    }
}
