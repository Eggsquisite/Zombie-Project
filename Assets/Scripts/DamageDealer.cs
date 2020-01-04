using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] float damage = 50f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject != null)
            collision.GetComponent<Health>().Damage(damage);
    }
}
