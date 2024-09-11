using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBarXR : MonoBehaviour
{
    Interface gui;
    
    public float newSize; // Desired size
    public Slider slider;  // Reference to the slider

    void Start()
    {
        gui = GameObject.Find ("Interface").GetComponent<Interface> ();

        Update();
    }

    // Function to update the fill area size based on a given number
    void Update()
    {
        newSize = gui.curBarValue;
        print(newSize);

        slider.value = newSize;
        slider.onValueChanged.Invoke(slider.value);
    }

}
