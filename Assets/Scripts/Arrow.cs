using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float speed = 500f;
    [SerializeField] float destroyWait = 5f;
    
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed * Time.deltaTime;
        StartCoroutine(Remove());
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
