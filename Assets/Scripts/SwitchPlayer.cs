using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SwitchPlayer : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] Player humanPlayer;
    [SerializeField] Player shadowPlayer;
    [SerializeField] float switchCooldown = 2f;
    [SerializeField] float switchTimer = 0f;

    [Header("Lights")]
    [SerializeField] Light2D humanLight;
    [SerializeField] Light2D shadowLight;

    bool startActive = true;

    // Start is called before the first frame update
    void Start()
    {
        humanPlayer.SetActive(startActive);
        shadowPlayer.SetActive(!startActive);

        shadowLight.enabled = !startActive;
        humanLight.enabled = startActive;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchInput();
    }

    private void SwitchInput()
    {
        if (switchTimer <= switchCooldown)
            switchTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && switchTimer >= switchCooldown)
        {
            switchTimer = 0f;

            startActive = !startActive;
            humanPlayer.SetActive(startActive);
            shadowPlayer.SetActive(!startActive);

            humanLight.enabled = startActive;
            shadowLight.enabled = !startActive;
        }
    }
}
