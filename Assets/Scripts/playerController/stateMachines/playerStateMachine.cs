using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStateMachine : stateMachine
//Inherits everything from the stateMachine class, as opposed to a monobehavior. Yet can still be stuck onto a game object
//Where all the values for the player are stored and set. Some in the inspector to be changed and altered through code as the player enters certain states
{
    //Input reader ref but as a property. We can GET the methods, but SET the states PRIVATELY 
    [field: Header("PlayerController / Camera base attributes")]
    public bool sprintToggle;

    [field: SerializeField] public inputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController charController { get; private set; }
    [field: SerializeField] public Rigidbody charRigidBody { get; set; }
    [field: SerializeField] public float freeRoamMovementSpeed { get; set; }
    [field: SerializeField] public float sprintSpeed { get; set; }
    [field: SerializeField] public float jumpForce { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public float RotationDamp { get; private set; }
    [field: SerializeField] public float boostDuration { get; private set; }
    [field: SerializeField] public float boostDistance { get; private set; }
    [field: SerializeField] public float boostCooldown { get; private set; }
    [field: SerializeField] public forceReciever ForceReciever { get; private set; }
    public Transform mainCameraTransform { get; private set; }
    public GameObject mainCamera;
    public GameObject boostCamera;
    public Transform Cam;

    //Inital fields used for wall run method
    [field: Header("Wall-Running attributes")]
    [field: SerializeField] public Transform orientation { get; set; }
    [field: SerializeField] public bool wallRunning { get; set; }
    [field: SerializeField] public float horizontalInput { get; set; }
    [field: SerializeField] public float verticalInput { get; set; }
    [field: SerializeField] public bool exitWall { get; set; }
    [field: SerializeField] public float exitWallTime { get; set; }
    [field: SerializeField] public float exitWallTimer { get; set; }
    [field: SerializeField] public LayerMask whatisWall { get; set; }
    [field: SerializeField] public LayerMask ground { get; set; }
    [field: SerializeField] public float wallrunForce { get; set; }
    [field: SerializeField] public float wallJumpUpForce { get; set; }
    [field: SerializeField] public float wallJumpUpSideForce { get; set; }
    [field: SerializeField] public float minJumpHeight { get; set; }
    [field: SerializeField] public float wallrunSpeed { get; set; }
    [field: SerializeField] public bool wallLeft { get; set; }
    [field: SerializeField] public bool wallRight { get; set; }
    [field: SerializeField] public float maxWallRunTimer { get; set; }
    [field: SerializeField] public float wallrunTimer { get; set; }
    [field: SerializeField] public float wallCheckDistance { get; set; }

    [field: Header("Grind pathing attributes")]
    [field: SerializeField] public RailScript gameObj_RailScript;
    [field: SerializeField] public bool onRail { get; set; }
    [field: SerializeField] public float grindSpeed { get; set; }
    [field: SerializeField] public float heightOffset { get; set; }
    [field: SerializeField] public float endOff { get; set; }
    [field: SerializeField] public float grindExitTime { get; set; }
    [field: SerializeField] public float grindExitTimer { get; set; }
    [field: SerializeField] public bool grindExit { get; set; }


    //Need to grab transform of camera, not the actual component itself

    public float previousBoostTime { get; private set; } = Mathf.NegativeInfinity;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //set the transform variable to our camera in scene (using Camera.main.transform)
        mainCameraTransform = Camera.main.transform;
        switchState(new playerFreeLookState(this));
    }


    //set boost time
    public void setBoostTime(float boostTime)
    {
        previousBoostTime = boostTime;
    }


    //Character controller collider hit to switch into grind pathing state 
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Rail"))
        {
            onRail = true;
            gameObj_RailScript = hit.gameObject.GetComponent<RailScript>();
            switchState(new playerGrindPathMovement(this));
        }
    }
}
