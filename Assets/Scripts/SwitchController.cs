using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SwitchController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] Player humanPlayer;
    [SerializeField] Player shadowPlayer;
    [SerializeField] float switchCooldown = 2f;
    [SerializeField] float switchTimer = 0f;

    [Header("Lights")]
    [SerializeField] Light2D humanLight;
    [SerializeField] Light2D shadowLight;

    [Header("Slowdown")]
    [SerializeField] float slowDownFactor = 0.05f;
    [SerializeField] float slowDownTimer = 0f;
    [SerializeField] float slowDownLength = 2f;
    [SerializeField] float cameraTransition = 0.5f;

    [SerializeField] CameraTrack myCamera;

    bool startActive = true;

    // Start is called before the first frame update
    void Start()
    {
        humanPlayer.SetActive(startActive);
        shadowPlayer.SetActive(!startActive);

        myCamera.UpdatePlayer(humanPlayer.transform);

        shadowLight.enabled = !startActive;
        humanLight.enabled = startActive;
    }

    // Update is called once per frame
    void Update()
    {
        SwitchInput();

        if (Time.timeScale != 1f)
            ResetTimeScale();
    }

    private void ResetTimeScale()
    {
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Debug.Log(Time.timeScale);
       
    }

    private void SwitchInput()
    {
        if (switchTimer <= switchCooldown)
            switchTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && switchTimer >= switchCooldown)
        {
            switchTimer = 0f;
            slowDownTimer = 0f;

            StartCoroutine(DoSlowMotion());

            startActive = !startActive;
            humanPlayer.SetActive(startActive);
            shadowPlayer.SetActive(!startActive);

            if (startActive)
            {
                myCamera.UpdatePlayer(humanPlayer.transform);
                shadowLight.enabled = false;
                humanLight.enabled = true;
            }
            else if (!startActive)
            {
                myCamera.UpdatePlayer(shadowPlayer.transform);
                humanLight.enabled = false;
                shadowLight.enabled = true;
            }
        }
    }

    IEnumerator DoSlowMotion()
    {
        Debug.Log("Old delta time: " + Time.deltaTime);
        var oldDeltaTime = Time.fixedDeltaTime;
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * .02f;
        yield return new WaitForSeconds(cameraTransition);

        Time.fixedDeltaTime = oldDeltaTime;
        Debug.Log("New delta time: " + Time.deltaTime);
    }
}
