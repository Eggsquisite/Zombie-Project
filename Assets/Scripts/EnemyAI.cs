using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] GameObject enemyGFX = null;
    Transform target = null;
    [SerializeField] float speed = 200f;
    [SerializeField] float nextWaypointDistance = 3f;       // how close enemy needs to be to waypoint before moving to next one
    [SerializeField] float deathWait = 5f;
    [SerializeField] float checkDistance = 5f;

    Path path;
    private int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    bool dead = false;
    public bool inDistance = false;

    Seeker seeker;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Player>().gameObject.transform;
    }

    private void InDistance()
    {
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    private void UpdatePath()
    {
        if (seeker.IsDone() && !dead && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (target != null)
        {
            float newDistance = Vector2.Distance(rb.position, target.position);
            Vector2 direction = ((Vector2)target.position - rb.position).normalized;



            if (newDistance <= 5)
            {
                Debug.Log("In distance!");
                Debug.DrawLine(rb.position, target.position, Color.green);
                RaycastHit2D hit = Physics2D.Raycast(rb.position, direction, checkDistance);

                if (hit.collider.gameObject.name == "Player")
                {
                    Debug.Log("Player hit with raycast");
                    InDistance();
                    return;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
            reachedEndOfPath = false;


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;         // gives vector from position to next waypoint
        Vector2 force = direction * speed * Time.deltaTime;
        

        rb.AddForce(force);
        //Facing(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
            currentWaypoint++;
    }

    private void CheckDistance(Vector2 targetDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetDir, checkDistance);

        if (hit.collider.gameObject.tag == "Player")
        {
            Debug.Log("Player hit with raycast");
            InDistance();
            return;
        }
    }

    public void SetDisabled()
    {
        dead = false;
        if(rb != null)
            rb.velocity = new Vector3(0f, 0f, 0f);

        Destroy(gameObject, deathWait * 2);
    }

    private void Facing(Vector2 force)
    {
        if (enemyGFX == null)
            return;

        if (force.x >= 0.01f)
        {
            enemyGFX.transform.localScale = new Vector3(1f, 1f, 1f);
            //enemyGFX.GetComponent<Animator>().SetFloat("Horizontal", 1f);
            //enemyGFX.GetComponent<Animator>().SetFloat("Vertical", 0f);
        }
        else if (force.x <= -0.01f)
        { 
            enemyGFX.transform.localScale = new Vector3(-1f, 1f, 1f);
            //enemyGFX.GetComponent<Animator>().SetFloat("Vertical", 1f);
            //enemyGFX.GetComponent<Animator>().SetFloat("Horizontal", 0f);
        }

        if (force.y >= 0.01f)
        {
            enemyGFX.GetComponent<Animator>().SetFloat("Horizontal", 0f);
            enemyGFX.GetComponent<Animator>().SetFloat("Vertical", 1f);
        }
        else if (force.y <= -0.01f)
        {
            enemyGFX.GetComponent<Animator>().SetFloat("Vertical", -1f);
            enemyGFX.GetComponent<Animator>().SetFloat("Horizontal", 0f);
        }
    }
}
