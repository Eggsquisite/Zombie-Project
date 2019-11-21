using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;

    Vector2 movement;   // stores x (horiz) and y (vert)

    void Update() {
        // Inputs
        movement.x = Input.GetAxisRaw("Horizontal");     // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");       // works default with WASD and arrow keys

    }

    // Movement
    void FixedUpdate() {
        // Movement

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
