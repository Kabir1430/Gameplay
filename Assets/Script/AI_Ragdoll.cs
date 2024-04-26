 using UnityEngine;




public class AI_Ragdoll : MonoBehaviour
{

    [Header("Ragdoll")]
    [SerializeField]
    private Animator Animator;
    [SerializeField]
    private Transform RagdollRoot;
    [SerializeField]
    
    //private bool StartRagdoll = false;
    // Only public for Ragdoll Runtime GUI for explosive force
    public Rigidbody[] Rigidbodies;
    private CharacterJoint[] Joints;
    private Collider[] Colliders;

    [Header("Path")]
    public Transform[] waypoints;   // Array to hold the waypoint markers
    public float speed = 5f;        // Speed at which the AI moves

    private int currentWaypointIndex = 0;



    private void Awake()
    {
        Rigidbodies = RagdollRoot.GetComponentsInChildren<Rigidbody>();
        Joints = RagdollRoot.GetComponentsInChildren<CharacterJoint>();

        Colliders = RagdollRoot.GetComponentsInChildren<Collider>();
        EnableAnimator();
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
        if (collision.gameObject.tag == "BOX")
        {


            Debug.Log("BOX");
            EnableRagdoll();
        }
    }

    private void AIMove()
    {
        if (waypoints.Length == 0)
            return;

        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // If AI reaches the current waypoint, move to the next waypoint
        if (transform.position == targetPosition)
        {

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
    void Update()
    {

        AIMove();
    }
}



