using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] float TimeBtwChars = 0.03f;
    [SerializeField] public bool IsTyping = false;
    [SerializeField] public bool fullText;

    [SerializeField] private AudioClip _dialogueAudioClip;
    [SerializeField] private AudioSource _dialogueAudioSource;


    public void ShowDialogue(string dialogue)
    {
        fullText = false;
        _text.text = dialogue;
        gameObject.SetActive(true);
        StartCoroutine(IncreaseMaxVisibleChar(dialogue));


    }

    public void HideDialogue()
    {
        gameObject.SetActive(false);
    }


    IEnumerator IncreaseMaxVisibleChar(string dialogue)
    {
        IsTyping = true;
        

        _text.text = dialogue; //Make the text mesh's content the whole message string right at the beginning. So the characters will stay at the correct positions since the beginning
        _text.maxVisibleCharacters = 0;
        int messageCharLength = dialogue.Length;

        while (_text.maxVisibleCharacters < messageCharLength)
        {
            
            if (fullText) //When fullText is triggered (by pressing E), the typewriter stops and the full text is shown
            {
                _text.maxVisibleCharacters = messageCharLength;
            }

            _text.maxVisibleCharacters++;
            _dialogueAudioSource.Play();
            yield return new WaitForSeconds(TimeBtwChars);
        }

        IsTyping = false;
    }





}
