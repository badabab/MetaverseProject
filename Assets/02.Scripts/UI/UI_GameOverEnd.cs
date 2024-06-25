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

    private bool redReached;
    private bool red1Reached;
    private bool yellowReached;
    private bool yellow1Reached;
    private bool blueReached;
    private bool blue1Reached;

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

        redReached = false;
        red1Reached = false;
        yellowReached = false;
        yellow1Reached = false;
        blueReached = false;
        blue1Reached = false;
    }

    void Update()
    {
        if (!redReached)
            redReached = MoveToTarget(redRectTransform, redTargetPosition);

        if (!red1Reached)
            red1Reached = MoveToTarget(red1RectTransform, red1TargetPosition);

        if (!yellowReached)
            yellowReached = MoveToTarget(yellowRectTransform, yellowTargetPosition);

        if (!yellow1Reached)
            yellow1Reached = MoveToTarget(yellow1RectTransform, yellow1TargetPosition);

        if (!blueReached)
            blueReached = MoveToTarget(blueRectTransform, blueTargetPosition);

        if (!blue1Reached)
            blue1Reached = MoveToTarget(blue1RectTransform, blue1TargetPosition);
    }

    private bool MoveToTarget(RectTransform rectTransform, Vector3 targetPosition)
    {
        rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPosition, speed * Time.deltaTime);
        if (Vector3.Distance(rectTransform.localPosition, targetPosition) < 0.1f)
        {
            rectTransform.localPosition = targetPosition;
            return true;
        }
        return false;
    }
}
