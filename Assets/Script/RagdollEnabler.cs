using TMPro;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{

    [Header("Ragdoll")]
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]
    private bool StartRagdoll = false;
    // Only public for Ragdoll Runtime GUI for explosive force
    public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    [Header("Touch")]

      [SerializeField] bool holding = false;
    [SerializeField] Vector3 lastPosition;


    [Header("Character")]

    public Animator Anim;
    public Transform[] waypoints;
    public float moveSpeed = 5f;
    public float stoppingDistance = 0.1f;

    public int currentIndex = 0;
    public Rigidbody rb;
    public float rotationSpeed;


    [Header("Script")]
    public Manager Manager_Script;

    [Header("Score")]
    public TextMeshProUGUI Text;

//    public bool Big_View;
    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();
        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        if (StartRagdoll)
        {

           EnableRagdoll();
        }
        else
        {

           // EnableAnimator();
        }
        EnableAnimator();
    }
    private void Start()
    {
        EnableAnimator();
        
        rb.isKinematic = false;
        rb.useGravity = false;

    }

    public void EnableRagdoll()
    {
        Animator.enabled = false;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = true;
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = true;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.velocity = Vector3.zero;
            rigidbody.detectCollisions = true;
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;

        }
    }

    public void EnableAnimator()
    {
        Animator.enabled = true;
        foreach (CharacterJoint joint in Joints)
        {
            joint.enableCollision = false;

            
        }
        foreach (Collider collider in Colliders)
        {
            collider.enabled = false;
        }
        foreach (Rigidbody rigidbody in Rigidbodies)
        {
            rigidbody.detectCollisions = false;
            rigidbody.useGravity = false;

            rigidbody.isKinematic = true;
        }
    }





    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="BOX")
        {
            Debug.Log("BOX");
            EnableRagdoll();
        }
    }

    void Touch()
    {


        if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
        {
            holding = true;
            lastPosition = Input.mousePosition;
        
        }

        if (Input.GetMouseButtonUp(0))
        {
            holding = false;
        }

        if (holding)
        {
            Anim.SetBool("run", true);
            Debug.Log("Holding");
            //Vector3 currentPosition = Input.mousePosition;
            //Vector3 delta = currentPosition - lastPosition;
            // Adjust the movement sensitivity according to your needs
       
            // transform.Translate(delta * Time.deltaTime);
            //lastPosition = currentPosition;
        }
 
        else
        {
            Anim.SetBool("run", false);
        }

    }


     void Move()
    {
        
            if (currentIndex < waypoints.Length)
            {
                Vector3 targetPosition = waypoints[currentIndex].position;
                Vector3 moveDirection = (targetPosition - transform.position).normalized;

                // Move towards the target position
                rb.MovePosition(transform.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            // Rotate towards the target direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            }
            // Check if the player is close enough to the waypoint
            if (Vector3.Distance(transform.position, targetPosition) < stoppingDistance)
                {
                    currentIndex++;
                }
            }
        
    }

    private void Update()
    {
        Touch();
       ScoreUp();
        //Move();
    }


    private void FixedUpdate()
    {
        if(holding)
        Move();
    }

    void ScoreUp()
    {
   
        if(currentIndex==1)
        {
            Text.text = "10";
        }
        else if(currentIndex == 2)
            {
                Text.text = "20";
            }
        else if (currentIndex == 3)
        {
            Text.text = "30";
        }
        else if (currentIndex == 4)
        {
            Text.text = "40";
        }
        else if (currentIndex == 6)
        {
            Text.text = "60";
            Anim.SetBool("Dance", true);
           
            Anim.SetBool("run", false);
        }

    }




}
