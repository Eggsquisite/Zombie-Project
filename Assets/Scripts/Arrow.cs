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
    bool ready = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        FindAngle();
        StartCoroutine(Remove());
    }

    private void FindAngle()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lookDir = mousePos - rb.position;
        angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
        ready = true;
    }

    private void FixedUpdate()
    {
        if (ready)
            rb.velocity = transform.up * speed * Time.unscaledDeltaTime;
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
