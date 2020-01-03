using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] GameObject projectile;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float projectileFreq = 0.25f;

    Coroutine firingCoroutine;
    bool alive = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator FireContinuously()
    {
        anim.SetBool("Shooting", true);
        //yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        while (true)
        {
            GameObject laser = Instantiate(
                    projectile,
                    transform.position,
                    Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);

            yield return new WaitForSeconds(projectileFreq);
            anim.SetBool("Shooting", false);
        }
        
    }

    public void Fire()
    {
        if (alive)
        {
            if (Input.GetButtonDown("Fire1"))
                firingCoroutine = StartCoroutine(FireContinuously());

            if (Input.GetButtonUp("Fire1"))
                StopCoroutine(firingCoroutine);
        }
        else
            StopCoroutine(firingCoroutine);
    }
}
