using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour 
{
    CharacterController npcController;
    Transform enemyPlayer;
    Vector3 viewPoint;
    Vector3 targetVector;
    Vector3 forwardVector;
    Vector3 lastPositionPlayer;
    AIPath pathAI;
    Vector3 move;
    Vector3 waypoint;

    int rotFlag = 0;
    float rotationTime = 1.0f;
    float startRotation;
    float rotateY = 0;

    float distance;
    float pickNextWaypoint = 2.0f;

    public float npcSpeed = 5.0f;
    float viewPointHeight = 1.65f;
    float distanceAggr = 50.0f;
    float visibilityRange = 120.0f;

    float lastSwipe;
    float swipeRange = 0.5f;

    bool inVisibilityRange = false;
    bool debug;

    public float hitPoints = 100.0f;
    public GameObject deadNinja;

	// Use this for initialization
    void Awake()
    {
        npcController = GetComponent("CharacterController") as CharacterController;
        enemyPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        pathAI = GetComponent<AIPath>();
        visibilityRange /= 2;
        lastPositionPlayer = Vector3.zero;
        pathAI.canMove = true;
    }
   
	
	// Update is called once per frame
	void Update () 
    {
        Debuging();
        if (enemyPlayer == null) return;
        viewPoint = new Vector3(transform.position.x,
            transform.position.y + viewPointHeight, transform.position.z);
	
        inVisibilityRange = DetermineInRange();
        if (enemyPlayer.transform.tag == "Player" && inVisibilityRange)
        {
            Hunting();
            lastPositionPlayer = enemyPlayer.position;
        }
        else if(!inVisibilityRange && lastPositionPlayer != Vector3.zero)
        {
            SearchEnemy();
        }
        else
        {
            Patrol();
        }  
	}

    public bool DetermineInRange()
    {
        Vector3 targetPosition = new Vector3(enemyPlayer.position.x,
            enemyPlayer.position.y + viewPointHeight, enemyPlayer.position.z);
        if(Physics.Linecast(viewPoint, targetPosition))
        {
            return false;
        }
        forwardVector = transform.TransformDirection(Vector3.forward);
        targetVector = targetPosition - transform.position;
        float angle = Vector3.Angle(forwardVector, targetVector);
        distance = Vector3.Distance(transform.position, enemyPlayer.position);
        if (angle < visibilityRange && distance <= distanceAggr)
        {
            return true;
        }
        else if (distance < 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void Hunting()
    {
        if(!pathAI.enabled)
            pathAI.enabled = true;
        rotFlag = 0;
        pathAI.target = enemyPlayer.position;
        distance = Vector3.Distance(transform.position, enemyPlayer.position);
        if (distance < 0.8f)
        {
            if (Time.time > lastSwipe + swipeRange)
            {
                BroadcastMessage("SwipeSwordAnim");
                BroadcastMessage("SwipeSwordSound");
                lastSwipe = Time.time;
            }
        }
        BroadcastMessage("WalkForward");
    }
    void SearchEnemy()
    {
        pathAI.target = lastPositionPlayer;
        float dist = Vector3.Distance(transform.position, lastPositionPlayer);
        if ( dist < 0.3f)
        {
            pathAI.enabled = false;
            BroadcastMessage("Idle");
            switch (rotFlag)
            {
                case 0:
                    rotateY = transform.eulerAngles.y + 90;
                    rotFlag = 1;
                    startRotation = Time.time;
                    break;
                case 1:
                    if (Time.time > startRotation + rotationTime)
                    {
                        rotateY = transform.eulerAngles.y - 180;
                        rotFlag = 2;
                        startRotation = Time.time;
                    }
                    break;
                case 2:
                    if(Time.time > startRotation + rotationTime * 2)
                    {
                        rotateY = transform.eulerAngles.y + 90;
                        rotFlag = 3;
                        startRotation = Time.time;
                    }
                    break;
                case 3:
                    if (Time.time > startRotation + rotationTime)
                    {
                        lastPositionPlayer = Vector3.zero;
                        pathAI.enabled = true;
                    }
                    break;
            }

            float rotation = Mathf.Lerp(transform.eulerAngles.y, rotateY, Time.deltaTime * rotationTime);
            transform.rotation = Quaternion.AngleAxis(rotation, Vector3.up);
        }
    }
    void Patrol()
    {
        if (waypoint == Vector3.zero)
        {
            waypoint = WayPoint.FindClosest(transform.position);
        }
        pathAI.target = waypoint;
        if (Vector3.Distance(transform.position, waypoint) < pickNextWaypoint)
        {
            int waypointIndex = WayPoint.waypoints.IndexOf(waypoint);
            if (waypointIndex != WayPoint.waypoints.Count - 1)
            {
                waypoint = WayPoint.waypoints[waypointIndex + 1];
            }
            else
            {
                waypoint = WayPoint.waypoints[0];
            }
        }
        BroadcastMessage("WalkForward");
    }
   
    void CauseDamage(float damage)
    {
        if (hitPoints <= 0.0f)
        {
            return;
        }
        hitPoints -= damage;
        if (hitPoints <= 0.0f)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x,
                transform.eulerAngles.y + 180, transform.eulerAngles.z);
            Destroy(gameObject);
            Instantiate(deadNinja, transform.position, transform.rotation);
        }

    }

    void Debuging()
    {
        if (enemyPlayer == null)
            return;
        Vector3 range = new Vector3(30, 0, 20);
        range = transform.TransformDirection(range);
        Vector3 _range = new Vector3(-30, 0, 20);
        _range = transform.TransformDirection(_range);
        Debug.DrawLine(viewPoint, enemyPlayer.transform.position + 
            new Vector3(0, viewPointHeight, 0), Color.green);
        Debug.DrawRay(viewPoint, range, Color.red);
        Debug.DrawRay(viewPoint, _range, Color.red);
    }
}

