using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed;
    private float updatedMoveSpeed;
    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;   // stores x (horiz) and y (vert)

    private void Start()
    {
        updatedMoveSpeed = baseMoveSpeed;
    }

    void Update() {
        // Inputs
        movement.x = Input.GetAxisRaw("Horizontal");     // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");       // works default with WASD and arrow keys

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Base Speed", updatedMoveSpeed);    
        anim.SetFloat("Speed", movement.sqrMagnitude);    // sqrMag will always be pos, optimal since sqr root is unneeded

        if (Input.GetKeyDown(KeyCode.LeftShift))
            updatedMoveSpeed = baseMoveSpeed / 2;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            updatedMoveSpeed = baseMoveSpeed;
    }

    // Movement
    void FixedUpdate() {
        // Movement

        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
