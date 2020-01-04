using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float health = 200f;
    [SerializeField] float deathWait = 1f;
    Animator anim;
    Collider2D m_Collider;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        m_Collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    public void Damage(float dmg)
    {
        health -= dmg;
        if (health <= 0)
            StartCoroutine(Death());
        
    }

    IEnumerator Death()
    {
        anim.SetBool("Death", true);
        m_Collider.enabled = !m_Collider.enabled;
        yield return new WaitForSeconds(deathWait);
        //Destroy(gameObject);
    }

}
