using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ZombiePatrol : MonoBehaviour
{
    enum AIstate {  patrol, chase }
    AIstate aiState = AIstate.patrol;
    [SerializeField] GameObject[] waypoints;
    NavMeshAgent zombieAI;
    float nearWaypoint = 2f;
    [SerializeField] int presentWaypoint = 0;
    float currentSightAngle = 0f;
    Vector3 raycastDirection;
    float visionDistance = 12f; //zombies are blind from afar.
    float scanSpeed = 250f;
    float maxScanAngle = 125f; // sight angle...
                               // 
    GameObject playerTarget;

    // Start is called before the first frame update
    void Start()
    {
        zombieAI = GetComponent<NavMeshAgent>();

        if (aiState == AIstate.patrol)
        {
            GoToNextWaypoint();
        }
        
        //GoToNextWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (aiState == AIstate.patrol)
        {
            if (zombieAI.remainingDistance < nearWaypoint)
            {
                GoToNextWaypoint(); // Once in close enough proximity of a waypoint. It will move to the next one.
            }
        }
        else if(aiState == AIstate.chase)
        {
            zombieAI.SetDestination(playerTarget.transform.position);
        }
        VisionScan();
    }
    void GoToNextWaypoint()
    {
        presentWaypoint++;
        if (presentWaypoint >= waypoints.Length)
            presentWaypoint = 0;
        zombieAI.SetDestination(waypoints[presentWaypoint].transform.position);
    }

    void VisionScan()
    {
        currentSightAngle += scanSpeed * Time.deltaTime;
        currentSightAngle = currentSightAngle % maxScanAngle; // repeatedly oscialltes between the wto variables. 
        // make current sight scope change. 
        float angle = (currentSightAngle * 2) - maxScanAngle;
        raycastDirection = transform.TransformDirection(Quaternion.Euler(0, angle, 0) * Vector3.forward) * visionDistance;

        RaycastHit hit;

        if(Physics.Raycast(transform.position, raycastDirection, out hit, visionDistance) && hit.collider.tag == "Player")
        {
            Debug.DrawRay(transform.position, raycastDirection, Color.red);
            playerTarget = hit.transform.gameObject;
            aiState = AIstate.chase; 
        }
        else
        {
            Debug.DrawRay(transform.position, raycastDirection, Color.green);
        }

    }

    
}
