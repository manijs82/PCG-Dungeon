using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    
    float horizontal;
    float vertical;
    float moveLimiter = 0.7f;
    
    public float runSpeed = 20.0f;
    
    void Start ()
    {
       body = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
       horizontal = Input.GetAxisRaw("Horizontal");
       vertical = Input.GetAxisRaw("Vertical");
       
    }

    private void FixedUpdate()
    {
        var moveDir = new Vector2(horizontal, vertical);
        moveDir.Normalize();
        var moveVector = moveDir * (runSpeed);

        body.velocity = moveVector;
    }
}
