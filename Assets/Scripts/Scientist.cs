using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scientist : MonoBehaviour
{
    public float playerSpeed = 5f;
    private NavMeshAgent navMeshAgent;

    public enum State
    {
        POSSESSED,
        INNOCENT,
        DEAD
    }

    public State state = State.INNOCENT;

    private new Rigidbody2D rigidbody;
    private Vector2 desiredPlayerDirection;
    private GameManager gameManager;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        gameManager = FindObjectOfType<GameManager>();

        if(state == State.POSSESSED)
        {
            navMeshAgent.enabled = false;
            gameManager.possessedScientist = this;
        }

        rigidbody.isKinematic = state == State.INNOCENT;

        desiredPlayerDirection = new();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(state == State.INNOCENT)
        {
            Vector3 target = new();
            Vector3 direction = gameManager.possessedScientist.transform.position - transform.position;
            target = transform.position + direction.normalized;
            navMeshAgent.SetDestination(target);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.POSSESSED)
        {
            desiredPlayerDirection.x = Input.GetAxis("Horizontal");
            desiredPlayerDirection.y = Input.GetAxis("Vertical");
            desiredPlayerDirection = Vector2.ClampMagnitude(desiredPlayerDirection, 1f);
        }
        else if(state == State.INNOCENT)
        {
            Vector3 target = new();
            Vector3 diffFromPlayer = transform.position - gameManager.possessedScientist.transform.position;
            Vector2 diffFromExit = gameManager.exitDoor.position - transform.position;

            if(diffFromExit.sqrMagnitude > diffFromPlayer.sqrMagnitude)
            {
                target = transform.position + diffFromPlayer.normalized;
                navMeshAgent.SetDestination(target);
            }
            else
            {
                navMeshAgent.SetDestination(gameManager.exitDoor.position);
            }
        }
    }

    private void FixedUpdate()
    {
        if(state == State.POSSESSED)
        {
            rigidbody.velocity = desiredPlayerDirection * playerSpeed;
        }
    }
}
