using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float maxSpeed = 5f;

    Rigidbody rb;

    Vector3 mDirection;

    float minInputVal = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        mDirection.x = Input.GetAxis("Horizontal");
        mDirection.z = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        if(mDirection.sqrMagnitude > minInputVal)
        {
            rb.AddForce(mDirection * moveSpeed, ForceMode.Impulse);
            if(rb.velocity.sqrMagnitude > maxSpeed *maxSpeed)
            {
                rb.velocity =  Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            }

        }
         rb.velocity  = rb.velocity * 0.9f;
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
