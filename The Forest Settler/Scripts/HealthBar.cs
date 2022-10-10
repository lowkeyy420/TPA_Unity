using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider healthBar;
    public TMP_Text text;

    void Update()
    {
        displayHealth();
    }

    public void setMaxHealth(int hp)
    {
        healthBar.value = hp;
        healthBar.maxValue = hp;
    }

    public void setHealth(int hp)
    {
        healthBar.value = hp;
    }

    public void displayHealth()
    {
        text.SetText(healthBar.value + "/" + healthBar.maxValue);
    }

    
}
