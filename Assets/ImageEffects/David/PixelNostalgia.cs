//////////////////////////////////////////////////////////////
//
//  Author: David F. Arppe - May 2016
//  ------------------
//
//  Simulates various effects to create a retro game feel.
//
//  This class handles the material during runtime. It passes
//  the properties of the image effect to the material before
//  rendering it to the screen.
//
//////////////////////////////////////////////////////////////

using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode, AddComponentMenu("Image Effects/Custom/Pixel Nostalgia")]
public class PixelNostalgia : MonoBehaviour
{
    #region member variables
    public int m_redBits = 8;
    public int m_greenBits = 8;
    public int m_blueBits = 8;
    public int m_redPreset;
    public int m_greenPreset;
    public int m_bluePreset;
    public bool m_resize = true;

    public Vector2 m_resolution = new Vector2(100, 100);

    public static string[] g_descriptions =
    {
        "RGB332 - 8Bit",
        "RGB444 - Direct Color",
        "RGB555 - High Color",
        "RGB565 - 16Bit",
        "RGB666 - 18Bit",
        "RGB888 - True Color",
        "User Defined"
    };

    public enum Presets
    {
        BGR233,
        RGB444,
        RGB555,
        RGB565,
        RGB666,
        RGB888,
        USER
    }

    public Presets m_presets = Presets.USER;

    public bool bitsEnabled = true;
    public bool ditherEnabled = true;
    public bool monochromeEnabled = false;

    private Material mainMaterial;
    public Material m_mainMaterial
	{
		get
		{
            if (mainMaterial == null)
			{
                Shader shader = Shader.Find("Hidden/NostalgiaPixels-Base");
                mainMaterial = new Material(shader);
                mainMaterial.hideFlags = HideFlags.DontSave;
			}
            return mainMaterial;
		} 
	}
    #endregion

    #region member functions
    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            Debug.LogWarning("Your system does not support PixelNostalgia");
            this.enabled = false;
        }
    }

    void SetKeyword(Material m, bool firstOn, string firstKeyword, string secondKeyword)
    {
        m.EnableKeyword(firstOn ? firstKeyword : secondKeyword);
        m.DisableKeyword(firstOn ? secondKeyword : firstKeyword);
    }

    public void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (m_mainMaterial)
        {
            #region setting material properties
            int r = 0, g = 0, b = 0;

            switch (m_presets)
            {
                case Presets.BGR233:
                    r = g = 3; b = 2; break;
                case Presets.RGB444:
                    r = g = b = 4; break;
                case Presets.RGB555:
                    r = g = b = 5; break;
                case Presets.RGB565:
                    r = b = 5; g = 6; break;
                case Presets.RGB666:
                    r = g = b = 6; break;
                case Presets.RGB888:
                    r = g = b = 8; break;
                case Presets.USER:
                    r = m_redBits; g = m_greenBits; b = m_blueBits; break;
            }
            m_redPreset = r; m_greenPreset = g; m_bluePreset = b;

            m_mainMaterial.SetFloat("_redBits", Mathf.Pow(2, r) - 1);
            m_mainMaterial.SetFloat("_greenBits", Mathf.Pow(2, g) - 1);
            m_mainMaterial.SetFloat("_blueBits", Mathf.Pow(2, b) - 1);

            SetKeyword(m_mainMaterial, bitsEnabled, "BITS_ON", "BITS_OFF");
            SetKeyword(m_mainMaterial, ditherEnabled, "DITHER_ON", "DITHER_OFF");
            SetKeyword(m_mainMaterial, monochromeEnabled, "MONOCHROME_ON", "MONOCHROME_OFF");

            #endregion
            #region blit to fullscreen with effect
            if (m_resize)
            {
                m_mainMaterial.SetFloat("_xRes", (int)m_resolution.x);
                m_mainMaterial.SetFloat("_yRes", (int)m_resolution.y);

                Graphics.Blit(src, dest, m_mainMaterial);
            }
            else
            {
                m_mainMaterial.SetFloat("_xRes", (int)src.width);
                m_mainMaterial.SetFloat("_yRes", (int)src.height);

                Graphics.Blit(src, dest, m_mainMaterial);
            }
            #endregion
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
    #endregion
}