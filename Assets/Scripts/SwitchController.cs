using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class SwitchController : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] GameObject humanPlayer = null;
    [SerializeField] GameObject shadowPlayer = null;
    [SerializeField] GameObject shadowPrefab = null;
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
    [SerializeField] float smoothTime = 50f;

    [SerializeField] CameraTrack myCamera;

    bool startActive = true;
    Player myHuman, myShadow;

    // Start is called before the first frame update
    void Start()
    {
        myHuman = humanPlayer.GetComponent<Player>();
        myShadow = shadowPlayer.GetComponent<Player>();

        myHuman.SetActive(startActive);
        myShadow.SetActive(!startActive);

        myCamera.UpdatePlayer(myHuman.transform);

        shadowLight.enabled = !startActive;
        humanLight.enabled = startActive;
    }

    // Update is called once per frame
    void Update()
    {
        //SwitchInput();

        if (switchTimer <= switchCooldown)
            switchTimer += Time.deltaTime;

        if (Time.timeScale != 1f)
            ResetTimeScale();
    }

    private void ResetTimeScale()
    {
        Time.timeScale += (1f / slowDownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Debug.Log(Time.timeScale);
       
    }

    public void SwitchPlayer()
    {
        if (switchTimer >= switchCooldown)
        {
            if (myShadow.GetShadowDeath() == true)
            {
                myShadow.transform.position = new Vector3(myHuman.transform.position.x, myHuman.transform.position.y, myHuman.transform.position.z);
                myShadow.SetShadowDeath(false);
            }

            switchTimer = 0f;
            slowDownTimer = 0f;

            StartCoroutine(DoSlowMotion());

            startActive = !startActive;
            myHuman.SetActive(startActive);
            myShadow.SetActive(!startActive);
            myCamera.SetSmoothTime(smoothTime);

            if (startActive)
            {
                myCamera.UpdatePlayer(myHuman.transform);
                shadowLight.enabled = false;
                humanLight.enabled = true;
            }
            else if (!startActive)
            {
                myCamera.UpdatePlayer(myShadow.transform);
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
        myCamera.ResetSmoothTime();
        Debug.Log("New delta time: " + Time.deltaTime);
    }
}
