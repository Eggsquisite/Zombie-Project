using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;
    public Animator playerAnim;

    public float arrowForce = 5f;
    private float horizontal;
    private float vertical;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            horizontal = playerAnim.GetFloat("Facing Horizontal");
            vertical = playerAnim.GetFloat("Facing Vertical");

            if (vertical == -1)
                playerAnim.SetFloat("Vertical Shoot", -1f);          // if aiming down, shoot down
            else if (vertical == 1)
                playerAnim.SetFloat("Vertical Shoot", 1f);         // if aiming up, shoot up
            else
                playerAnim.SetFloat("Vertical Shoot", 0f);

            if (vertical == 0 && horizontal == -1)
                playerAnim.SetFloat("Horizontal Shoot", -1f);        // if aiming left, shoot left
            else if (vertical == 0 && horizontal == 1)
                playerAnim.SetFloat("Horizontal Shoot", 1f);       // if aiming right, shoot right
            else
                playerAnim.SetFloat("Horizontal Shoot", 0f);

            Shoot();
        }
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.AddForce(targetPos * arrowForce, ForceMode2D.Impulse);
    }
}
