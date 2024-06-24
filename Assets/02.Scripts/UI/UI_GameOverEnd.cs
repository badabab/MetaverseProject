using UnityEngine;


public class UI_GameOverEnd : MonoBehaviour
{
    public GameObject Red;
    public GameObject Red1;
    public GameObject Yellow;
    public GameObject Yellow1;
    public GameObject Blue;
    public GameObject Blue1;

    public float RedStop;
    public float Red1Stop;
    public float YellowStop;
    public float Yellow1Stop;
    public float BlueStop;
    public float Blue1Stop;

    public float speed;

    private RectTransform redRectTransform;
    private RectTransform red1RectTransform;
    private RectTransform yellowRectTransform;
    private RectTransform yellow1RectTransform;
    private RectTransform blueRectTransform;
    private RectTransform blue1RectTransform;

    private Vector3 redTargetPosition;
    private Vector3 red1TargetPosition;
    private Vector3 yellowTargetPosition;
    private Vector3 yellow1TargetPosition;
    private Vector3 blueTargetPosition;
    private Vector3 blue1TargetPosition;

    void Start()
    {
        redRectTransform = Red.GetComponent<RectTransform>();
        red1RectTransform = Red1.GetComponent<RectTransform>();
        yellowRectTransform = Yellow.GetComponent<RectTransform>();
        yellow1RectTransform = Yellow1.GetComponent<RectTransform>();
        blueRectTransform = Blue.GetComponent<RectTransform>();
        blue1RectTransform = Blue1.GetComponent<RectTransform>();

        redTargetPosition = new Vector3(RedStop, redRectTransform.localPosition.y, redRectTransform.localPosition.z);
        red1TargetPosition = new Vector3(Red1Stop, red1RectTransform.localPosition.y, red1RectTransform.localPosition.z);
        yellowTargetPosition = new Vector3(YellowStop, yellowRectTransform.localPosition.y, yellowRectTransform.localPosition.z);
        yellow1TargetPosition = new Vector3(Yellow1Stop, yellow1RectTransform.localPosition.y, yellow1RectTransform.localPosition.z);
        blueTargetPosition = new Vector3(BlueStop, blueRectTransform.localPosition.y, blueRectTransform.localPosition.z);
        blue1TargetPosition = new Vector3(Blue1Stop, blue1RectTransform.localPosition.y, blue1RectTransform.localPosition.z);
    }

    void Update()
    {
        MoveToTarget(redRectTransform, redTargetPosition);
        MoveToTarget(red1RectTransform, red1TargetPosition);
        MoveToTarget(yellowRectTransform, yellowTargetPosition);
        MoveToTarget(yellow1RectTransform, yellow1TargetPosition);
        MoveToTarget(blueRectTransform, blueTargetPosition);
        MoveToTarget(blue1RectTransform, blue1TargetPosition);
    }

    private void MoveToTarget(RectTransform rectTransform, Vector3 targetPosition)
    {
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, speed * Time.deltaTime);
    }
}
