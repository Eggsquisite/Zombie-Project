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

        mousePos = myCamera.ScreenToWorldPoint(Input.mousePosition);
      
            if (mousePos.x <= rb.position.x)
            {   // mouse is pointed LEFT, aim LEFT
                anim.SetFloat("Facing Horizontal", -1);
                
            }
            else if (mousePos.x > rb.position.x)
            {   // mouse is pointed RIGHT, aim RIGHT
                anim.SetFloat("Facing Horizontal", 1);

            }
            else if (mousePos.y > rb.position.y)
            {   // mouse is pointed UP, aim UP
                anim.SetFloat("Facing Vertical", mousePos.y);

            }
            else if (mousePos.y <= rb.position.y)
            {   // mouse is pointed DOWN, aim DOWN
                anim.SetFloat("Facing Vertical", mousePos.y);

            }

        
    }

    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;   // vector pointing towards mousePos
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;      // Gets the correct angle of mouse position relating to player model
        
        
    }
}
