using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 0.1f;

    [SerializeField]
    private InputActionReference movement;

    private Rigidbody playerBody;
    private Vector2 movementInputValue;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        movementInputValue = movement.action.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        Vector3 direction = new Vector3(movementInputValue.x, 0, movementInputValue.y);

        playerBody.MovePosition(playerBody.position + direction * speed);
    }
}
