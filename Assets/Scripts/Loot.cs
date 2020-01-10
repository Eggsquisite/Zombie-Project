using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] float destroyWait = 5f;
    [SerializeField] float blinkTime = 0.25f;
    [SerializeField] int maxBlinks = 16;
    [SerializeField] int blinkAmount = 0;

    [SerializeField] AudioClip pickupNoise = null;

    SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(destroyWait);
        InvokeRepeating("BlinkPeriod", 0f, blinkTime);
    }

    private void BlinkPeriod()
    {
        if (blinkAmount >= maxBlinks)
            Destroy(gameObject);

        sp.enabled = !sp.enabled;
        blinkAmount++;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            AudioSource.PlayClipAtPoint(pickupNoise, transform.position);
            // Call player function
            Destroy(gameObject);
        }
    }
}
