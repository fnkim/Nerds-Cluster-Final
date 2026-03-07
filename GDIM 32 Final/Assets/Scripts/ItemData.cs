using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public bool stackable = true;
    [SerializeField] private AudioClip audio;
}
