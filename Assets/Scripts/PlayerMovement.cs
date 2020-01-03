using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float baseMoveSpeed;
    private float updatedMoveSpeed;

    public Camera myCamera;
    public Rigidbody2D rb;
    public Animator anim;

    Vector2 movement;   // stores x (horiz) and y (vert)
    Vector2 mousePos;

    private void Start()
    {
        updatedMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        // Movement inputs
        movement.x = Input.GetAxisRaw("Horizontal");        // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");          // works default with WASD and arrow keys

        if (Input.GetKeyDown(KeyCode.LeftShift))            // press and hold shift to move faster
            updatedMoveSpeed = baseMoveSpeed * 0f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
            updatedMoveSpeed = baseMoveSpeed;

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Base Speed", updatedMoveSpeed);
        anim.SetFloat("Speed", movement.sqrMagnitude);      // sqrMag will always be pos, optimal since sqr root is unneeded

        if (movement.x >= 1)
        {
            anim.SetFloat("Facing Horizontal", 1);
            anim.SetFloat("Facing Vertical", 0);
        }
        else if (movement.x < 0)
        {
            anim.SetFloat("Facing Horizontal", -1);
            anim.SetFloat("Facing Vertical", 0);
        }

        if (movement.y >= 1)
        {
            anim.SetFloat("Facing Vertical", 1);
            anim.SetFloat("Facing Horizontal", 0);
        }
        else if (movement.y < 0)
        {
            anim.SetFloat("Facing Vertical", -1);
            anim.SetFloat("Facing Horizontal", 0);
        }

    }

    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
