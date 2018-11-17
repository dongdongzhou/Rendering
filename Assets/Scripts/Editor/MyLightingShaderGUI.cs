﻿using UnityEditor;
using UnityEngine;

public class MyLightingShaderGUI : ShaderGUI
{
    private static readonly GUIContent staticLabel = new GUIContent();

    private Material target;
    private MaterialEditor editor;
    private MaterialProperty[] properties;

    public override void OnGUI(MaterialEditor editor, MaterialProperty[] properties)
    {
        target = editor.target as Material;
        this.editor = editor;
        this.properties = properties;
        DoMain();
        DoSecondary();
    }

    private void DoMain()
    {
        GUILayout.Label("Main Maps", EditorStyles.boldLabel);

        MaterialProperty mainTex = FindProperty("_MainTex");
        editor.TexturePropertySingleLine(MakeLabel(mainTex, "Albedo (RGB)"), mainTex,
                                         FindProperty("_Tint"));
        DoMetallic();
        DoSmoothness();
        DoNormals();
        DoOcclusion();
        DoEmission();
        DoDetailMask();
        editor.TextureScaleOffsetProperty(mainTex);
    }

    private void DoNormals()
    {
        MaterialProperty map = FindProperty("_NormalMap");
        Texture tex = map.textureValue;
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(map), map,
                                         tex ? FindProperty("_BumpScale") : null);
        if (EditorGUI.EndChangeCheck() && tex != map.textureValue)
        {
            SetKeyword("_NORMAL_MAP", map.textureValue);
        }
    }

    private void DoMetallic()
    {
        MaterialProperty map = FindProperty("_MetallicMap");
        Texture tex = map.textureValue;
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(map, "Metallic (R)"), map,
                                         tex ? null : FindProperty("_Metallic"));
        if (EditorGUI.EndChangeCheck()&& tex != map.textureValue)
            SetKeyword("_METALLIC_MAP", map.textureValue);
    }

    private void DoSmoothness()
    {
        var source = SmoothnessSource.Uniform;
        if (IsKeywordEnabled("_SMOOTHNESS_ALBEDO"))
            source = SmoothnessSource.Albedo;
        else if (IsKeywordEnabled("_SMOOTHNESS_METALLIC"))
            source = SmoothnessSource.Metallic;
        MaterialProperty slider = FindProperty("_Smoothness");
        EditorGUI.indentLevel += 2;
        editor.ShaderProperty(slider, MakeLabel(slider));
        EditorGUI.indentLevel += 1;
        EditorGUI.BeginChangeCheck();
        source = (SmoothnessSource) EditorGUILayout.EnumPopup(MakeLabel("Source"), source);
        if (EditorGUI.EndChangeCheck())
        {
            RecordAction("Smoothness Source");
            SetKeyword("_SMOOTHNESS_ALBEDO", source == SmoothnessSource.Albedo);
            SetKeyword("_SMOOTHNESS_METALLIC", source == SmoothnessSource.Metallic);
        }

        EditorGUI.indentLevel -= 3;
    }

    private void DoOcclusion()
    {
        MaterialProperty map = FindProperty("_OcclusionMap");
        Texture tex = map.textureValue;
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(map, "Occlusion (G)"), map,
                                         tex? FindProperty("_OcclusionStrength"): null);
        if (EditorGUI.EndChangeCheck()&& tex != map.textureValue)
            SetKeyword("_OCCLUSION_MAP", map.textureValue);
    }

    private void DoEmission()
    {
        MaterialProperty map = FindProperty("_EmissionMap");
        Texture tex = map.textureValue;
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertyWithHDRColor(MakeLabel(map, "Emission (RGB)"), map,
                                           FindProperty("_Emission"),
                                           false);
        if (EditorGUI.EndChangeCheck()&&tex!=map.textureValue)
            SetKeyword("_EMISSION_MAP", map.textureValue);
    }

    private void DoDetailMask()
    {
        MaterialProperty mask = FindProperty("_DetailMask");
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(mask, "Detail Mask (A)"), mask);
        if (EditorGUI.EndChangeCheck())
            SetKeyword("_DETAIL_MASK", mask.textureValue);
    }

    private void DoSecondary()
    {
        GUILayout.Label("Secondary Maps", EditorStyles.boldLabel);

        MaterialProperty detailTex = FindProperty("_DetailTex");
        editor.TexturePropertySingleLine(MakeLabel(detailTex, "Albedo (RGB) multiplied by 2"),
                                         detailTex);
        DoSecondaryNormals();
        editor.TextureScaleOffsetProperty(detailTex);
    }

    private void DoSecondaryNormals()
    {
        MaterialProperty map = FindProperty("_DetailNormalMap");
        Texture tex = map.textureValue;
        EditorGUI.BeginChangeCheck();
        editor.TexturePropertySingleLine(MakeLabel(map), map,
                                         tex ? FindProperty("_DetailBumpScale") : null);
        if (EditorGUI.EndChangeCheck() && tex != map.textureValue)
        {
            SetKeyword("_DETAIL_NORMAL_MAP", map.textureValue);
        }
    }

    private MaterialProperty FindProperty(string name) => FindProperty(name, properties);

    private static GUIContent MakeLabel(string text, string tooltip = null)
    {
        staticLabel.text = text;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    private static GUIContent MakeLabel(
        MaterialProperty property, string tooltip = null
    )
    {
        staticLabel.text = property.displayName;
        staticLabel.tooltip = tooltip;
        return staticLabel;
    }

    private void SetKeyword(string keyword, bool state)
    {
        if (state)
        {
            foreach (Material m in editor.targets)
            {
                m.EnableKeyword(keyword);
            }
          
        }
        else
        {
            foreach (Material m in editor.targets)
            {
                   m.DisableKeyword(keyword); 
            }
        }
    }

    private bool IsKeywordEnabled(string keyword) => target.IsKeywordEnabled(keyword);

    private void RecordAction(string label)
    {
        editor.RegisterPropertyChangeUndo(label);
    }

    private enum SmoothnessSource
    {
        Uniform,
        Albedo,
        Metallic
    }
}