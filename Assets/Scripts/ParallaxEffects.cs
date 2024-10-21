using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffects : MonoBehaviour
{
    [SerializeField]
    private Transform cam;
    [SerializeField]
    private float porcentaje;
    private Vector3 previousPos;

    // Start is called before the first frame update
    void Start()
    {
        previousPos = cam.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 diferencia = cam.position - previousPos;
        transform.Translate(diferencia * porcentaje);
        previousPos = cam.position;
    }
}
