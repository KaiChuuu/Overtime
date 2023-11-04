using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [HideInInspector] public CanvasManager canvasManager;
    [HideInInspector] public Camera gameCamera;

    [SerializeField]
    private InputActionReference aim;

    [HideInInspector] public Vector3 touchWorldPosition;

    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    private void OnEnable()
    {
        aim.action.performed += UpdateAim;
    }

    private void OnDisable()
    {
        aim.action.performed -= UpdateAim;
    }

    private void UpdateAim(InputAction.CallbackContext context)
    {
        Vector2 playerDirection = context.ReadValue<Vector2>();

        RaycastHit hit;
        Ray ray = gameCamera.ScreenPointToRay(playerDirection);

        if (Physics.Raycast(ray, out hit))
        {
            //maybe filter to mask layer
            //then create a collider layer for this with specific mask name

            touchWorldPosition = hit.point;
            touchWorldPosition.y = transform.position.y;

            transform.LookAt(touchWorldPosition);

            canvasManager.UpdatePlayerRadar(transform.localRotation.eulerAngles.y);
        }
    }
}
