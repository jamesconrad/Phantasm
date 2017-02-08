//////////////////////////////////////////////////////////////
//
//  Author: David F. Arppe - May 2016
//  ------------------
//
//  Simulates various effects to create a retro game feel.
//
//  This class provides a custom editor GUI for the main
//  class. There are sliders and some basic colors to help
//  simplify the input of material properties
//
//////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

#if UNITY_EDITOR
[CustomEditor(typeof(PixelNostalgia))]
#endif
public class PixelNostalgiaEditor : Editor
{
    #region member variables
    public SerializedProperty m_redBits, m_greenBits, m_blueBits;
    public SerializedProperty m_redPreset, m_greenPreset, m_bluePreset;

    public SerializedProperty m_resolution;
    public SerializedProperty m_presets;
    public SerializedProperty bitsEnabled, ditherEnabled, monochromeEnabled, m_resize;

    public SerializedProperty m_mainMaterial;

    public SerializedObject serObj;
    #endregion

    #region member functions
    void OnEnable()
    {
        if (target == null) return;
        serObj = new SerializedObject(target);

        #region finding properties
        m_redBits = serObj.FindProperty("m_redBits");
        m_greenBits = serObj.FindProperty("m_greenBits");
        m_blueBits = serObj.FindProperty("m_blueBits");
        m_redPreset = serObj.FindProperty("m_redPreset");
        m_greenPreset = serObj.FindProperty("m_greenPreset");
        m_bluePreset = serObj.FindProperty("m_bluePreset");

        m_resolution = serObj.FindProperty("m_resolution");
        m_presets = serObj.FindProperty("m_presets");

        bitsEnabled = serObj.FindProperty("bitsEnabled");
        ditherEnabled = serObj.FindProperty("ditherEnabled");
        monochromeEnabled = serObj.FindProperty("monochromeEnabled");
        m_resize = serObj.FindProperty("m_resize");

        string[] keyWords = (target as PixelNostalgia).m_mainMaterial.shaderKeywords;
        #endregion
    }

    private void DrawLine(Rect rect)
    {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.Slider(rect, 0.0f, 0.0f, 1.0f);
        EditorGUI.EndDisabledGroup();
    }

    public override void OnInspectorGUI()
    {
        if (target == null) return;

        serObj.Update();

        float width = 0.0f, posY = 0.0f, origPosY = 0.0f;
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            Rect scale = GUILayoutUtility.GetLastRect();

            width = scale.width;
            posY = scale.position.y + 5;
            origPosY = scale.position.y;
        }

        EditorGUI.BeginChangeCheck();
        bitsEnabled.boolValue = EditorGUI.ToggleLeft(new Rect(20, posY, width / 3, 20), "Color Depth", bitsEnabled.boolValue);
        ditherEnabled.boolValue = EditorGUI.ToggleLeft(new Rect((width / 3) + 20, posY, width / 3, 20), "Dithering", ditherEnabled.boolValue);
        m_resize.boolValue = EditorGUI.ToggleLeft(new Rect((2 * (width / 3)) + 20, posY, width / 3, 20), "Resize", m_resize.boolValue); posY += 10;

        #region bit input location
        DrawLine(new Rect(-50, posY, width + 250, 20)); posY += 10;
        EditorGUI.BeginDisabledGroup(!bitsEnabled.boolValue);
        {
            int r, g, b;
            bool u = ((PixelNostalgia.Presets)m_presets.enumValueIndex == PixelNostalgia.Presets.USER);
            if (u) { r = m_redBits.intValue; g = m_greenBits.intValue; b = m_blueBits.intValue; }
            else { r = m_redPreset.intValue; g = m_greenPreset.intValue; b = m_bluePreset.intValue; }

            EditorGUI.DropShadowLabel(new Rect(10, posY - 5.0f, width, 20),
                "You have " + Mathf.Pow(2, r + g + b).ToString("N0") + " possible colors!"); posY += 20;
            EditorGUI.BeginDisabledGroup((u) ? false : true);
            {
                EditorGUI.DrawRect(new Rect(0, posY, width + 20, 20), Color.red * 0.5f);
                r = EditorGUI.IntSlider(new Rect(10, posY, width, 20), "Red Bits", r, 0, 8); posY += 20;
                EditorGUI.DrawRect(new Rect(0, posY, width + 20, 20), Color.green * 0.5f);
                g = EditorGUI.IntSlider(new Rect(10, posY, width, 20), "Green Bits", g, 0, 8); posY += 20;
                EditorGUI.DrawRect(new Rect(0, posY, width + 20, 20), Color.blue * 0.5f);
                b = EditorGUI.IntSlider(new Rect(10, posY, width, 20), "Blue Bits", b, 0, 8); posY += 20;

                if (u) { m_redBits.intValue = r; m_greenBits.intValue = g; m_blueBits.intValue = b; }
            }
            EditorGUI.EndDisabledGroup();
            m_presets.enumValueIndex = (int)(PixelNostalgia.Presets)EditorGUI.Popup(new Rect(10, posY + 3, width, 20), "Color Depth Preset:",
                m_presets.enumValueIndex, PixelNostalgia.g_descriptions); posY += 20;
            EditorGUI.DrawRect(new Rect(0, posY, width + 20, 20), Color.white * 0.5f);
            monochromeEnabled.boolValue = EditorGUI.Toggle(new Rect(10, posY, width, 20), "Monochrome", monochromeEnabled.boolValue); posY += 10;
        }
        EditorGUI.EndDisabledGroup();
        #endregion

        #region resolution input location
        DrawLine(new Rect(-50, posY, width + 250, 20)); posY += 13;
        EditorGUI.BeginDisabledGroup(!m_resize.boolValue);
        m_resolution.vector2Value = EditorGUI.Vector2Field(new Rect(10, posY, width, 20), "Resolution",
            new Vector2(Mathf.Max(1, Mathf.Round(m_resolution.vector2Value.x)), Mathf.Max(1, Mathf.Round(m_resolution.vector2Value.y)))); posY += 20;

        if (GUI.Button(new Rect(10, posY + 20, width/2, 20), "Half"))
        {
            m_resolution.vector2Value = new Vector2(Mathf.Max(1, Mathf.Round(m_resolution.vector2Value.x / 2)), Mathf.Max(1, Mathf.Round(m_resolution.vector2Value.y / 2)));
        }
        if (GUI.Button(new Rect((width / 2) + 10, posY + 20, width / 2, 20), "Double"))
        {
            m_resolution.vector2Value.Scale(new Vector2(2, 2));

            float x = m_resolution.vector2Value.x;
            float y = m_resolution.vector2Value.y;

            if (x * 2 > 1920) { x = 1920.0f; } else { x = x * 2; }
            if (y * 2 > 1080) { y = 1080.0f; } else { y = y * 2; }

            m_resolution.vector2Value = new Vector2(x, y);
        }
        posY += 20;

        EditorGUI.EndDisabledGroup();
        #endregion

        if (EditorGUI.EndChangeCheck())
        {
            List<string> keywords = new List<string> {
                bitsEnabled.boolValue ? "BITS_ON" : "BITS_OFF",
                ditherEnabled.boolValue ? "DITHER_ON" : "DITHER_OFF",
                monochromeEnabled.boolValue ? "MONOCHROME_ON" : "MONOCHROME_OFF" };
            (target as PixelNostalgia).m_mainMaterial.shaderKeywords = keywords.ToArray();
            EditorUtility.SetDirty((target as PixelNostalgia).m_mainMaterial);
        }

        EditorGUILayout.LabelField(string.Empty, GUIStyle.none, GUILayout.Height(posY - origPosY + 25.0f));
        serObj.ApplyModifiedProperties();
    }
    #endregion
}