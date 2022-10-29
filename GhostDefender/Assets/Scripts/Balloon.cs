using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Waves/Balloon")]
public class Balloon : Spawnable
{
    [Header("Textures")]
    [Tooltip("The default texture")]
    public Sprite balloonTexture;
    public Sprite camoTexture;
    public Sprite regenTexture;
    public Sprite regenCamoTexture;
    public RuntimeAnimatorController animator;

}