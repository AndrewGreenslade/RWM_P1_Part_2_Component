using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePlayerControllerAnims : MonoBehaviour
{
    private AnimationManager manager;

    private void Start()
    {
        manager = GetComponent<AnimationManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (manager.currentAnimation.frameName != "Walk") 
            { 
                manager.ChangeAnimEvent.Invoke("Walking");
            }
        }
        else
        {
            if (manager.currentAnimation.frameName != "Idle")
            {
                manager.ChangeAnimEvent.Invoke("Idle");
            }
        }
    }
}
