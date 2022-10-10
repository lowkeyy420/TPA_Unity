using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Slider expBar;
    public TMP_Text levelBar;
    public TMP_Text levelValue;

    private float maxXP, currXP, level;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        displayXP();
    }

    public void setMaxXP(float xp)
    {
        expBar.maxValue = xp;
        expBar.value = 0;
    }

    public void setCurrentXP(float xp)
    {
        expBar.value = xp;
    }

    public void setLevel(float lvl)
    {
        level = lvl;
    }

    public void displayXP()
    {
        levelBar.SetText(expBar.value + "/" + (int)expBar.maxValue);
        levelValue.SetText(level.ToString());
    }

}
