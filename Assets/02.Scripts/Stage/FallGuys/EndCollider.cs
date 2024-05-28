using UnityEngine;

public class EndCollider : MonoBehaviour
{
    public Transform Start2, Start3;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.name == "End1")
            {
                other.transform.position = Start2.position;
            }
            else if (gameObject.name == "End2")
            {
                other.transform.position = Start3.position;
            }
            else if (gameObject.name == "End3")
            {
                Debug.Log("게임 끝~");
            }
        }
    }
}
