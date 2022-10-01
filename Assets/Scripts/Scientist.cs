using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : MonoBehaviour
{
    public float playerSpeed = 5f;
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
        desiredPlayerDirection.x = Input.GetAxis("Horizontal");
        desiredPlayerDirection.y = Input.GetAxis("Vertical");
        desiredPlayerDirection = Vector2.ClampMagnitude(desiredPlayerDirection, 1f);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = desiredPlayerDirection * playerSpeed;
    }
}
