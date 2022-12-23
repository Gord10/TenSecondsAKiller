using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Scientist : MonoBehaviour
{
    public float playerSpeed = 5f;
    public Sprite deadSprite;

    private NavMeshAgent navMeshAgent;
    private SpriteRenderer spriteRenderer;
    private new ParticleSystem particleSystem;
    
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
    private GameCamera gameCamera;
    private Animator animator;
    private new AudioSource audio;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        particleSystem = GetComponentInChildren<ParticleSystem>();

        gameCamera = FindObjectOfType<GameCamera>();

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        gameManager = FindObjectOfType<GameManager>();
        desiredPlayerDirection = new();
        
        if(state == State.POSSESSED)
        {
            GetPossessed();
        }
        else
        {
            GetUnpossessed();
        }
    }

    public void GetPossessed()
    {
        gameManager.possessedScientist = this;
        navMeshAgent.enabled = false;
        gameCamera.GetAttachedToScientist(this);
        rigidbody.isKinematic = false;
        state = State.POSSESSED;
        animator.SetBool("demon", true);
    }

    public void GetUnpossessed()
    {
        navMeshAgent.enabled = true;
        rigidbody.isKinematic = true;
        state = State.INNOCENT;
        animator.SetBool("running", true);
        animator.SetBool("demon", false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.POSSESSED && gameManager.IsInGame()) //This is the player "scientist". We will read the input.
        {
            desiredPlayerDirection.x = Input.GetAxis("Horizontal");
            desiredPlayerDirection.y = Input.GetAxis("Vertical");
            desiredPlayerDirection = Vector2.ClampMagnitude(desiredPlayerDirection, 1f);

            if(desiredPlayerDirection.x != 0)
            {
                spriteRenderer.flipX = desiredPlayerDirection.x > 0;
            }

            //Running animation will play as long as horizontal or vertical keys are pressed
            animator.SetBool("running", Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0);
        }
        else if(state == State.INNOCENT) //Scientist AI will run either to the exit door or from the player, depending on the distances between them
        {
            Vector3 target = new();
            Vector3 diffFromPlayer = transform.position - gameManager.possessedScientist.transform.position;
            Vector2 diffFromExit = gameManager.ExitDoor.position - transform.position;

            if(diffFromExit.sqrMagnitude > diffFromPlayer.sqrMagnitude) //Escape from player
            {
                //We will set the target position of the nav mesh as the opposite direction of player
                target = transform.position + diffFromPlayer.normalized * 3;
                navMeshAgent.SetDestination(target);
                spriteRenderer.flipX = diffFromPlayer.x > 0;
            }
            else //Run to the exit
            {
                navMeshAgent.SetDestination(gameManager.ExitDoor.position);
                spriteRenderer.flipX = diffFromExit.x > 0;
            }

            //Play crouching animation if the scientist has been cornered
            animator.SetBool("running", navMeshAgent.remainingDistance > 0.1f);
        }
    }

    public void StopMovement()
    {
        rigidbody.velocity = Vector2.zero;
        animator.SetBool("running", false);
    }

    private void FixedUpdate()
    {
        if(state == State.POSSESSED)
        {
            if(gameManager.IsInGame())
            {
                rigidbody.velocity = desiredPlayerDirection * playerSpeed;
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(state == State.DEAD)
        {
            return;
        }

        Scientist otherScientist = collision.gameObject.GetComponent<Scientist>();
        if(otherScientist) //Scientist touched another scientist
        {
            if(otherScientist.state == State.POSSESSED) //Innocent scientist has touched the demon, so he will die
            {
                Die();
            }
        }
    }

    private void Die()
    {
        gameManager.OnScientistKill();
        animator.enabled = false;
        spriteRenderer.sprite = deadSprite;
        GetComponent<Collider2D>().enabled = false;

        //We don't need nav mesh agent anymore, because scientist has died
        navMeshAgent.isStopped = true;
        Destroy(navMeshAgent);

        state = State.DEAD;
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;

        //Play sound with a randomized pitch
        audio.pitch = Random.Range(0.9f, 1.1f);
        audio.Play();

        //Play blood particle effect
        particleSystem.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(state == State.INNOCENT)
        {
            if (collision.CompareTag("Exit")) //AI scientist has reached the exit area 
            {
                gameManager.OnScientistRescue();
                Destroy(gameObject);
            }
        }
    }
}
