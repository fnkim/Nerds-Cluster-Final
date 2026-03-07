using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBubble : MonoBehaviour
{
    [SerializeField] private TMP_Text _npctext;
    [SerializeField] float TimeBtwChars = 0.03f;
    [SerializeField] public bool IsTyping = false;
    [SerializeField] public bool speedUpText;

    [SerializeField] private AudioClip _dialogueAudioClip;
    [SerializeField] private AudioSource _dialogueAudioSource;



    [SerializeField] private GameObject _npcDialogue;
    [SerializeField] private GameObject _playerOptions;


    [SerializeField] private TMP_Text _option1;
    [SerializeField] private TMP_Text _option2;
    [SerializeField] private TMP_Text _option3;

    public void ShowDialogue(string dialogue)
    {


        _npctext.text = dialogue;
        _npcDialogue.SetActive(true);

        gameObject.SetActive(true);
        _playerOptions.SetActive(false);


        StartCoroutine(IncreaseMaxVisibleChar(dialogue));


    }




    public void ShowPlayerOptions(string[] options)
    {
        gameObject.SetActive(true);

        _npcDialogue.SetActive(false);
        _playerOptions.SetActive(true);

        _option1.text = options[0];

        if(options.Length >= 2)
        {
            _option2.transform.parent.gameObject.SetActive(true);
            _option2.text = options[1];
        }
        else 
        {
            _option2.transform.parent.gameObject.SetActive(false);
        }

        if(options.Length >= 3)
        {
            _option3.transform.parent.gameObject.SetActive(true);
            _option3.text = options[2];
        }
        else 
        {
            _option3.transform.parent.gameObject.SetActive(false);
        }
    }

    public void HideDialogue()
    {
        _playerOptions.SetActive(false);
        _npcDialogue.SetActive(false);
        gameObject.SetActive(false);

    }



    IEnumerator IncreaseMaxVisibleChar(string dialogue)
    {
        IsTyping = true;
        

        _npctext.text = dialogue; //Make the text mesh's content the whole message string right at the beginning. So the characters will stay at the correct positions since the beginning
        _npctext.maxVisibleCharacters = 0;
        int messageCharLength = dialogue.Length;

        while (_npctext.maxVisibleCharacters < messageCharLength)
        {
            
            if (speedUpText) //When fullText is triggered (by pressing space), the typewriter stops and the full text is shown
            {
                TimeBtwChars = 0.01f;
            }

            _npctext.maxVisibleCharacters++;
            _dialogueAudioSource.Play();
            yield return new WaitForSeconds(TimeBtwChars);
        }

        IsTyping = false;
    }



}
