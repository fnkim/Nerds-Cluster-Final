using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class OvenCrafter : MonoBehaviour
{
    [SerializeField] private RecipeData recipe;
    [SerializeField] private float bakeTime = 2f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip bakeDoneSfx;

    public IEnumerator Bake(Inventory playerInventory, Inventory ovenInventory, Image progressFill, Action onFinished)
    {
        if (!CanCraft(ovenInventory))
            yield break;

        if (progressFill != null)
            progressFill.fillAmount = 0f;

        float t = 0f;
        while (t < bakeTime)
        {
            t += Time.unscaledDeltaTime;

            if (progressFill != null)
                progressFill.fillAmount = t / bakeTime;

            yield return null;
        }

        foreach (var req in recipe.requirements)
            ovenInventory.Remove(req.item, req.amount);

        playerInventory.Add(recipe.result, 1);

        if (audioSource != null && bakeDoneSfx != null)
            audioSource.PlayOneShot(bakeDoneSfx);

        if (progressFill != null)
            progressFill.fillAmount = 0f;

        onFinished?.Invoke();
    }

    private bool CanCraft(Inventory inventory)
    {
        foreach (var req in recipe.requirements)
        {
            if (!inventory.Has(req.item, req.amount))
                return false;
        }

        return true;
    }
}
