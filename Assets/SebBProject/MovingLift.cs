using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingLift : MonoBehaviour
{
    //Lift

    [SerializeField] float amplitude;
    [SerializeField] float frequency;
    float oscilate;
    [SerializeField] float scaleX;
    [SerializeField] float scaleY; //Y Axis movement 
    [SerializeField] float scaleZ;
    float timer;
    Vector3 startPosition;


    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }
  
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        oscilate = amplitude * Mathf.Sin(timer * frequency); // Sin relating to the sin wave ranges from zero , to 90, - -90 
        oscilate = Mathf.Clamp(oscilate, -1f, 1f);
        transform.position = new Vector3(startPosition.x + (oscilate * scaleX), startPosition.y + (oscilate * scaleY), startPosition.z + (oscilate * scaleZ));
    }
}


