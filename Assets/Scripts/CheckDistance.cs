using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    EnemyAI enemyAI;
    Rigidbody2D rb;
    Transform target;
    bool found = false;
    public bool deathStatus = false;

    [SerializeField] float checkDistance = 5f;
    [SerializeField] float haltTime = 5f;
    [SerializeField] LayerMask myLayerMask;
    [SerializeField] Transform startPosition = null;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();

        rb = GetComponent<Rigidbody2D>();
        target = FindObjectOfType<Player>().gameObject.transform;

        enemyAI.enabled = false;
        InvokeRepeating("CalculateDistance", 0f, 1f);
    }

    private void CalculateDistance()
    {

        Vector2 targetDir = ((Vector2)target.position - rb.position).normalized;
        Debug.DrawLine(rb.position, target.position);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetDir, checkDistance, myLayerMask);
        found = Physics2D.Raycast(rb.position, targetDir, checkDistance, myLayerMask);

        if (found && hit.collider.gameObject.name == "Player")
        {
            if (!CheckDeath())
                enemyAI.enabled = true;

            return;
        }
        else
        {
            found = false;
            StartCoroutine(HaltMovement());
        }
    }

    IEnumerator HaltMovement()
    {
        yield return new WaitForSeconds(haltTime);
        if (!found)
            enemyAI.enabled = false;
    }

    private bool CheckDeath()
    {
        deathStatus = enemyAI.GetDeathStatus();
        return enemyAI.GetDeathStatus();
    }
}
