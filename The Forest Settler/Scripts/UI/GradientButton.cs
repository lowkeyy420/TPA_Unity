using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GradientButton : MonoBehaviour
{
    // Start is called before the first frame update

    public Gradient gBox;
    public Image fill;
    void Start()
    {
        fill = GetComponent<Image>();
        fill.color = gBox.Evaluate(1f);
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Space))
        fill.color = gBox.Evaluate(Random.Range(0f,1f));
    }
}
