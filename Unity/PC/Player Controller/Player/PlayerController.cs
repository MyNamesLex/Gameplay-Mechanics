using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    public Camera Cam;
    private const float YMin = -50.0f;
    private const float YMax = 50.0f;

    public Transform lookAt;

    public float distance;
    private float currentX;
    private float currentY;
    public float sensivity;

    [Header("Stat Canvas")]
    public StatCanvas sc;

    [Header("State Booleans")]
    public bool OnGround;
    public bool isSprinting;
    public bool isSliding;
    public bool isMoving;
    public bool CoyoteTime;
    public bool isJumping;
    public bool isAirborne;
    public bool isWallRunning;
    public bool JumpBuffer;
    public bool isCrouching;
    public bool isGroundSlamming;

    [Header("Sliding Booleans")]
    public bool InitialSlideBoost;
    public bool RequestedSlideInAir;

    [Header("Sprint Change Booleans")]
    public bool RequestedSprintChangeWhilstInAir;

    [Header("Coyote Booleans")]
    public bool TriggeredCoyote;

    [Header("WallRun Booleans")]
    public bool isWallRight;
    public bool isWallLeft;
    public bool StopWallRunJumped;

    [Header("Ground Slam Variables")]
    public BoxCollider GroundSlamDetect;
    public float GroundSlamDamage;
    public float GroundSlamPlayerWeightMultiplier;

    [Header("Weapons")]
    public float CurrentEquipped; // 1 = pistol | 2 = shotgun | 3 = knife

    [Header("Pistol")]
    public GameObject PistolObj;

    [Header("Shotgun")]
    public GameObject ShotgunObj;

    [Header("Knife")]
    public GameObject KnifeObj;

    [Header("Movement")]

    //Walk/Run//

    public Rigidbody rb;
    public float MaxSpeed;
    public float SprintSpeed;
    public float CrouchSpeed;
    public float CrouchSprintSpeed;
    public float YVelocityLimit;
    //Slide//

    public float SlideForce;
    public float SlideMovementDecrease;

    //Jump//

    public float UpwardsMass;
    public float DownwardsMass;
    public float JumpForce;

    //Jump Buffer//

    public float GroundLength;
    public float JumpBufferLength;

    //Wallrun//

    public float WallRunForce;
    public float MaxWallRunTime;
    private float OGMaxWallRunTime;
    public float MaxWallSpeed;
    public float WallRunJumpX;
    public float WallRunJumpY;
    public float WallRunJumpZ;

    //Detection//

    public LayerMask WhatIsWall;
    public LayerMask WhatIsFloor;

    [Header("Timers")]
    public float SlideTimerFloat;
    private float OGSlideTimerFloat;
    public float CoyoteTimerFloat;
    private float OGCoyoteTimerFloat;

    [Header("Input Floats")]
    public float VerticalInput;
    public float HorizontalInput;
    public Vector3 Movement;

    [Header("Controls")]
    public KeyCode SprintKey;
    public KeyCode JumpKey;
    public KeyCode SlideKey;
    public KeyCode CrouchKey;
    public KeyCode GroundSlamKey;
    public KeyCode PistolEquipKey;
    public KeyCode ShotgunEquipKey;
    public KeyCode KnifeEquipKey;

    // TODO //

    // COYOTE TIME (DONE)
    // SLIDE (DONE?)
    // UPWARDS/DOWNARDS GRAVITY JUMP CHANGES (DONE)
    // SLIDY CONTROLS FIX (DONE)
    // WALLRUN (DONE?)
    // JUMP BUFFER (DONE?)

    // Start is called before the first frame update
    void Start()
    {
        OGCoyoteTimerFloat = CoyoteTimerFloat;
        OGSlideTimerFloat = SlideTimerFloat;
        OGMaxWallRunTime = MaxWallRunTime;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        CurrentEquippedWeapon();
    }

    private void FixedUpdate()
    {
        // CAMERA //

        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Cam.transform.position = lookAt.position + rotation * Direction;
        Cam.transform.LookAt(lookAt.position);

        // COYOTE TIME //

        if(isAirborne && TriggeredCoyote == false && isJumping == false)
        {
            CoyoteTimeFunction();
            //Debug.Log("Coyote Time Started");
        }

        if (OnGround && TriggeredCoyote == true)
        {
            CoyoteTime = false;
            TriggeredCoyote = false;
            CoyoteTimerFloat = OGCoyoteTimerFloat;
            //Debug.Log("Coyote Time Ended");
        }

        // MOVEMENT //

        HorizontalInput = Input.GetAxisRaw("Horizontal") * MaxSpeed;
        VerticalInput = Input.GetAxisRaw("Vertical") * MaxSpeed;

        if (OnGround && RequestedSprintChangeWhilstInAir)
        {
            isSprinting = false;
            RequestedSprintChangeWhilstInAir = false;
        }

        Movement = Cam.transform.right * HorizontalInput + Cam.transform.forward * VerticalInput;
        Movement.y = 0f;

        if (RequestedSlideInAir)
        {
            if (OnGround)
            {
                RequestedSlideInAir = false;
            }
        }

        // CHANGE VELOCITY

        Vector2 v2 = new Vector2(rb.velocity.x, rb.velocity.z);
        Vector3 ClampedSpeed = new Vector3(0,0,0);

        if (isSprinting && !isCrouching) // sprinting
        {
            ClampedSpeed = Vector3.ClampMagnitude(v2, SprintSpeed);
        }

        if(!isSprinting && !isCrouching) // walking
        {
            ClampedSpeed = Vector3.ClampMagnitude(v2, MaxSpeed);
        }

        if (isCrouching && !isSprinting) // crouching
        {
            ClampedSpeed = Vector3.ClampMagnitude(v2, CrouchSpeed);
        }

        if(isCrouching && isSprinting) // crouch sprint
        {
            ClampedSpeed = Vector3.ClampMagnitude(v2, CrouchSprintSpeed);
        }

        if(isWallRunning && isSprinting) // wallrunning and sprinting
        {
            ClampedSpeed = Vector3.ClampMagnitude(v2, MaxWallSpeed);
        }

        rb.velocity = new Vector3(ClampedSpeed.x, rb.velocity.y, ClampedSpeed.y);

        // STOP CLIPPING THROUGH FLOOR IF GOING TOO FAST
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, YVelocityLimit);

        // JUMPING //

        if (OnGround && isJumping == true)
        {
            StartCoroutine(JumpTimer());
        }

        // UPWARDS/DOWNWARDS GRAVITY

        if(!OnGround)
        {
            isAirborne = true;
        }

        if(OnGround)
        {
            isAirborne = false;
            if(StopWallRunJumped)
            {
                StopWallRunJumped = false;
            }
        }

        if(isAirborne && rb.velocity.y > 0 && !isWallRunning) // UPWARDS GRAVITY
        {
            Vector3 vel = rb.velocity;
            vel.y -= UpwardsMass;
            rb.velocity = vel;
            //Debug.Log("up");
        }

        if (isAirborne && rb.velocity.y < 0 && !isWallRunning) // DOWNWARDS GRAVITY
        {
            Vector3 vel = rb.velocity;
            vel.y -= DownwardsMass;
            rb.velocity = vel;
            //Debug.Log("down");
        }

        // NOT SLIDING INPUT SPEEDS
        if (isSprinting && isSliding == false && !isCrouching && !isWallRunning) // sprint
        {
            rb.AddForce(Movement * SprintSpeed, ForceMode.Impulse);
        }
        if (isSprinting && isSliding == false && !isCrouching && isWallRunning) // sprint & wallrunning
        {
            rb.AddForce(Movement * MaxWallSpeed, ForceMode.Impulse);
        }
        if (isSprinting == false && isSliding == false && !isCrouching) // walk
        {
            rb.AddForce(Movement * MaxSpeed, ForceMode.Impulse);
        }
        if(isSprinting == false && isSliding == false && isCrouching) // crouching
        {
            rb.AddForce(Movement * CrouchSpeed, ForceMode.Impulse);
        }
        if(isSprinting && isSliding == false && isCrouching) // crouch sprint
        {
            rb.AddForce(Movement * CrouchSprintSpeed, ForceMode.Impulse);
        }

        // SLIDING //

        // SLIDING INPUT SPEEDS
        if (isSprinting && isSliding && InitialSlideBoost == false && !isAirborne)
        {
            rb.AddForce(Movement * SprintSpeed / SlideMovementDecrease, ForceMode.Impulse);
        }
        else if (isSprinting == false && isSliding && InitialSlideBoost == false && !isAirborne)
        {
            rb.AddForce(Movement * MaxSpeed / SlideMovementDecrease, ForceMode.Impulse);
        }
        if (isSliding && !RequestedSlideInAir)
        {
            SlideTimerFunction();
        }

        // ROTATION //

        Quaternion CamRotation = Cam.transform.rotation;
        CamRotation.x = 0f;
        CamRotation.z = 0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // GROUND CHECK //

        Debug.DrawRay(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z), Vector3.down, Color.green, 10f);
        OnGround = Physics.Raycast((new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.8f, gameObject.transform.position.z)), Vector3.down, GroundLength, WhatIsFloor);
        JumpBuffer = Physics.Raycast((new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 1f, gameObject.transform.position.z)), Vector3.down, JumpBufferLength, WhatIsFloor);

        // CONTROLS //

        // SPRINTING //

        if (Input.GetKey(SprintKey) && OnGround && isSprinting == false || Input.GetKey(SprintKey) && isWallRunning && isSprinting == false)
        {
            isSprinting = true;
        }

        if (Input.GetKeyUp(SprintKey))
        {
            if (!OnGround && !isWallRunning)
            {
                RequestedSprintChangeWhilstInAir = true;
            }
            else
            {
                isSprinting = false;
            }
        }

        // JUMP //

        if(Input.GetKeyDown(JumpKey) && OnGround && isJumping == false && !isWallRunning)
        {
            Jump();
            //Debug.Log("Jumped");
        }

        // COYOTE JUMP //

        if (Input.GetKeyDown(JumpKey) && CoyoteTime && isJumping == false && !isWallRunning)
        {
            Jump();
            CoyoteTimerFloat = OGCoyoteTimerFloat;
            //Debug.Log("Coyote Jump");
        }

        // JUMP BUFFER //

        if (Input.GetKeyDown(JumpKey) && JumpBuffer && isJumping == false && CoyoteTime == false && !isWallRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Jump();
            //Debug.Log("Jump Buffer Jump");
        }

        // SLIDING //

        if (Input.GetKeyDown(SlideKey) && isSliding == false && isMoving && Input.GetKey(KeyCode.W) && !isWallRunning && !isCrouching)
        {
            StartSliding();
        }

        if (Input.GetKeyUp(SlideKey) && isSliding)
        {
            StopSliding();
        }

        // GROUND SLAM //
        if((Input.GetKeyDown(GroundSlamKey) && isGroundSlamming == false && isMoving && !isWallRunning && !isCrouching) && isAirborne)
        {
            StartGroundSlam();
        }

        if (Input.GetKeyUp(GroundSlamKey) && isGroundSlamming)
        {
            StopGroundSlam();
        }

        // CROUCH //

        if (Input.GetKeyDown(CrouchKey) && !isCrouching && !isSliding)
        {
            StartCrouch();
        }
        if (Input.GetKeyUp(CrouchKey) && isCrouching)
        {
            StopCrouch();
        }

        // WALLRUN //

        CheckForWall();
        WallRunInput();

        // CHANGE WEAPON //

        if(Input.GetKeyDown(PistolEquipKey))
        {
            CurrentEquipped = 1;
            CurrentEquippedWeapon();
        }

        if (Input.GetKeyDown(ShotgunEquipKey))
        {
            CurrentEquipped = 2;
            CurrentEquippedWeapon();
        }

        if (Input.GetKeyDown(KnifeEquipKey))
        {
            CurrentEquipped = 3;
            CurrentEquippedWeapon();
        }

        // BOOL UPDATES //
        // MOVEMENT BOOL CHANGE //

        if (HorizontalInput == 0 && VerticalInput == 0 && !isAirborne || isWallRunning && HorizontalInput == 0 && VerticalInput == 0)
        {
            isMoving = false;
        }
        else if(isMoving == false)
        {
            isMoving = true;
        }
    }

    // JUMP //

    public void Jump()
    {
        rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
        isJumping = true;
    }

    // SLAM FUNCTION //

    public void StartGroundSlam()
    {
        isGroundSlamming = true;
        GroundSlamDetect.enabled = true;
        rb.drag -= GroundSlamPlayerWeightMultiplier;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z / 2);
    }

    public void StopGroundSlam()
    {
        isGroundSlamming = false;
        GroundSlamDetect.enabled = false;
        rb.drag += GroundSlamPlayerWeightMultiplier;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2, gameObject.transform.localScale.z * 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().TakeDamage(GroundSlamDamage);
        }
    }

    // SLIDING FUNCTIONS //

    public void SlideTimerFunction()
    {
        if (isAirborne)
        {
            RequestedSlideInAir = true;
        }
        else
        {
            if (SlideTimerFloat > 0)
            {
                InitialSlideBoost = true;
                rb.AddForce(transform.forward * SlideForce, ForceMode.Impulse);
                SlideTimerFloat -= Time.deltaTime;
            }
            else
            {
                InitialSlideBoost = false;
            }
        }
    }

    public void StartSliding()
    {
        isSliding = true;

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z / 2);
    }

    public void StopSliding()
    {
        isSliding = false;

        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2, gameObject.transform.localScale.z * 2);
        SlideTimerFloat = OGSlideTimerFloat;
    }

    // COYOTE TIME FUNCTION //

    public void CoyoteTimeFunction()
    {
        if(CoyoteTimerFloat > 0)
        {
            CoyoteTime = true;
            CoyoteTimerFloat -= Time.deltaTime;
        }
        else if(TriggeredCoyote == false)
        {
            TriggeredCoyote = true;
            CoyoteTime = false;
            CoyoteTimerFloat = OGCoyoteTimerFloat;
        }
    }

    // JUMP DETECTION //

    public IEnumerator JumpTimer()
    {
        yield return new WaitForSeconds(.1f);

        if (OnGround && isJumping == true)
            isJumping = false;

        yield return null;
    }

    // WEAPON/SHOOT FUNCTIONS //

    public void CurrentEquippedWeapon()
    {
        switch(CurrentEquipped)
        {
            case 1: // pistol
                PistolObj.SetActive(true);
                ShotgunObj.SetActive(false);
                KnifeObj.SetActive(false);
                sc.CurrentEquipped.text = "Pistol Equipped";
                break;
            case 2: // shotgun
                PistolObj.SetActive(false);
                ShotgunObj.SetActive(true);
                KnifeObj.SetActive(false);
                sc.CurrentEquipped.text = "Shotgun Equipped";
                break;
            case 3: // knife
                PistolObj.SetActive(false);
                ShotgunObj.SetActive(false);
                KnifeObj.SetActive(true);
                sc.CurrentEquipped.text = "Knife Equipped";
                break;
        }
    }
    public void Shoot()
    {

    }

    // WALLRUN FUNCTIONS //

    public void WallRunInput()
    {
        if(Input.GetKey(KeyCode.D) && isWallRight && isMoving && isJumping == true && StopWallRunJumped == false)
        {
            StartWallRun();
        }

        if (Input.GetKey(KeyCode.A) && isWallLeft && isMoving && isJumping == true && StopWallRunJumped == false)
        {
            StartWallRun();
        }

        if(!isMoving && isWallRunning)
        {
            StopWallRun();
            //Debug.Log("Not Moving, stopping wallrun");
        }

        if(isJumping == false && Input.GetKeyDown(JumpKey) && isWallRunning)
        {
            WallRunJump();
        }

        if (isWallRunning)
        {
            if (MaxWallRunTime > 0)
            {
                MaxWallRunTime -= Time.deltaTime;
            }
            else
            {
                StopWallRun();
            }
        }

        if (isJumping)
        {
            StopWallRun();
        }

        if(StopWallRunJumped)
        {
            StopWallRun();
        }
    }

    public void StartWallRun()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        rb.useGravity = false;
        isWallRunning = true;
        isJumping = false;

        if (rb.velocity.magnitude <= MaxWallSpeed)
        {
            rb.AddForce(transform.forward * WallRunForce * Time.deltaTime);

            if(isWallRight)
            {
                rb.AddForce(transform.right * WallRunForce / 5 * Time.deltaTime);
            }
            else
            {
                rb.AddForce(-transform.right * WallRunForce / 5 * Time.deltaTime);
            }
        }
    }

    public void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;
        MaxWallRunTime = OGMaxWallRunTime;
    }

    public void CheckForWall()
    {
        Debug.DrawRay(transform.position, transform.right, Color.black);
        Debug.DrawRay(transform.position, -transform.right, Color.black);

        isWallRight = Physics.Raycast(transform.position, transform.right, 1.5f, WhatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -transform.right, 1.5f, WhatIsWall);

        if (!isWallRight && !isWallLeft)
        {
            StopWallRun();
        }
    }

    public void WallRunJump()
    {
        if (isWallLeft && !Input.GetKey(KeyCode.D) && isJumping == false || isWallRight && !Input.GetKey(KeyCode.A) && isJumping == false) // causes infinite jump scaling
        {
            rb.AddForce(new Vector3(WallRunJumpX, WallRunJumpY, WallRunJumpZ), ForceMode.Impulse);
            StopWallRunJumped = true;
            // Debug.Log("Vertical Jump");
        }

        if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) && isJumping == false || Input.GetKey(KeyCode.D) && isJumping == false)
        {
            rb.AddForce(new Vector3(WallRunJumpX, WallRunJumpY, WallRunJumpZ), ForceMode.Impulse);
            //Debug.Log("Jump Into Wall");
        }

        if (isWallRight && Input.GetKey(KeyCode.A) && isJumping == false)
        {
            rb.AddForce(new Vector3(-WallRunJumpX, WallRunJumpY, WallRunJumpZ), ForceMode.Impulse);
            //Debug.Log("Left Hop");
        }

        if (isWallLeft && Input.GetKey(KeyCode.D) && isJumping == false)
        {
            rb.AddForce(new Vector3(WallRunJumpX, WallRunJumpY, WallRunJumpZ), ForceMode.Impulse);
            //Debug.Log("Right Hop");
        }

        rb.AddForce(transform.forward * JumpForce * 1f);
        isJumping = true;
        StopWallRun();
    }

    // CROUCH FUNCTIONS //

    public void StartCrouch()
    {
        isCrouching = true;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x / 2, gameObject.transform.localScale.y / 2, gameObject.transform.localScale.z / 2);
    }

    public void StopCrouch()
    {
        isCrouching = false;
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x * 2, gameObject.transform.localScale.y * 2, gameObject.transform.localScale.z * 2);
    }
}