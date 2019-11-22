using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public GameObject hitEffect;

    private void Start()
    {
        Vector2 aimDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;      // Gets the correct angle of mouse position relating to player model

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Destroy(gameObject);
    }
}
