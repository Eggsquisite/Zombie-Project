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
    private IEnumerator coroutine;
    private float baseMoveSpeed;
    private float updatedMoveSpeed;

    private void Start()
    {
        baseMoveSpeed = playerAnim.GetFloat("Base Speed");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            horizontal = playerAnim.GetFloat("Facing Horizontal");
            vertical = playerAnim.GetFloat("Facing Vertical");
            updatedMoveSpeed = baseMoveSpeed / 2;
            playerAnim.SetFloat("Base Speed", updatedMoveSpeed);

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

            playerAnim.SetBool("Shooting", true);
            coroutine = Shoot();
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length + .5f);
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.AddForce(targetPos * arrowForce, ForceMode2D.Impulse);

        playerAnim.SetBool("Shooting", false);
        playerAnim.SetFloat("Base Speed", baseMoveSpeed);
    }
}
