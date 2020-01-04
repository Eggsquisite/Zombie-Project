using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] List<Transform> firePoints;
    [SerializeField] float health = 200f;
    [SerializeField] float baseMoveSpeed;
    [SerializeField] GameObject deathVFX;
    [SerializeField] AudioClip deathSFX;
    [SerializeField] float durationOfDeath = 3f;

    [Header("Sprite Blink")]
    [SerializeField] float gracePeriod = 1f;
    [SerializeField] float spriteBlinkMiniDuration = 0.1f;
    [SerializeField] float spriteBlinkTotalDuration = 1.0f;
    private float spriteBlinkTimer = 0.0f;
    private float spriteBlinkTotalTimer = 0.0f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sp;
    private bool alive = true;
    private bool aiming = false;
    private float updatedMoveSpeed, rotation;

    Vector2 movement;   // stores x (horiz) and y (vert)

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sp = GetComponent<SpriteRenderer>();

        anim.SetFloat("Horizontal", 1);
        updatedMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        Move();
        ReadyFire();
    }

    private void Move()
    {
        // Movement inputs
        movement.x = Input.GetAxisRaw("Horizontal");        // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");          // works default with WASD and arrow keys

        if (Input.GetKey(KeyCode.LeftShift))                // press and hold shift to move faster
            updatedMoveSpeed = 0f;
        else if (aiming)
            updatedMoveSpeed = baseMoveSpeed * 0f;
        else 
            updatedMoveSpeed = baseMoveSpeed;

        anim.SetFloat("Base Speed", updatedMoveSpeed);
        anim.SetFloat("Speed", movement.sqrMagnitude);      // sqrMag will always be pos, optimal since sqr root is unneeded

        if (movement.x >= 1)
        {
            anim.SetFloat("Horizontal", 1);
            anim.SetFloat("Vertical", 0);
        }
        else if (movement.x < 0)
        {
            anim.SetFloat("Horizontal", -1);
            anim.SetFloat("Vertical", 0);
        }

        if (movement.y >= 1)
        {
            anim.SetFloat("Vertical", 1);
            anim.SetFloat("Horizontal", 0);
        }
        else if (movement.y < 0)
        {
            anim.SetFloat("Vertical", -1);
            anim.SetFloat("Horizontal", 0);
        }
    }

    private void ReadyFire()
    {
        if (alive)
        {
            if (Input.GetButtonDown("Fire1") && aiming == false)
            {
                aiming = true;
                anim.SetBool("Shoot", true);
            }
        }
    }

    public void Fire(int facing)
    {
        aiming = false;
        anim.SetBool("Shoot", false);

        if (facing == 0)
            rotation = 180f;
        else if (facing == 1)
            rotation = 0f;
        else if (facing == 2)
            rotation = -90f;
        else if (facing == 3)
            rotation = 90f;

        Instantiate(
            projectile,
            firePoints[facing].position,
            Quaternion.Euler(0f, 0f, rotation));
    }

    public void GracePeriod()
    {
        Debug.Log("Hit");
        spriteBlinkTotalTimer += Time.deltaTime;
        if (spriteBlinkTotalTimer >= spriteBlinkTotalDuration)
        {
            spriteBlinkTotalTimer = 0.0f;
            sp.enabled = true;

            return;
        }

        spriteBlinkTimer += Time.deltaTime;
        if (spriteBlinkTimer >= spriteBlinkMiniDuration)
        {
            spriteBlinkTimer = 0.0f;
            if (sp.enabled)
                sp.enabled = false;
            else
                sp.enabled = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null)
        {
            Damage(damageDealer.GetDamage());
        }
    }

    public void Damage(int damageDealt)
    {
        health -= damageDealt;
        if (health > 0)
            return;
        //AudioSource.PlayClipAtPoint(hurtSFX, Camera.main.transform.position, hurtVolume);
        else
            Death();
    }

    private void Death()
    {
        alive = false;
        Destroy(gameObject);
        //AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, hurtVolume);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.Euler(0f, 180f, 0f));
        Destroy(explosion, durationOfDeath);
        //FindObjectOfType<Level>().LoadGameOver();
    }

    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * 5 * Time.fixedDeltaTime);
    }
}
