using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] float baseMoveSpeed;

    Rigidbody2D rb;
    Animator anim;
    private bool alive = true;
    private bool aiming = false;
    private float updatedMoveSpeed, rotation;

    Vector2 movement;   // stores x (horiz) and y (vert)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        anim.SetFloat("Horizontal", 1);
        updatedMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        Move();
        ReadyFire();
    }

    private void Move()
    {
        // Movement inputs
        movement.x = Input.GetAxisRaw("Horizontal");        // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");          // works default with WASD and arrow keys

        if (Input.GetKey(KeyCode.LeftShift))                // press and hold shift to move faster
            updatedMoveSpeed = 0f;
        else if (aiming)
            updatedMoveSpeed = baseMoveSpeed * 0f;
        else 
            updatedMoveSpeed = baseMoveSpeed;

        anim.SetFloat("Base Speed", updatedMoveSpeed);
        anim.SetFloat("Speed", movement.sqrMagnitude);      // sqrMag will always be pos, optimal since sqr root is unneeded

        if (movement.x >= 1)
        {
            anim.SetFloat("Horizontal", 1);
            anim.SetFloat("Vertical", 0);
        }
        else if (movement.x < 0)
        {
            anim.SetFloat("Horizontal", -1);
            anim.SetFloat("Vertical", 0);
        }

        if (movement.y >= 1)
        {
            anim.SetFloat("Vertical", 1);
            anim.SetFloat("Horizontal", 0);
        }
        else if (movement.y < 0)
        {
            anim.SetFloat("Vertical", -1);
            anim.SetFloat("Horizontal", 0);
        }
    }

    private void ReadyFire()
    {
        if (alive)
        {
            if (Input.GetButtonDown("Fire1") && aiming == false)
            {
                aiming = true;
                anim.SetBool("Shoot", true);
            }
        }
    }

    public void Fire(int facing)
    {
        aiming = false;
        anim.SetBool("Shoot", false);

        if (facing == 0)
            rotation = 180f;
        else if (facing == 1)
            rotation = 0f;
        else if (facing == 2)
            rotation = -90f;
        else if (facing == 3)
            rotation = 90f;

        Instantiate(
            projectile,
            firePoints[facing].position,
            Quaternion.Euler(0f, 0f, rotation));
    }


    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
