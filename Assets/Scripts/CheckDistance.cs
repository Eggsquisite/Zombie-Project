using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckDistance : MonoBehaviour
{
    EnemyAI enemyAI;
    Rigidbody2D rb;
    Transform target, startPosition;
    bool found = false;

    [SerializeField] float checkDistance = 5f;
    [SerializeField] float haltTime = 5f;
    [SerializeField] LayerMask myLayerMask;

    private void Start()
    {
        enemyAI = GetComponent<EnemyAI>();

        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("/Human Player").transform;
        //target = FindObjectOfType<Player>().gameObject.transform;
        startPosition = gameObject.transform;

        enemyAI.enabled = false;
        InvokeRepeating("CalculateDistance", 0f, 0.5f);
    }

    private void CalculateDistance()
    {

        Vector2 targetDir = ((Vector2)target.position - rb.position).normalized;
        Debug.DrawLine(rb.position, target.position);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, targetDir, checkDistance, myLayerMask);
        found = Physics2D.Raycast(rb.position, targetDir, checkDistance, myLayerMask);

        if (found && hit.collider.gameObject.name.Contains("Human"))
        {
            if (!CheckDeath())
            {
                enemyAI.enabled = true;
                enemyAI.SetTarget(target);

                return;
            }
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
        return enemyAI.GetDeathStatus();
    }
}
