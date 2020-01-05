using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] GameObject projectile = null;
    [SerializeField] List<Transform> firePoints = null;

    [Header("Player Stats")]
    [SerializeField] float health = 200f;
    [SerializeField] float baseMoveSpeed = 1f;
    [SerializeField] float speedMultiplier = 5f;
    [SerializeField] GameObject deathVFX = null;
    [SerializeField] AudioClip deathSFX = null;
    [SerializeField] List<AudioClip> hurtSFX = null;
    [SerializeField] float durationOfDeath = 3f;
    [Range(0,1)] [SerializeField] float hurtVolume = 1f;

    [Header("Sprite Blink")]
    [SerializeField] float spriteBlinkMiniDuration = 0.1f;
    [SerializeField] float spriteBlinkTotalDuration = 1.0f;
    private float spriteBlinkTimer = 0.0f;
    private float spriteBlinkTotalTimer = 0.0f;

    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sp;
    private bool alive = true;
    private bool aiming = false;
    private bool playerHit = false;
    private bool dash = true;
    private float updatedMoveSpeed, rotation;
    [SerializeField] int facing = -1;
    [SerializeField] float dashTimer = 3f;
    [SerializeField] float maxDash = 20f;
    [SerializeField] float dashSpeed = 10f;
    public DashState dashState;
    

    Vector2 movement, savedVelocity;   // stores x (horiz) and y (vert)

    public enum DashState
    {
        Ready,
        Dashing,
        Cooldown
    }

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
        Fire();
        Attack();

        if (playerHit)
            GracePeriod();

        switch (dashState)
        {
            case DashState.Ready:
                var isDashKeyDown = Input.GetKeyDown(KeyCode.LeftShift);
                if (isDashKeyDown)
                {
                    savedVelocity = rb.velocity;
                    rb.velocity = movement * dashSpeed * speedMultiplier * Time.deltaTime;
                    dashState = DashState.Dashing;
                }
                break;

            case DashState.Dashing:
                dashTimer += Time.deltaTime * 3;
                if (dashTimer >= maxDash)
                {
                    dashTimer = maxDash;
                    rb.velocity = savedVelocity;
                    dashState = DashState.Cooldown;
                }
                break;

            case DashState.Cooldown:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    dashTimer = 0;
                    dashState = DashState.Ready;
                }
                break;
        }
    }

    private void Move()
    {
        // Movement inputs
        movement.x = Input.GetAxisRaw("Horizontal");        // value btwn -1 and 1
        movement.y = Input.GetAxisRaw("Vertical");          // works default with WASD and arrow keys

        if (aiming || Input.GetButton("Fire2"))
            updatedMoveSpeed = baseMoveSpeed * 0f;
        else 
            updatedMoveSpeed = baseMoveSpeed;

        anim.SetFloat("Base Speed", updatedMoveSpeed);
        anim.SetFloat("Speed", movement.sqrMagnitude);      // sqrMag will always be pos, optimal since sqr root is unneeded

        if (movement.y >= 1)
        {
            // facing/moving UP
            if (aiming && facing >= 0)
                facing = 1;

            anim.SetFloat("Vertical", 1);
            anim.SetFloat("Horizontal", 0);
        }
        else if (movement.y < 0)
        {
            // facing/moving DOWN
            if (aiming && facing >= 0)
                facing = 0;

            anim.SetFloat("Vertical", -1);
            anim.SetFloat("Horizontal", 0);
        }

        if (movement.x >= 1)
        {
            // facing/moving RIGHT
            if (aiming && facing >= 0)
                facing = 2;

            anim.SetFloat("Horizontal", 1);
            anim.SetFloat("Vertical", 0);
        }
        else if (movement.x < 0)
        {
            // facing/moving LEFT
            if (aiming && facing >= 0)
                facing = 3;

            anim.SetFloat("Horizontal", -1);
            anim.SetFloat("Vertical", 0);
        }
    }

    private void ReadyFire()
    {
        if (alive)
        {
            if (Input.GetButton("Fire2") && aiming == false)
            {
                aiming = true;
                anim.SetBool("Shoot", true);
            }
            else if (Input.GetButtonUp("Fire2"))
            {
                facing = -1;
                aiming = false;
                anim.SetBool("Shoot", false);
            }
        }
    }

    private void Attack()
    {
        if (alive)
        {
            if (Input.GetButton("Fire1") && aiming == false)
            {
                anim.SetBool("Attack", true);
            }
        }
    }

    public void AttackRegister()
    {
        firePoints[facing].GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SetRotation(int rotation)
    {
        facing = rotation;
    }

    public void Fire()
    {
        if (Input.GetButton("Fire1") && aiming == true && facing >= 0)
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

            facing = -1;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (damageDealer != null && playerHit == false)
        {
            Damage(damageDealer.GetDamage());
        }
    }

    public void Damage(int damageDealt)
    {
        health -= damageDealt;
        if (health > 0)
        {
            playerHit = true;
            AudioSource.PlayClipAtPoint(hurtSFX[Random.Range(0, hurtSFX.Count)], Camera.main.transform.position, hurtVolume);
        }
        else
            Death();
    }

    private void GracePeriod()
    {
        Debug.Log("Hit");

        spriteBlinkTotalTimer += Time.deltaTime;
        if (spriteBlinkTotalTimer >= spriteBlinkTotalDuration)
        {
            spriteBlinkTotalTimer = 0.0f;
            sp.enabled = true;

            playerHit = false;
            return;
        }

        // Flicker sprite
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

    private void Death()
    {
        alive = false;
        AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, hurtVolume);
        Destroy(gameObject);
        GameObject explosion = Instantiate(deathVFX, transform.position, Quaternion.Euler(0f, 180f, 0f));
        Destroy(explosion, durationOfDeath);
        //FindObjectOfType<Level>().LoadGameOver();
    }

    // Movement
    void FixedUpdate() {
        // Movement
        rb.MovePosition(rb.position + movement * updatedMoveSpeed * speedMultiplier * Time.fixedDeltaTime);
    }
}
