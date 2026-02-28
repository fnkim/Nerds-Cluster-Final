using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Recipe")]
public class RecipeData : ScriptableObject
{
    public string recipeName;
    public ItemData result;
    public ItemRequirement[] requirements;
}

[System.Serializable]
public class ItemRequirement
{
    public ItemData item;
    public int amount;
}
