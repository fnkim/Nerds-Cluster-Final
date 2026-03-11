using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

[ExecuteAlways]
public class WormMeshColor : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer wormRenderer;
    [SerializeField] private Color baseColor = Color.green;
    [SerializeField] private Color firstShadeColor = new Color(0.3f, 0.7f, 0.3f);

    void Start()
    {
        if (wormRenderer == null)
            wormRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        ApplyColors();
    }

    void OnValidate()
    {
        if (wormRenderer == null)
            wormRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        ApplyColors();
    }

    void ApplyColors()
    {
        if (wormRenderer == null) return;

        Material mat = Application.isPlaying
            ? wormRenderer.material
            : wormRenderer.sharedMaterial;

        mat.SetColor("_BaseColor", baseColor);
        mat.SetColor("_1st_ShadeColor", firstShadeColor);
    }
}
