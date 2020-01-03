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
    [SerializeField] float baseMoveSpeed;


    Coroutine firingCoroutine;
    private bool alive = true;
    private float updatedMoveSpeed;

    Vector2 movement;   // stores x (horiz) and y (vert)

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
                firingCoroutine = StartCoroutine(Fire());

            if (Input.GetButtonUp("Fire1"))
                StopCoroutine(firingCoroutine);
        }
        else
            StopCoroutine(firingCoroutine);
    }

    IEnumerator Fire()
    {
        anim.SetBool("Shoot", true);
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        anim.SetBool("Shoot", false);
    }


    public void ProjectileFly(int facing)
    {
        GameObject arrow = Instantiate(
            projectile,
            firePoints[facing].position,
            Quaternion.identity) as GameObject;
        arrow.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
    }

    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
