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

    [HideInInspector] public Animator animator;
    private int walkingHash;

    // Start is called before the first frame update
    void Awake()
    {
        walkingHash = Animator.StringToHash("Walking");
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

            if(movementInputValue.x != 0 || movementInputValue.y != 0)
            {
                animator.SetBool(walkingHash, true);
            }
            else if(movementInputValue.x == 0 && movementInputValue.y == 0)
            {
                animator.SetBool(walkingHash, false);
            }

            Vector3 direction = new Vector3(movementInputValue.x, 0, movementInputValue.y);

            playerBody.AddForce(direction * speed, ForceMode.Force);
        }
        else
        {
            animator.SetBool(walkingHash, false);
        }
    }

    //Used to move to spawn
    public void SetPosition(Vector3 position)
    {
        gameObject.transform.position = position;
    }

    public void ResetPhysics()
    {
        playerBody.drag = 2f;
    }

    public void UpdatePlayerPhysics(int drag)
    {
        playerBody.drag = drag;
    }
}
