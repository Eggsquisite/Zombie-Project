using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public Transform firePoint;
    public GameObject arrowPrefab;

    public float arrowForce = 5f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
            Shoot();
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();

        var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.AddForce(targetPos * arrowForce, ForceMode2D.Impulse);
    }
}
