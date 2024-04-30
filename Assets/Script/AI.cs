using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class AI : MonoBehaviour
{
    [Header("Move")]

    public Transform[] waypoints;
    // public Transform[] dynamicwaypoints;

    public float moveSpeed;        // Speed at which the AI moves
                                   //  public float Dash = 2.8f;        // Speed at which the AI moves
    public int currentIndex = 0;
    public Rigidbody rb;

    public float rotationSpeed;
    public Collider Capsuale;
    public float stoppingDistance;


    //public Vector3 Spawn_offset;
    [Header("Raycast")]

    public float maxDistance = 10f;
    public float verticalAngleOffset, S_verticalAngleOffset;
    public Vector3 positionOffset = new Vector3(0f, 1f, 0f);
    public LayerMask layerMask;

    [Header("Obstacle_Raycast")]
    public float MaxDistance = 10f;



    public Vector3 PositionOffset = new Vector3(0f, 1f, 0f); // Position offset
    public float VerticalAngleOffset = 30f        ;
    public float radius = 5f; // Radius of the half circle
    public int rayCount = 10;
    public float Angle;
    //public int maxIterations = 5; // Maximum number of raycast iterations//public float raycastDistance = 1f;
    [Header("Spawn")]
    public GameObject AI_Prefab, AI_Armature;

    public Transform SpawnPoint;

    public bool Spawn;
    [Header("Ragdoll")]
    [SerializeField]

    private Animator AI_Animator;
    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]

    //private bool StartRagdoll = false;
    // Only public for Ragdoll Runtime GUI for explosive force
    public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;


    [Header("Script")]
    public Manager Manager_Script;




    private void Awake()
    {

        AI_Armature.transform.position = transform.position;

        //  Manager_Script=GetComponent<Manager>();
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();

        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        EnableAnimator();
        Attach();

    }
    void Start()
    {
        Capsuale.enabled = true;
        rb = GetComponent<Rigidbody>();
        TransferArrayData();

        // dynamicwaypoints[0]= waypoints[0];
    }

    public void EnableRagdoll()
    {

        AI_Animator.enabled = false;
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
        AI_Animator.enabled = true;
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
        if (collision.gameObject.tag == "BOX")
        {


          //  Debug.Log("RagDoll ");
            EnableRagdoll();

            Capsuale.enabled = false;
            //   Detach();

            StartCoroutine(Spawning());
        }
    }

    /*private void OnTriggerStay()
    {
        if (Collider.gameObject.tag == "BOX")
        {
            Stop();
            Debug.Log("Box Collide");
        }

    }

    private void OnTriggerExit()
    {
        
            Debug.Log("Not Collide");
            Move();
    }*/
    IEnumerator Spawning()
    {
        yield return new WaitForSeconds(1.6f);
        ReSpawn();
        Destroy(gameObject);
    }

    void Detach()

    {
        GameObject parentObject = GameObject.Find("AI");

        // Detach all children from the parent
        foreach (Transform child in parentObject.transform)
        {
            child.SetParent(null); // Set the parent to null to detach it
        }
    }
    void Attach()
    {
        GameObject parentObject = GameObject.Find("AI");
        parentObject = AI_Armature.transform.parent.gameObject;
    }
    void TransferArrayData()
    {
        if (Manager_Script.Store.Length != waypoints.Length)
        {

            //Debug.LogError("Source and destination arrays must have the same length.");
            return;
        }


        // Print the names of destination objects (just for verification)
        //Debug.Log("Destination Array:");
        for (int i = 0; i < Manager_Script.Store.Length; i++)
        {

            waypoints[i] = Manager_Script.Store[i];
        }
        foreach (Transform obj in waypoints)
        {
         //    Debug.Log(obj.name);
        }


    }
    void ReSpawn()
    {
        //    if (Spawn)
        Instantiate(AI_Prefab, waypoints[currentIndex - 1].position, Quaternion.identity);
    }

    void Move()
    {
        //    moveSpeed = 2.6f;
        AI_Animator.SetBool("run", true);
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
    void Stop()
    {
        AI_Animator.SetBool("run", false);
        rb.velocity = Vector3.zero;

    }




    void Fov()
    {
        // Calculate the direction with the vertical angle offset

        Vector3 direction = Quaternion.Euler(0, 0f, verticalAngleOffset) * transform.forward;
        Vector3 S_direction = Quaternion.Euler(0, 0f, S_verticalAngleOffset) * transform.forward;

        // Add the position offset
        Vector3 rayOrigin = transform.position + positionOffset;
        Vector3 S_rayOrigin = transform.position + positionOffset;

        RaycastHit hit,S_Hit;

        // Perform the raycast
        if (Physics.Raycast(rayOrigin, direction, out hit, maxDistance,layerMask) || Physics.Raycast(S_rayOrigin, S_direction, out S_Hit, maxDistance,layerMask))
        {
            // If the ray hits something, draw the ray and log the hit position
            Debug.DrawRay(rayOrigin, direction * maxDistance, Color.red);
            Debug.DrawRay(S_rayOrigin, S_direction * maxDistance, Color.red);



            Stop();

          
        }
        else
        {
            Move();
            Debug.DrawRay(S_rayOrigin, S_direction * maxDistance, Color.green);
            // If the ray doesn't hit anything, draw it up to its max distance
            Debug.DrawRay(rayOrigin, direction * maxDistance, Color.green);
        }

    }

    
    void S_Fov()
    {
        bool obstacleDetected = false;
        float angleStep =   Angle/ (rayCount - 1);

        // Calculate the direction with the vertical angle offset
        Quaternion rotation = Quaternion.Euler(0, VerticalAngleOffset, 0f);

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of the ray
            Vector3 direction = rotation * Quaternion.Euler(0f, i * angleStep, 0f) * transform.forward;

            // Add the position offset
            Vector3 rayOrigin = transform.position + PositionOffset;

            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(rayOrigin, direction, out hit, radius,layerMask))
            {

                obstacleDetected = true;
                // If the ray hits something, draw the ray and log the hit position
                Debug.DrawRay(rayOrigin, direction * hit.distance, Color.red);
              //  Debug.Log("Object detected at: " + hit.point);

                // Respond to the detected object here

                // For demonstration, let's just draw a cyan sphere at the hit point
                Debug.DrawRay(hit.point, Vector3.up * 0.1f, Color.cyan);
            }
            else
            {

           
                // If no obstacle is hit, draw the ray up to its max distance
                Debug.DrawRay(rayOrigin, direction * radius, Color.green);
            }


            if (obstacleDetected)
            {
                Stop();
                // Respond to the detected obstacle here
                // For example, you can stop the object's movement or perform other actions
              //  Debug.Log("Obstacle detected. Stopping the object.");
            }
            else Move();
        }

    }

    void Update()
        { 
 
        if (currentIndex >= 2)
            S_Fov();
        else
            
            Fov();
if (currentIndex == 6)
        {
//            Text.text = "60";
  AI_Animator.SetBool("Dance", true);

           AI_Animator.SetBool("run", false);
        }

    }

}
