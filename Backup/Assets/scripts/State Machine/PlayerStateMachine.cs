using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

//local multi p1 vs p2 exc... :: https://www.youtube.com/watch?v=g_s0y5yFxYg
[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    [Header("Player Parameters")]
    public int PlayerIndex;
    //attaking collider
    public Transform PlayerTransform;

    public int getPlayerIndex() { return PlayerIndex; }
    [SerializeField] private float playerWeight = 1.4f;
    public float PlayerWeight { get { return playerWeight; } }

    private PlayerInput playerInput;

    private CharacterController controller;

    //handel rotation ints
    private float currentYRotation = 0f; // Track the current rotation
    private float _rotationFactorPerFrame = 26f; //was 25.8


    [Header("Gravity Parameters")]
    [SerializeField] private float gravityValue = -9.81f;
    //character controller down force
    private Vector3 playerVelocity;
    //public bool groundedPlayer;
    public float GravityValue { get { return gravityValue; } }


    [Header("RAY CAST Ground")]
    [SerializeField] public LayerMask layerMask; //set to Ground
    RaycastHit hit;
    private float rayBeam = 0.1f;
    public Vector3 boxSize = new Vector3(1f, 0.02f, 1f); //1 ,0.02 ,1 thight ground check
    public bool _grounded = true;
    //private float _downForce = 9.8f;
    public bool Grounded { get { return _grounded; } set { _grounded = value; } }


    private void OnDrawGizmos()
    {
        //hitbox for grounded.
        //Gizmos.color = Color.green;
        //Gizmos.DrawCube(transform.position - transform.up * rayBeam, boxSize);

        ////attack1_hitbox
        //Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        //Vector3 positionInFront = transform.position + transform.forward * location_ray_position_1; // "2f" הוא המרחק מהדמות
        //positionInFront.y += location_ray_height_1; // הזז את הגיזמו למעלה ב-0.5 יחידות
        //Gizmos.DrawCube(positionInFront, boxSize_hitbox_1);

        // Offset slightly below the player
        Vector3 checkPosition = transform.position + Vector3.down * 0.1f;

        // בדיקה אם הדמות נוגעת בקרקע
        bool isGrounded = Physics.CheckBox(checkPosition, boxSize, transform.rotation, layerMask);

        // אם נוגע ברצפה -> צבע ורוד, אם לא -> צבע ירוק
        Gizmos.color = isGrounded ? Color.magenta : Color.green;

        // מצייר את הקובייה כדי לראות את אזור הבדיקה
        Gizmos.DrawWireCube(checkPosition, boxSize);

    }


    [Header("Jump parameters")]
    [SerializeField] private float jumpHeight = 2f;
    public bool jumped = false; //unity input context  (jumped)
    public int _jumpCount = 0; // jump counter
    public int _maxJumpCount = 1; // max Jumps allowed
    private float maxJumptime = 0.5f;
    private float intitialjumpVelocity;
    public float _jumpSpeed = 30f;
    public float InitialJumpVelocity { get { return intitialjumpVelocity; } }

    //https://www.youtube.com/watch?v=RFix_Kg2Di0 for smooth jumping after leaving platform
    private float _coyoteTime = 1f;
    private float _coyoteTimeCounter;
    private float _jumpBufferTime = 1f;
    private float _jumpBufferCounter;

    public float CoyoteTime { get { return _coyoteTime; } }
    public float CoyoteTimeCounter { get { return _coyoteTimeCounter; } set { _coyoteTimeCounter = value; } }
    public float JumpBufferTime { get { return _jumpBufferTime; } }
    public float JumpBufferCounter { get { return _jumpBufferCounter; } set { _jumpBufferCounter = value; } }

    //doublejump effect
    public ParticleSystem DoublejumpSmoke;
    


    //state machine setters && getters 
    public bool Jumped { get { return jumped; } set { jumped = value; } }
    public int JumpCount { get { return _jumpCount; } set { _jumpCount = value; } }
    public int MaxJumpCount { get { return _maxJumpCount; } set { _maxJumpCount = value; } }
    public float MaxJumptime { get { return maxJumptime; } set { maxJumptime = value; } }
    public float IntiaialJumpVelocity { get { return intitialjumpVelocity; } set { intitialjumpVelocity = value; } }
    public float JumpMaxHeight { get { return jumpHeight; } }
    public float JumpMaxSpeed { get { return _jumpSpeed; } }


    [Header("Movement Parameters")]
    [SerializeField] private float playerSpeed = 15.0f;
    [SerializeField] private float DownInput = 0.09f;
    public bool _isMovementPressed;
    public bool _isWalking;
    public bool _isRunning;
    //movement calculation
    Vector3 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _appliedMovment;
    public float _walkSpeed = 6f;
    public float _runSpeed = 15f;
    private float _currentSpeed;
    private float _threshold = 0.9f;

    public Vector3 CurrentMovement { get { return _currentMovement; } set { _currentMovement = value; } }
    public Vector3 CurrentMovementInput { get { return _currentMovementInput; } }
    public Vector3 AppliedMovement { get { return _appliedMovment; } set { _appliedMovment = value; } }
    public float Threshold { get { return _threshold; } }

    //animations
    public Animator _animator;
    const string _PLAYER_RUN = "Player_run";
    const string _PLAYER_IDLE = "Player_idle";
    const string _PLAYER_CROUCH = "Player_crouch";
    const string _PLAYER_WALK = "Player_walk";
    const string _PLAYER_JUMP = "Player_jump";
    const string _PLAYER_DOUBLE_JUMP = "Player_doublejump";
    const string _PLAYER_ATTACK = "Attack 1";
    const string _PLAYER_JUMP_RECOIL = "Player_jump_recoil";
    //getters
    public Animator ANIMATOR { get { return _animator; } }
    public string PLAYER_JUMP { get { return _PLAYER_JUMP; } }
    public string PLAYER_DOUBLE_JUMP { get { return _PLAYER_DOUBLE_JUMP; } }
    public string PLAYER_IDLE { get { return _PLAYER_IDLE; } }
    public string PLAYER_ATTACK { get { return "Attack 1"; } }
    public string PLAYER_JUMP_RECOIL { get { return "Player_jump_recoil"; } }

    [Header("KnockBack")]
    public float KBForce = 1f;
    public float _kbCounter = 0.01f;

    private float _kbTotalTime = 0.05f;
    private bool _knockFromRight;

    public float fixedForce = 2.0f;
    public float PlayerDamage;
    public float KBCounter { get { return _kbCounter; } set { _kbCounter = value; } }
    public float KBTotalTime { get { return _kbTotalTime; } set { _kbTotalTime = value; } }
    public bool KnockFromRight { get { return _knockFromRight; } set { _knockFromRight = value; } }

    [Header("RAY CAST Knockback")]
    [SerializeField] public LayerMask KB_PlayerMask;
    RaycastHit hit_hitbox;
    private float rayBeam_knockback = 0.1f;
    public LayerMask Global_KB_PlayerMask { get { return KB_PlayerMask; } }

    [Header("Normal Attack Parameters")]
    public bool _isAttackPressed;
    public float TimeAttack1 = 10f;

    //projectile
    public bool _hasProjectil;
    //new using script ProjectileBehavior
    public ProjectileBehaviour projectilePrefab;
    public Transform LauchOffSet; //maybe _ctx.transform

    public bool IsAttackPressed { get { return _isAttackPressed; } }
    public bool HasProjectil { get { return _hasProjectil; } }

    [Header("RAY CAST Attack1")]
    public Vector3 boxSize_hitbox_1 = new Vector3(1f, 0.02f, 1f); //1 ,0.3 ,1
    public float location_ray_height_1;
    public float location_ray_position_1;
    public bool hitbox1 = false;
    public float damage1 = 10f;
    public float getDamage1() { return damage1; }
    public Vector3 BoxSize_hitbox_1 { get { return boxSize_hitbox_1; } }
    public float Location_Ray_Position_1 { get { return location_ray_position_1; } }
    public float Location_Ray_height_1 { get {return location_ray_height_1; } }
    public bool HitBox1 { get { return hitbox1; } set { hitbox1 = value; } }
   


    //[SerializeField] private Collider[] childCollider; // Reference to the capsule collider
    
    //State Variables
    PlayerBaseState _currentState;
    PlayerStateFactory _state;
    //getters && setters
    public PlayerBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }
    

    public void takehealth(float Force)
    {
        //        [ [ [  [ (p / 10) + (p * pd)/20 ] *             [200 / w+100 ] ] * 1.4] +18]
        KBForce = (((((Force / 10) + ((Force * 10) / 20)) * (200 / (playerWeight + 100))) * 1.4f) + 18) / 30;

        fixedForce = Force;
        /* [ [ [ [ (p/10 + pd/20) * (200/ (w+100) ) * 1.4 ] + 18 ] * s ] + b ] * r ] formula
        p = currenthealth (force)
        d = other_player_damage
        w = character weight
        s = Knockback scaling (knock back increase rate) 
        b = base knock back;
        r = 1 or 0 Error turms (rage,crouch camcelling , character size , frozem effect ...)
        */
    }

    private void Awake()
    {

        //get player index
        playerInput = GetComponent<PlayerInput>(); // מקבל את ה- PlayerInput
        if (playerInput != null)
        {
            PlayerIndex = playerInput.playerIndex + 1;
            Debug.Log("Player Index: " + PlayerIndex);
        }
        else
        {
            Debug.LogError("PlayerInput component not found!");
        }

        //state setup
        _state = new PlayerStateFactory(this);
        _currentState = _state.Grounded();
        _currentState.EnterState();

        setupJumpVariables();
    }

    void setupJumpVariables()
    {
        float timeToApex = maxJumptime / 2;
        gravityValue = (-2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        intitialjumpVelocity = (2 * jumpHeight * 0.0681f) / timeToApex;
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        PlayerTransform = this.transform;
    }

    void Update()
    {
        //update index
        PlayerIndex = playerInput.user.index;

        //Movement check
        if (_isMovementPressed)
        {
            handleRotation();
        }

        if (KBCounter < 0)
        {
            //controller.Move(_appliedMovment * Time.deltaTime);
            //_appliedMovment.x = _currentMovement.x;
            //_appliedMovment.y = _currentMovement.y;
            handleMovement();
        }
        else
        {
            Vector3 move;
            if (_knockFromRight)
            {
                move = new Vector3(-KBForce, KBForce, 0);
            }
            else
            {
                move = new Vector3(KBForce, KBForce, 0);
            }
            controller.Move(move * Time.deltaTime * playerSpeed);
            _kbCounter -= Time.deltaTime;
        }

        //if (_kbCounter > 0)
        //{
        //    // הגדר תנועת knockback ברורה
        //    if (_knockFromRight)
        //    {
        //        _currentMovement.x = -KBForce;  // זרוק שמאלה
        //        _currentMovement.y = KBForce;   // זרוק למעלה
        //    }
        //    else
        //    {
        //        _currentMovement.x = KBForce;   // זרוק ימינה
        //        _currentMovement.y = KBForce;    // זרוק למעלה
        //    }

        //    handleMovement();
        //    _kbCounter -= Time.deltaTime;
        //}
        //else
        //{
        //    // החזר לתנועה רגילה
        //    _currentMovement.x = 0;
        //    handleMovement();
        //}


        //Attack Check
        //if (_isAttackPressed && _grounded)
        //{
        //    //handleAttack();
        //    //Attack1();
        //}

        //else if (_isAttackPressed && !_grounded)
        //{
        //    //handleAirAttack();
        //}
        //else
        //{

        //}

        //idont think we can pull in on state machine
        JumpBufferCounter -= Time.deltaTime;

        GroundCheck();
        handleGravity();

        // Jump Check
        //if (jumped && _grounded || jumped && (_jumpCount < _maxJumpCount))
        //{
        //    HandleJump();
            
        //}
        _currentState.UpdateState();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _isMovementPressed = _currentMovementInput.magnitude > 0;
        
        if (!_isMovementPressed)
        {
            _isRunning = false;
            _isWalking = false;
            _animator.Play(_PLAYER_IDLE);
        }
        else if (_grounded && _currentMovementInput.y < 0 && _currentMovementInput.magnitude >= Threshold)
        {
            _isRunning = false;
            _isWalking = false;
            _animator.Play(_PLAYER_CROUCH);
        }
        else if (_currentMovementInput.magnitude >= _threshold)
        {
            _isRunning = true;
            _isWalking = false;
            _animator.Play(_PLAYER_RUN);
        }
        
        else if (_currentMovementInput.magnitude < _threshold)
        {
            _isWalking = true;
            _isRunning = false;
            _animator.Play(_PLAYER_WALK);
        }
        


        //old movement
        //movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (_jumpCount <= _maxJumpCount)
        {
            if (context.performed) // Trigger only when the button is pressed once
            {
                //Debug.LogError("Ouch you clicked me!");

                jumped = true;
                _grounded = false;
                //StartCoroutine(SpawnPrefabsWithDelay(0.025f)); //relese the jump button
            }
            else if (context.canceled) // Reset on release (optional)
            {
                jumped = false;
            }
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isAttackPressed = true;
        }
        else if (context.canceled)
        {
            _isAttackPressed = false;
            StartCoroutine(SpawnDelay(TimeAttack1));
            //for (int i = 0; i < childCollider.Length; i++)
            //{
            //    childCollider[i].enabled = false; // להדליק את הקוליידר בזמן התקפה
            //}
        }
    }

    private IEnumerator SpawnPrefabsWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        jumped = false;

    }

    void handleMovement()
    {

        Vector3 move = Vector3.zero; //Initialize move to avoid unassigned variable error

        if (_kbCounter < 0)
        {
            //crouch
            if(_grounded && _currentMovementInput.y < 0)
            {
                //impement small hurtbox script
                //_currentMovement.y -= DownInput;
                
            }
            //walk
            else if(_currentMovementInput.magnitude < _threshold)
            {
                //fix
                //_appliedMovment.x = _currentMovement.x * _walkSpeed;
                //_appliedMovment.y = _currentMovement.y;
                //
                _currentMovement = new Vector3(_currentMovementInput.x, _currentMovement.y, 0);
                _appliedMovment = new Vector3(_currentMovement.x * _walkSpeed, _currentMovement.y, 0);
                move = _appliedMovment;
                //Debug.Log($"Walking: Move = {move}");
                //move = new Vector3(_currentMovementInput.x , _currentMovement.y, 0);
                //controller.Move(move * Time.deltaTime * _walkSpeed);
            }
            //run
            else if(_currentMovementInput.magnitude >= _threshold)
            {
                //fix
                //_appliedMovment.x = _currentMovement.x * _runSpeed;
                //_appliedMovment.y = _currentMovement.y;
                //
                _currentMovement = new Vector3(_currentMovementInput.x, _currentMovement.y, 0);
                _appliedMovment = new Vector3(_currentMovement.x * _runSpeed, _currentMovement.y, 0);
                move = _appliedMovment;
                //Debug.Log($"Running: Move = {move}");
                //move = new Vector3(_currentMovementInput.x, _currentMovement.y, 0);
                //controller.Move(move * Time.deltaTime * _runSpeed);
            }

            controller.Move(Time.deltaTime * move);
        }
        else
        {
            move = new Vector3((_currentMovement.x * _currentMovement.x) / playerWeight, (_currentMovement.y * _currentMovement.y) / playerWeight, 0); //look at desmos [-(0.5* 20)/x]
            Debug.Log($"Knockback Move: X={move.x}, Y={move.y}, Force={KBForce}");
            controller.Move(move * Time.deltaTime);
        }



        if (!_grounded && _currentMovementInput.y < 0 && _currentMovementInput.magnitude >= Threshold)
        {
            //float adjustedDownInput = Mathf.Clamp(DownInput/playerWeight, 0f, 2* DownInput); //cap down speed to 1f
            //_currentMovement.y -= adjustedDownInput;

            _currentMovement.y -= DownInput;
        }
        //Vector3 move;
        //if (_kbCounter > 0)
        //{
        //    // וודא שיש תנועה גם ב-X
        //    move = new Vector3((_currentMovement.x * _currentMovement.x)/playerWeight, (_currentMovement.y * _currentMovement.y) / playerWeight, 0); //look at desmos [-(0.5* 20)/x]
        //    Debug.Log($"Knockback Move: X={move.x}, Y={move.y}, Force={KBForce}");
        //}
        //else
        //{
        //    move = new Vector3(_currentMovementInput.x, _currentMovement.y, 0);
        //}

        //controller.Move(move * Time.deltaTime * playerSpeed);

        //if (!_grounded && _currentMovementInput.y < 0)
        //{
        //    _currentMovement.y -= DownInput;
        //}

        //Vector3 move = new Vector3(_currentMovementInput.x, _currentMovement.y / jumpHeight, 0);
        //controller.Move(move * Time.deltaTime * playerSpeed);
        //if (!_grounded && _currentMovementInput.y < 0)
        //{
        //    _currentMovement.y -= DownInput;
        //}
    }
    void handleRotation()
    {
        // Check if there is movement input to determine rotation direction
        if (_currentMovementInput.x > 0) // Moving right
        {
            currentYRotation = 90f; // Snap to 90 degrees
        }
        else if (_currentMovementInput.x < 0) // Moving left
        {
            currentYRotation = -90f; // Snap to -90 degrees
        }

        // Create a target rotation based on the current Y angle
        Quaternion targetRotation = Quaternion.Euler(0f, currentYRotation, 0f);

        // Smoothly interpolate to the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationFactorPerFrame);
    }
    void handleGravity()
    {
        //Debug.Log($"Move Input: {_currentMovementInput}, Movement _currentMovement: {_currentMovement} ,Movement _appliedMovment :{_appliedMovment} ");
        //old and workin

        //if (controller.isGrounded)
        //{
        //    float groundedGravity = -.05f;
        //    _currentMovement.y = groundedGravity;
        //}
        //else
        //{
        //    _currentMovement.y += gravityValue * Time.deltaTime;
        //}

        //new fix maybe

        float maxFallSpeed = gravityValue * playerWeight * 4; // Adjust as needed
        float maxRiseSpeed = -0.02f; // Adjust to prevent "flying"

        bool isFalling = _currentMovement.y <= 0.0f || !jumped;
        float fallmultiplier = playerWeight;

        if (isFalling)
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = CurrentMovement.y + (gravityValue / fallmultiplier  * Time.deltaTime);
            _currentMovement.y = Mathf.Clamp(_currentMovement.y, maxFallSpeed, 80 * playerWeight);
            _appliedMovment.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f, -20.0f);
        }
            else
            {
                // applied when character is not grounded
                float previousYVelocity = _currentMovement.y;
                _currentMovement.y = _currentMovement.y + (Time.deltaTime);
                _appliedMovment.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f);

            }

            //if (controller.isGrounded)
            //{
            //    float groundedGravity = -.05f;
            //    _currentMovement.y = groundedGravity;
            //}
            //else
            //{
            //    //applied when character is not grounded
            //    float previousYVelocity = _currentMovement.y;
            //    _currentMovement.y = _currentMovement.y + (Time.deltaTime);
            //    _appliedMovment.y = Mathf.Max((previousYVelocity + _currentMovement.y) * .5f, -0.02f);
            //}
        }

    private void GroundCheck()
    {
        Vector3 checkPosition = transform.position + Vector3.down * 0.1f;
        // Check if the box collides with the ground and set the grounded status
        if (Physics.CheckBox(transform.position, boxSize, transform.rotation, layerMask))
        // Perform the check
        //if (Physics.CheckBox(checkPosition, boxSize, transform.rotation, layerMask))
        {
            //Debug.DrawRay(checkPosition, Vector3.down * hit.distance, Color.red);
            _coyoteTimeCounter = _coyoteTime;
            //Debug.Log("Grounded");
            _jumpCount = 0;
            _grounded = true;

            //groundedPlayer = _grounded;
            //return true;
            
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
            //Debug.Log("NOT grounded");
            _grounded = false;

            //groundedPlayer = _grounded;
            //return false;
            //Debug.DrawRay(checkPosition, Vector3.down * 1f, Color.green);
        }

        // Debugging: Show the actual box in the Scene
        Debug.DrawRay(checkPosition, Vector3.down * 0.2f, Color.green, 0.1f);

    }

    //private void handleAttack()
    //{
    //    _animator.Play(_PLAYER_ATTACK);
    //    for (int i = 0; i < childCollider.Length; i++)
    //    {
    //        childCollider[i].enabled = true; // להדליק את הקוליידר בזמן התקפה
    //    }

    //    StartCoroutine(SpawnDelay(TimeAttack1));
    //}

    //void HandleJump()
    //{
    //    if (Jumped && /*controller.isGrounded*/ Grounded || Jumped && (JumpCount < MaxJumpCount))
    //    {
    //        Vector3 movement = CurrentMovement;  // Get current movement
    //        movement.y = IntiaialJumpVelocity / PlayerWeight;  // Modify only y-axis
    //        CurrentMovement = movement; // Apply updated movement

    //        JumpCount++;
    //        Jumped = false;
    //    }
    //}

    //handle knockback

    private IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

    }

    //raycast cuz unity is shiiit (new)
    //private void Attack1()
    //{
    //    _animator.Play(_PLAYER_ATTACK);

    //    // בדיקה האם יש שחקן באזור הפגיעה
    //    Collider[] hitPlayers = Physics.OverlapBox(transform.position, boxSize_hitbox_1, transform.rotation, KB_PlayerMask);

    //    if (hitPlayers.Length > 0)
    //    {

    //        //Debug.Log("attack 1 active");
    //        hitbox1 = true;

    //        foreach (Collider player in hitPlayers)
    //        {
    //            PlayerControler playerScript = player.GetComponent<PlayerControler>();
    //            HealthControler playerHealthScript = player.GetComponent<HealthControler>();

    //            if (playerScript != null && playerScript.getPlayerIndex() != this.getPlayerIndex())
    //            {
    //                Debug.Log("Damage control: " + player.name);

    //                // קביעת זמן Knockback
    //                playerScript.KBCounter = playerScript.KBTotalTime;

    //                // בדיקת כיוון הפגיעה
    //                playerScript.KnockFromRight = (player.transform.position.x <= transform.position.x); //if hit from the left go left and if right go right

    //                // שליחת נזק ל-HealthController
    //                playerHealthScript.takeDamage(damage1);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("NOT HIT");
    //        hitbox1 = false;
    //    }

    //    StartCoroutine(SpawnDelay(TimeAttack1));
    //}
}
