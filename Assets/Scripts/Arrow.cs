using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed = 500f;
    [SerializeField] float destroyWait = 5f;
    
    Rigidbody2D rb;
    Vector2 mousePos, lookDir;
    float angle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        FindAngle();

        StartCoroutine(Remove());
    }

    private void FindAngle()
    {
        lookDir = mousePos - rb.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    private void FixedUpdate()
    {
        rb.velocity = transform.up * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject != null)
            Destroy(gameObject);
    }

    IEnumerator Remove()
    {
        yield return new WaitForSeconds(destroyWait);
        Destroy(gameObject);
    }
}
