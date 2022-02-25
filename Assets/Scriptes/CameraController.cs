using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 60f;
    public float panBorderThickness = 10f;
    public float smoothness = 10f;

    public Vector3 pointPosition;
    public bool mousePressed = false;
    public float scrollSpeed = 10f;
    public float minY = 10f;
    public float maxY = 80f;

    public Vector3 newPosition;

    void Start()
    {
        newPosition = transform.position;
    }

    void Update()
    {
        if (GameManager.gameIsOver)
        {
            this.enabled = false;
            return;
        }

        if (Input.GetKeyDown("u")) PlayerStatus.Money += 1000;
        if (Input.GetKeyDown("i")) { PlayerStatus.missileLauncherBuildOwn++; PlayerStatus.laserBeamerBuildOwn++; }

        if (Input.GetKey("w")/* || Input.mousePosition.y >= Screen.height - panBorderThickness*/)
        {
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("s")/* || Input.mousePosition.y <= panBorderThickness*/)
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("d")/* || Input.mousePosition.x >= Screen.width - panBorderThickness*/)
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }
        if (Input.GetKey("a")/* || Input.mousePosition.x <= panBorderThickness*/)
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetMouseButtonDown(1))
        {
            // Debug.Log("mouse is down");
            mousePressed = true;
            pointPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            // Debug.Log("mouse is up");
            mousePressed = false;
        }

        if (mousePressed)
        {
            Vector3 transition = Input.mousePosition - pointPosition;

            float percentageX = Mathf.Abs(transition.x / (Screen.width / 4));
            float percentageY = Mathf.Abs(transition.y / (Screen.height / 4));
            percentageX = Mathf.Clamp(percentageX, 0, 1);
            percentageY = Mathf.Clamp(percentageY, 0, 1);

            float speedControl = newPosition.y / maxY;
            percentageX *= speedControl;
            percentageY *= speedControl;


            if (transition.y > 0)
            {
                newPosition += Vector3.forward * panSpeed * percentageY;
                // transform.Translate(Vector3.forward * panSpeed * Time.deltaTime * percentageY, Space.World);
            }
            if (transition.y < 0)
            {
                newPosition += Vector3.back * panSpeed * percentageY;
                // transform.Translate(Vector3.back * panSpeed * Time.deltaTime * percentageY, Space.World);
            }
            if (transition.x > 0)
            {
                newPosition += Vector3.right * panSpeed * percentageX;
                // transform.Translate(Vector3.right * panSpeed * Time.deltaTime * percentageX, Space.World);
            }
            if (transition.x < 0)
            {
                newPosition += Vector3.left * panSpeed * percentageX;
                // transform.Translate(Vector3.left * panSpeed * Time.deltaTime * percentageX, Space.World);
            }
            // Debug.Log(transition + ", " + percentageX + ", " + percentageY);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        newPosition -= Vector3.up * scroll * scrollSpeed * 10;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        newPosition.x = Mathf.Clamp(newPosition.x, -10, 80);
        newPosition.z = Mathf.Clamp(newPosition.z, -40, 60);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothness);

        // transform.position = pos;
    }
}
