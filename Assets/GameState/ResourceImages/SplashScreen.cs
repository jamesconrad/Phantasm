using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class SplashScreen : MonoBehaviour
{
    private RawImage splashImage;

    public float splashScreenLength;
    public float maxOpacity;
    public float opacityChangeSpeed;

    public UnityEvent OnTimeReached;
    public bool destroyOnTimeReached = false;
    
    private float currentTime = 0.0f;

    private bool isCreated = false;



    // Use this for initialization
    void Start()
    {
        splashImage = GetComponent<RawImage>();

        splashImage.color = new Color(splashImage.color.r, splashImage.color.g, splashImage.color.b, 0.0f);

        splashImage.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    // This function is called every fixed framerate frame, if the MonoBehaviour is enabled
    public void FixedUpdate()
    {
        if (isCreated)
        {
            splashImage.color = new Color(splashImage.color.r, splashImage.color.g, splashImage.color.b, Mathf.Clamp( (splashImage.color.a + opacityChangeSpeed * Time.deltaTime), 0.0f, maxOpacity));
            currentTime += Time.deltaTime;
            if (currentTime >= splashScreenLength)
            {
                OnTimeReached.Invoke();
                if (destroyOnTimeReached)
                {
                    Destroy(GetComponentInParent<Canvas>().rootCanvas.gameObject);
                }
            }
        }
    }

    public void createSplashScreen()
    {
        isCreated = true;
    }
}
