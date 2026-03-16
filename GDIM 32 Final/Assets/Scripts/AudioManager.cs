using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] ItemSound itemSound;
    [SerializeField] private AudioSource berry;
    [SerializeField] private AudioSource worm;
    [SerializeField] private AudioSource tree;
    void Start()
    {
        PlayerInteractor.Instance.PickupCollectable += CollectSound;
    }

    void Update()
    {
        
    }

 void CollectSound(ItemData collectedItem)
    {
        itemSound = collectedItem.itemSound;
        switch (itemSound)
        {
            case ItemSound.Berry:
                PlayBerrySound();
                break;

            case ItemSound.Worm:
                PlayWormSound();
                break;
            
            case ItemSound.Tree:
               PlayTreeSound();
                break;
            
            default:
                break;
        }
    }

    private void PlayBerrySound()
    {
        berry.Play();
        Debug.Log("Berry Sound");
    }

    private void PlayWormSound()
    {
        worm.Play();
        Debug.Log("Worm Sound");
    }

    private void PlayTreeSound()
    {
        tree.Play();
        Debug.Log("Tree Sound");
    }

}
