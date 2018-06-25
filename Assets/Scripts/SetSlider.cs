using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SetSlider : MonoBehaviour {

    public Slider SliderToChange;
    public float gravity = 0;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SetValue);
    }

    void SetValue()
    {
        SliderToChange.value = gravity;
    }
}
