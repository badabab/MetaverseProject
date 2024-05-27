using UnityEngine;

public class UI_Minimap : MonoBehaviour
{
    public static UI_Minimap Instance { get; private set; }
    public Player MyPlayer;
    public float YDistance = 20f;
    private Vector3 _initalEulerAngles;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _initalEulerAngles = transform.eulerAngles;
    }
    private void LateUpdate()
    {
        if (MyPlayer == null)
        {
            return;
        }
        Vector3 targetPosition = MyPlayer.transform.position;
        targetPosition.y += YDistance;
        transform.position = targetPosition;

        Vector3 targetEulerAngles = MyPlayer.transform.eulerAngles;
        targetEulerAngles.x = _initalEulerAngles.x;
        targetEulerAngles.z = _initalEulerAngles.z;
        transform.eulerAngles = targetEulerAngles;
    }
}
