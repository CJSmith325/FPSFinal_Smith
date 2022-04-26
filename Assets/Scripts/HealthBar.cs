using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    private void Start()
    {
        SetMaxHealth();
    }

    public void SetMaxHealth()
    {
        slider.maxValue = PlayerHealth.playerHealth;
        slider.value = PlayerHealth.playerHealth;

        fill.color = gradient.Evaluate(1f);
    }

    public IEnumerator SetHealth()
    {
        yield return new WaitForSeconds(0.5f);
        slider.value = PlayerHealth.playerHealth;

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
