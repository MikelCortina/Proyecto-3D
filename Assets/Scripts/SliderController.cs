using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderController : MonoBehaviour
{
    public Slider mySlider;
    public TextMeshProUGUI valueText;

    void Start()
    {
        mySlider.value = GameData.sliderValue;
        mySlider.onValueChanged.AddListener(UpdateUI);
        UpdateUI(mySlider.value);
    }

    void UpdateUI(float value)
    {
        GameData.sliderValue = value;
        valueText.text = value.ToString("F2");
    }
}
