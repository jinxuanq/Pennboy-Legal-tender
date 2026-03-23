using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconSelector : MonoBehaviour
{

    [SerializeField] private Sprite fentFolder;
    [SerializeField] private Sprite loudTweaker;
    [SerializeField] private Sprite susMan;
    [SerializeField] private Sprite cop;
    [SerializeField] private UnityEngine.UI.Image iconImage;


    public void Sprite(String customerType)
    {
        if (customerType == "fentFolder")

        {
            iconImage.sprite = fentFolder;
        }
        if (customerType == "loudTweaker")
        {
            iconImage.sprite = loudTweaker;
        }
        if (customerType == "susMan")
        {
            iconImage.sprite = susMan;
        }
    }

    public void showCop()
    {
        iconImage.sprite = cop;
    }

}
