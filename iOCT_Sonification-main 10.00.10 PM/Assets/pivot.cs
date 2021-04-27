using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pivot : MonoBehaviour
{
    //Vector3 position;
    public Transform customPivot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
	if (Input.GetKey(KeyCode.W)) // upward angle
        {
	    // code obtained from StackOverflow
            transform.RotateAround(customPivot.position, Vector3.left, 2* Time.deltaTime);
	}
	else if (Input.GetKey(KeyCode.S)) // downward angle
        {

            transform.RotateAround(customPivot.position, Vector3.right, 2* Time.deltaTime);
	}
    }
}
