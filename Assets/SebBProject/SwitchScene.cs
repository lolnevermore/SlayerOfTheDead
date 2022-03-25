using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public string nextScene;
    private float timer = 31f; // 30 seconds of duration of cinematic but slight dleay - then transition to level 1 game scene. 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Application.LoadLevel(nextScene); // loads level1. 
        }

    }
}
