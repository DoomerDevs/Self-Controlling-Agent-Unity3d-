using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject[] wayPoints; //array of waypoints

    int currentWp = 0;
    public float speed = 10.0f;
    public float rotSpeed = 5.0f;


    //See player
    public float lookRadius = 5.0f;


    Transform target; //player
    NavMeshAgent agent; //enemy bot

    
    void Start()
    {
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance <= lookRadius) 
        {
            agent.SetDestination(target.position); //brings enemy to the target(player) position

            if (distance <= agent.stoppingDistance)
            {
                //face target
                FaceTarget();
            }
        
        }

        if (distance > lookRadius)  //moves between waypoints, doesnt follow player
        {
            if (Vector3.Distance(this.transform.position, wayPoints[currentWp].transform.position) <= 3)
            {
                currentWp++; //we increment waypoint
            }
            if (currentWp >= wayPoints.Length)
            {
                currentWp = 0;
            }

            Quaternion lookAtWp = Quaternion.LookRotation(wayPoints[currentWp].transform.position - this.transform.position);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, lookAtWp, rotSpeed * Time.deltaTime);
            this.transform.Translate(0, 0, speed * Time.deltaTime);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}
