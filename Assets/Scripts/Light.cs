using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float detectRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;

    Collider2D[] hitEnemies;

    private void Update()
    {
        hitEnemies = Physics2D.OverlapCircleAll(center.position, detectRange, enemyLayers);

        if (hitEnemies != null)
            Reveal();
    }

    private void Reveal()
    {
        foreach (Collider2D enemy in hitEnemies)
        { 
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
            collision.GetComponent<SpriteRenderer>().enabled = true;

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
            collision.GetComponent<SpriteRenderer>().enabled = false;
    }

    
}
