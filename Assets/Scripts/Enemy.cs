using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int health = 200;
    [SerializeField] float hitWait = 0.1f;
    [SerializeField] float deathWait = 1f;

    [SerializeField] AudioClip hurtSFX = null;
    [SerializeField] AudioClip deathSFX = null;
    [Range(0, 1)] [SerializeField] float enemyVolume = 1f;

    Animator anim = null;
    Collider2D m_Collider = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            Damage(damageDealer.GetDamage());
        }
    }

    public void Damage(int dmg)
    {
        health -= dmg;

        if (health <= 0)
            Death();
        else
            StartCoroutine(Hit());
        
    }

    private void Death()
    {
        anim.SetBool("Death", true);
        m_Collider.enabled = !m_Collider.enabled;
        AudioSource.PlayClipAtPoint(deathSFX, transform.position, enemyVolume);
        Destroy(gameObject, deathWait);
    }

    IEnumerator Hit()
    {
        GetComponent<SpriteRenderer>().color = new Color(0.91509f, 0.21077f, 0f);
        yield return new WaitForSeconds(hitWait);
        GetComponent<SpriteRenderer>().color = new Color(255f, 255f, 255f);
    }
}
