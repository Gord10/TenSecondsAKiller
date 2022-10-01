using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scientist : MonoBehaviour
{
    public float playerSpeed = 5f;
    public Color possessedColor = Color.red;
    private NavMeshAgent navMeshAgent;
    private SpriteRenderer spriteRenderer;

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
        spriteRenderer = GetComponent<SpriteRenderer>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        gameManager = FindObjectOfType<GameManager>();
        desiredPlayerDirection = new();
        GetUnpossessed();
    }

    public void GetPossessed()
    {
        navMeshAgent.enabled = false;
        gameManager.possessedScientist = this;
        rigidbody.isKinematic = false;
        spriteRenderer.color = possessedColor;
        state = State.POSSESSED;
    }

    public void GetUnpossessed()
    {
        navMeshAgent.enabled = true;
        rigidbody.isKinematic = true;
        spriteRenderer.color = Color.white;
        state = State.INNOCENT;
    }

    // Start is called before the first frame update
    void Start()
    {

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Scientist scientist = collision.gameObject.GetComponent<Scientist>();
        if(scientist)
        {
            if(scientist.state == State.POSSESSED)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(state == State.INNOCENT)
        {
            if (collision.CompareTag("Exit"))
            {
                Destroy(gameObject);
            }
        }
    }
}
