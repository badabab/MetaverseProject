using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionScene : MonoBehaviour
{
    public Image[] images;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Image_Coroutine());
    }

    IEnumerator Image_Coroutine()
    {
        for (int i = 0; i < images.Length; i++) 
        {
            images[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            images[i].gameObject.SetActive(false);
        }
    }
}
