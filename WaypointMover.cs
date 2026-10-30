using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentWaypointIndex;
    private bool isWaiting;

    private bool isWalking;
    private Animator animator;

    private float lastInputX;
    private float lastInputY;

    void Start()
    {
        animator = GetComponent<Animator>();
        waypoints = new Transform[waypointParent.childCount];

        for (int i = 0; i < waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    void Update()
    {
        if (PauseController.IsGamePaused || isWaiting)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("InputX", lastInputX);
            animator.SetFloat("InputY", lastInputY);
            
            return;
        }

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (transform.position - target.position).normalized * -1;

        if (direction.magnitude > 0f)
        {
            lastInputX = direction.x;
            lastInputY = direction.y;
        }
        
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);
        animator.SetBool("isWalking", direction.magnitude > 0f);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        animator.SetBool("isWalking", false);

        animator.SetFloat("lastInputX", lastInputX);
        animator.SetFloat("lastInputY", lastInputY);

        yield return new WaitForSeconds(waitTime);

        currentWaypointIndex = loopWaypoints ? (currentWaypointIndex + 1) % waypoints.Length : Mathf.Min(currentWaypointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}
