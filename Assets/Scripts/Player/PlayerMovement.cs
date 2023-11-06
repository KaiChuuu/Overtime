using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public bool canMove = false;

    [SerializeField]
    private InputActionReference movement;

    private Rigidbody playerBody;
    private Vector2 movementInputValue;

    // Start is called before the first frame update
    void Awake()
    {
        playerBody = GetComponent<Rigidbody>();
     
        //Disable Movement
        canMove = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            movementInputValue = movement.action.ReadValue<Vector2>();

            Vector3 direction = new Vector3(movementInputValue.x, 0, movementInputValue.y);

            playerBody.AddForce(direction * speed, ForceMode.Force);
        }
    }

    //Used to move to spawn
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }
}
