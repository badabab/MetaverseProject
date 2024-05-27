using UnityEngine;
using UnityEngine.UI;

public class UI_Map : MonoBehaviour
{
    public Image Map;
    private void Start()
    {
        Map.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Map.gameObject.SetActive(!Map.gameObject.activeSelf);
        }
    }
}
