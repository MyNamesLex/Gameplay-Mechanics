using System.Collections;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
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

    [Header("Movement")]
    public Rigidbody rb;
    public float speed;
    public float VerticalSpeed;
    public float HorizontalSpeed;
    public Vector3 Movement;
    public float VelocityLimit;

    [Header("States")]
    public bool isMoving;
    public bool CanUseSword;
    public bool Block;
    public bool Swinging;
    public bool Stunned;
    public bool KnockedBack;

    [Header("Sword Object")]
    public GameObject Sword;

    [Header("Sword Stats")]
    public float RateOfSwing;
    public float SwordRange;
    public LayerMask EnemyMask;
    public float SwordKBBack;
    public float SwordKBVert;
    public float PlayerSwordKBBack;
    public float PlayerSwordKBVert;

    [Header("Timers")]
    public float StunnedTimer;
    public float KnockbackTimer;
    private float OGKnockBackTimer;

    [Header("Popup")]
    public Popup popup;

    // Start is called before the first frame update
    void Start()
    {
        OGKnockBackTimer = KnockbackTimer;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // CAMERA //

        currentX += Input.GetAxis("Mouse X") * sensivity * Time.deltaTime;
        currentY += Input.GetAxis("Mouse Y") * sensivity * Time.deltaTime;

        currentY = Mathf.Clamp(currentY, YMin, YMax);

        Vector3 Direction = new Vector3(0, 0, -distance);

        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);

        Cam.transform.position = lookAt.position + rotation * Direction;
        Cam.transform.LookAt(lookAt.position);

        // ROTATION

        Quaternion CamRotation = Cam.transform.rotation;
        CamRotation.x = 0f;
        CamRotation.z = 0f;

        transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);

        // MOVEMENT //

        // GET INPUT

        if (Stunned == false)
        {
            HorizontalSpeed = Input.GetAxisRaw("Horizontal") * speed;
            VerticalSpeed = Input.GetAxisRaw("Vertical") * speed;

            Movement = Cam.transform.right * HorizontalSpeed + Cam.transform.forward * VerticalSpeed;
            Movement.y = 0f;
        }
        else if (Stunned == true)
        {
            HorizontalSpeed = 0;
            VerticalSpeed = 0;
            Movement = new Vector3(0, 0, 0);
        }

        // LIMIT SPEED

        Vector3 ClampedSpeed = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S) && Stunned == false)
        {
            Vector2 v2 = new Vector2(rb.velocity.x, rb.velocity.z);
            ClampedSpeed = Vector3.ClampMagnitude(v2, speed);

            //Debug.Log(rb.velocity);
            //Debug.Log(rb.velocity.magnitude);

            // ADD MOVEMENT FORCE
            rb.AddForce(Movement * speed, ForceMode.Impulse);
        }


        rb.velocity = new Vector3(ClampedSpeed.x, rb.velocity.y, ClampedSpeed.y);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, VelocityLimit);

        // KNOCKBACK

        if (KnockedBack)
        {
            KBFunction();
        }
    }

    private void Update()
    {
        if (Stunned == false)
        {
            if (Input.GetMouseButton(0) && CanUseSword) // swing sword
            {
                StartCoroutine(SwordCoroutine());
            }

            if (Input.GetMouseButton(1) && CanUseSword) // blocking
            {
                BlockFunction();
            }

            if (Input.GetMouseButtonUp(1) && CanUseSword == false) // stop blocking
            {
                CanUseSword = true;
                Block = false;
            }
        }
    }

    public void BlockFunction()
    {
        CanUseSword = false;
        Block = true;
    }

    public IEnumerator SwordCoroutine()
    {
        CanUseSword = false;
        Swinging = true;
        SwordFunction();
        //Debug.Log("Swung");
        yield return new WaitForSeconds(RateOfSwing);
        Swinging = false;
        CanUseSword = true;
    }

    public void SwordFunction()
    {
        RaycastHit hit;
        //Debug.DrawRay(Cam.transform.position, Cam.transform.forward, Color.green);
        if (Physics.Raycast(Cam.transform.position, Cam.transform.forward, out hit, SwordRange, EnemyMask))
        {
            if (hit.transform.GetComponent<TestEnemy>().Blocking)
            {
                Stun();
            }
            else
            {
                KnockbackEnemy(hit.transform);
                StunEnemy(hit.transform);
            }
        }
    }

    public void KnockbackEnemy(Transform Enemy)
    {
        Enemy.GetComponent<Rigidbody>().AddForce(transform.forward * SwordKBBack, ForceMode.VelocityChange);
        Enemy.GetComponent<Rigidbody>().AddForce(transform.up * SwordKBVert, ForceMode.VelocityChange);
        // Debug.Log("hit enemy");
    }

    public void StunEnemy(Transform Enemy)
    {
        Enemy.GetComponent<TestEnemy>().StartCoroutine(Enemy.GetComponent<TestEnemy>().StunnedFunction());
    }

    public void Stun()
    {
        popup.Popups("Player Stunned, enemy was blocking and player swung");
        StartCoroutine(StunnedFunction());
        KnockedBack = true; // calls KBFunction in FixedUpdate, KnockedBack gets set to false there too
        rb.AddForce(transform.up * PlayerSwordKBVert, ForceMode.VelocityChange);
    }
    public void KBFunction()
    {
        if (KnockbackTimer > 0)
        {
            rb.AddForce(-transform.forward * PlayerSwordKBBack * Time.deltaTime, ForceMode.VelocityChange);
            KnockbackTimer -= Time.deltaTime;
        }
        else
        {
            KnockbackTimer = OGKnockBackTimer;
            KnockedBack = false;
        }
    }

    public IEnumerator StunnedFunction()
    {
        //Debug.Log("stunned");
        CanUseSword = false;
        Stunned = true;
        yield return new WaitForSeconds(StunnedTimer);
        Stunned = false;
        CanUseSword = true;
    }
}
