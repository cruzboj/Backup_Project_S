using UnityEngine;
using UnityEngine.InputSystem;

//local multi p1 vs p2 exc... :: https://www.youtube.com/watch?v=g_s0y5yFxYg
[RequireComponent(typeof(CharacterController))]
public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private float playerSpeed = 15.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;

    //handel rotation ints
    private float currentYRotation = 0f; // Track the current rotation
    private float _rotationFactorPerFrame = 25.0f;

    //gravity
    //new - castRay
    [SerializeField] public LayerMask layerMask; //set to Ground
    RaycastHit hit;
    private float rayBeam = 0.1f;
    public Vector3 boxSize = new Vector3(1f, 0.3f, 1f); //1 ,0.3 ,1
    public bool _grounded = true;
    private float _downForce = 9.8f;


    //jump
    public bool jumped = false; //unity input context  (jumped)
    public int _jumpCount = 0; // jump counter
    private int _maxJumpCount = 1; // max Jumps allowed
    private float _jumpForce = 12f;
    //doublejump effect
    //public ParticleSystem DoublejumpSmoke;
    
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    
    private Vector2 movementInput = Vector2.zero; //unity input context  (movementInput)

    //newfunc - CastRay
    private void OnDrawGizmos()
    {
        //hitbox for grounded.
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position - transform.up * rayBeam, boxSize);
    }


    //[SerializeField] public LayerMask LayerMask_player;
    //void Awake()
    //{
    //    GameObject[] objs = FindObjectsOfType<GameObject>(); // Get all GameObjects in the scene

    //    int count = 0;
    //    foreach (GameObject obj in objs)
    //    {
    //        if ((LayerMask_player.value & (1 << obj.gameObject.layer)) != 0) // Replace with your actual layer name
    //        {
    //            count++;
    //            if (count > 1) // If there is already one, destroy the extra
    //            {
    //                Destroy(gameObject);
    //                return;
    //            }
    //        }
    //    }

    //    DontDestroyOnLoad(gameObject);
    //}

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();

    }
    public void OnJump(InputAction.CallbackContext context)
    {
        //jumped = context.ReadValue<bool>();
        //jumped = context.action.triggered;
        if (context.performed)
        {
            jumped = true; // Set flag when jump is triggered
        }
    }
    void Update()
    {
        //GroundCheck();
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(movementInput.x, 0, 0);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            //gameObject.transform.forward = move;
            handleRotation();
        }

        // Makes the player jump
        if (jumped && groundedPlayer)
        {
            //playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            //jumped = false;
            HandleJump();
        }
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
    }
    void HandleJump()
    {
        if (_grounded && jumped || jumped && (_jumpCount < _maxJumpCount))
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            _jumpCount++;
            //Debug.Log("Jumped! Jump Count: " + _jumpCount);
            jumped = false;
            //_animator.SetBool(_isJumpingHash, true);
            //changeAnimeationState(_PLAYER_JUMP2);
        }
    }
    void handleRotation()
    {
        // Check if there is movement input to determine rotation direction
        if (movementInput.x > 0) // Moving right
        {
            currentYRotation = 90f; // Snap to 90 degrees
        }
        else if (movementInput.x < 0) // Moving left
        {
            currentYRotation = -90f; // Snap to -90 degrees
        }

        // Create a target rotation based on the current Y angle
        Quaternion targetRotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationFactorPerFrame);
    }

    private bool GroundCheck()
    {
        // Check if the box collides with the ground and set the grounded status
        if (Physics.CheckBox(transform.position, boxSize, transform.rotation, layerMask))
        {
            //Debug.Log("Grounded");
            _jumpCount = 0;
            _grounded = true;
            return true;
        }
        else
        {
            //Debug.Log("NOT grounded");
            _grounded = false;
            return false;
        }
    }
}
