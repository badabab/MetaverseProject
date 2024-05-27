using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortKeyMovement : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> PortKey;
    public InputField PortKeyPassword_text;
    public float PortKeySpeed;
    public int PortKeyPassword = 4885;
    private int incorrectPasswordAttempts = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPortKeyPassword()
    {
        int Password;
        bool isNumber = int.TryParse(PortKeyPassword_text.text, out Password);
        {
            if (isNumber && Password != PortKeyPassword)
            {
                incorrectPasswordAttempts++;
                Debug.Log("패스워드가 틀렸습니다.");
                if (incorrectPasswordAttempts == 3)
                {
                    Debug.Log("관계자가 아니시군요.");
                }
                else if (incorrectPasswordAttempts == 5)
                {
                    Debug.Log("잔꾀부리지 마세요.");
                }
                return;
            }
            incorrectPasswordAttempts = 0;
        }
    }
}
