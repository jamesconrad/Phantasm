using UnityEngine;
using System.Collections;

public class FXAAScript : MonoBehaviour
{
    public Material effectMaterial;
         
    // OnRenderImage is called after all rendering is complete to render image
    public void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, effectMaterial);
    }
}
