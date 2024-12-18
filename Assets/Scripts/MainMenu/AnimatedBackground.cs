using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BackgroundType { Blue, Brown, Gray, Green, Pink, Purple, Yellow }

public class AnimatedBackground : MonoBehaviour
{
    MeshRenderer mr;

    [SerializeField] Vector2 movementDirection;

    [Header("Background type")]
    [SerializeField] BackgroundType backgroundType;
    [SerializeField] Texture2D[] textures;

    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        UpdateBackgroundTexture();
    }

    void Update()
    {
        mr.material.mainTextureOffset += movementDirection * Time.deltaTime;
    }

    [ContextMenu("Update Background")]
    void UpdateBackgroundTexture()
    {
        if(!mr)
        {
            mr = GetComponent<MeshRenderer>();
        }
        mr.sharedMaterial.mainTexture = textures[(int) backgroundType];
    }
}
