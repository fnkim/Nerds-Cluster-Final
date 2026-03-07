using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ItemSound {Berry, Worm, Tree}


[CreateAssetMenu(menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string displayName;
    public Sprite icon;
    public bool stackable = true;

    public ItemSound itemSound;

    //enum for which item
}
