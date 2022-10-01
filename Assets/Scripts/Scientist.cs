using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    public float playerSpeed = 5f;

    public enum State
    {
        POSSESSED,
        RUNNING_FROM_DEMON,
        DEAD
    }

    public State state = State.RUNNING_FROM_DEMON;

    private new Rigidbody2D rigidbody;
    private Vector2 desiredPlayerDirection;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        desiredPlayerDirection = new();
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

    }

    private void FixedUpdate()
    {
        if(state == State.POSSESSED)
        {
            rigidbody.velocity = desiredPlayerDirection * playerSpeed;
        }
    }
}
