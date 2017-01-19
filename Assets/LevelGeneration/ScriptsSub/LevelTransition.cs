using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour {

    public string destinationLevel;
    public Texture2D fadeTexture; //overlay texture, probally will be the loading image
    public float fadeSpeed = 0.8f;

    private int drawDepth = -1000;//make sure overlay is overlayed
    private float alpha = 1.0f;
    private int fadeDir = -1;   //fade direction, fade in -1, fade out 1;

	void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
    }

    //Used for timing of level load
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }

    void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }

    public IEnumerator Transition()
    {
        float fadeTime = BeginFade(1);
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(destinationLevel);
    }
}
