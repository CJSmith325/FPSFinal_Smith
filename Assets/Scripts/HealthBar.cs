using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        SetMaxHealth();
    }

    private void Update()
    {
        if (slider.value == slider.minValue)
        {
            SceneManager.LoadScene("GameOver");
        }
    }

    public void SetMaxHealth()
    {
        slider.maxValue = PlayerHealth.playerHealth;
        slider.value = PlayerHealth.playerHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth()
    {
        //yield return new WaitForSeconds(0.5f);
        slider.value = PlayerHealth.playerHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
