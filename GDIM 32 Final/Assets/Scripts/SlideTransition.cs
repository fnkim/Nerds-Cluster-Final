using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SlideTransition : MonoBehaviour
{
    public  List<GameObject> slidesList = new List<GameObject>();
    public int selectedSlide;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
        {
            slidesList.Add(child.gameObject);
        }

        selectedSlide = 0;
        slidesList[selectedSlide].SetActive(true);




    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void MovetoNextSlide()
    {
        if(selectedSlide == 22)
        {
            SceneManager.LoadScene("Main");
        }
        else
        {
            selectedSlide ++;
            slidesList[selectedSlide].SetActive(true);
        }


    }




}
