using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator anim;
    [SerializeField] GameObject projectile;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileFreq = 0.25f;
    [SerializeField] float baseMoveSpeed, rotation;


    private bool alive = true;
    private float updatedMoveSpeed;

    Vector2 movement, arrowMovement;   // stores x (horiz) and y (vert)

    private void Start()
    {
        
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

        if (Input.GetKeyDown(KeyCode.LeftShift))            // press and hold shift to move faster
            updatedMoveSpeed = 0f;
        else if (Input.GetKeyUp(KeyCode.LeftShift))
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
            if (Input.GetButtonDown("Fire1"))
            {
                anim.SetBool("Shoot", true);
            }
        }
    }

    public void Fire(int facing)
    {
        anim.SetBool("Shoot", false);
        if (facing == 0)
            rotation = 90f;
        else if (facing == 1)
            rotation = -90f;
        else if (facing == 2)
            rotation = 180f;
        else if (facing == 3)
            rotation = -180f;

        GameObject newArrow = Instantiate(
            projectile,
            firePoints[facing].position,
            Quaternion.Euler(0f, 0f, rotation)) as GameObject;

        arrowMovement.x = Mathf.Abs(anim.GetFloat("Horizontal"));
        arrowMovement.y = Mathf.Abs(anim.GetFloat("Vertical"));

        Rigidbody2D rbArrow = newArrow.GetComponent<Rigidbody2D>();
        rbArrow.MovePosition(rbArrow.position + arrowMovement * projectileSpeed * 5 * Time.fixedDeltaTime);
    }


    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
