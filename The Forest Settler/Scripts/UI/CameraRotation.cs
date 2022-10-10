using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private float randomSpeed1, randomSpeed2;

    // Start is called before the first frame update
    void Start()
    {
        randomSpeed1 = 0.2f;
        randomSpeed2 = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cameraRotation = new Vector3(0f, Random.Range(randomSpeed1, randomSpeed2) * Time.deltaTime, 0f);
        transform.Rotate(cameraRotation);
    }
}
