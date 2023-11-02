using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dampTime = 0.2f;
    public float screenEdgeBuffer = 4f;
    public float minSize = 6.5f;
    [HideInInspector] public Transform[] targets;

    [HideInInspector] public Camera gameCamera; //Camera Reference
    private Vector3 desiredPosition; //Position camera is moving towards

    private Vector3 moveVelocity; //Reference velocity for smooth damping of camera position
    private float zoomSpeed; //Reference velocity for smooth damping of camera size

    private void Awake()
    {
        gameCamera = GetComponentInChildren<Camera>();
    }

    private void FixedUpdate()
    {
        Move(); //Center camera
        Zoom(); //Fit all targets within scene
    }

    private void Move()
    {
        FindAveragePositions();

        //Smoothly transition to position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref moveVelocity, dampTime);
    }

    private void FindAveragePositions()
    {
        Vector3 avgPosition = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < targets.Length; i++)
        {
            //Ignore targets that are non-active
            if (!targets[i].gameObject.activeSelf) continue;

            avgPosition += targets[i].position;
            numTargets++;
        }

        if (numTargets > 0) avgPosition /= numTargets;

        avgPosition.y = transform.position.y;

        desiredPosition = avgPosition;
    }

    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        gameCamera.orthographicSize = Mathf.SmoothDamp(gameCamera.orthographicSize, requiredSize, ref zoomSpeed, dampTime);
    }

    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(desiredPosition);

        float size = 0f;

        for (int i = 0; i < targets.Length; i++)
        {
            //Ignore targets that are non-active
            if (!targets[i].gameObject.activeSelf) continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(targets[i].position);

            //Create a larger space, encompassing the current target
            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

            //Take largest sizes
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));
            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / gameCamera.aspect);
        }

        size += screenEdgeBuffer; //Maintain buffer space for screen
        size = Mathf.Max(size, minSize); //Bound size of screen

        return size;
    }

    //Manually update camera position and size
    public void SetCameraPositionAndSize()
    {
        FindAveragePositions();
        transform.position = desiredPosition;
        gameCamera.orthographicSize = FindRequiredSize();
    }
}
