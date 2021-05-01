using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NeedleControl : MonoBehaviour
{

    float speed = 0.8f; // speed of the needle movement 
    Quaternion rotation; // rotation of needle
    Quaternion needleCenter; // correct needle center position 
    public Camera mainCamera; // front camera looking directly at the retina
    public Camera sideCamera;
    public Transform customPivot; // pivot that the needle rotates around
    private float currTime; // time passed (sec)
    private float maxTime; // max amount of time that can pass before updating rotation


    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
	sideCamera.enabled = false;
	needleCenter = this.transform.rotation; // update needle center 
	currTime = 0; // start time
	maxTime = 0.5f; // update max time 
     }

    // Update is called once per frame
    void Update()
    {
	mainCamera.enabled = true;
        rotation = this.transform.rotation; // update current rotation each time 
	

        // change camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }

	// Up and Down Arrow keys move the needle in and out of the retina 

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
         
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= transform.forward * Time.deltaTime * speed;

        }
	
	// Left and Right Arrow change the speed of needle movement
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (speed > 0)
            {
                speed-= 0.1f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            speed+=0.1f;
        }

	/* D and A keys allow angle movement towards left and right 
           rotation code obtained from StackOverflow
	*/
        if (Input.GetKey(KeyCode.D)) // rightward angle
        {
            transform.RotateAround(customPivot.position, Vector3.up, speed * Time.deltaTime);

	 }

	else if (Input.GetKey(KeyCode.A)) // leftward angle
        {
            transform.RotateAround(customPivot.position, Vector3.down, speed * Time.deltaTime);
	}
	// add time each time you update
	currTime += Time.deltaTime;
	// when time is greater than maxTime, check to see if sound should be played 
	if (currTime > maxTime) 
	{
		currTime = 0;
		if((transform.rotation.y - needleCenter.y) < 0)
		{
			float err = -1 * Quaternion.Angle(needleCenter, transform.rotation);
			PlaySound(err);
		}
		else {
			float err = Quaternion.Angle(needleCenter, transform.rotation);
			PlaySound(err);
		}
	}
	
    }
    public void SwitchCamera()
    {
	mainCamera.enabled = !(mainCamera.enabled);
	sideCamera.enabled = !(sideCamera.enabled);
    }


    /*  check to see if sound should be played
	code changed and obtained from ChucK website, written by 
        Ge Wang (gewang@cs.princeton.edu)
	Perry R. Cook (prc@cs.princeton.edu)
    */
    public void PlaySound(double error)
    {
	error = (error)/4; // normalize error (only looking at range of 4 degrees)
	GetComponent<ChuckSubInstance>().RunCode(string.Format( @" // our patch
	Shakers shake => JCRev r => dac;
	// set the gain
	.95 => r.gain;
	// set the reverb mix
	.025 => r.mix;

	// our main loop

    // add noises as threshold increases 
    if( {0} < 0.0000 || {0} > 0.0000 )
    {{
        2 => shake.which;
        50 => shake.freq;
        1 => shake.objects;
	100 => shake.energy;
	8 => shake.preset;
	
    }}
    // shake it!
    if( {0} < -0.05 || {0} > 0.05)
    {{ 
	300::ms => now; 
	112 => shake.freq;
	3.0 => shake.noteOn;
	5 => shake.objects;
	0 => shake.preset; // maracas
    }}

    if( {0} < -0.3 || {0} > 0.3 )
    {{ 
	210::ms => now;
	300 => shake.freq;
	4.0 => shake.noteOn;
	11 => shake.objects;
	12 => shake.preset; // coke can
    }}

    if( {0} < -0.6 || {0} > 0.6 )
    {{ 
	100::ms => now;
	376 => shake.freq;
	15.0 => shake.noteOn;
	21 => shake.objects;
	18 => shake.preset; // franc
    }}

    if( {0} < -0.9 || {0} > 0.9 )
    {{ 
	230::ms => now;
	500 => shake.freq;
	95.0 => shake.noteOn;
	71 => shake.objects;
	17 => shake.preset; // quarter

    }}
    if( {0} < -1.2 || {0} > 1.2 )
    {{ 
	100::ms => now;
	600 => shake.freq;
	15.0 => shake.noteOn;
	31 => shake.objects;
	10 => shake.preset; // wrench

    }}
    // code to pick and change direction 
    if( {0} < -1.2 || {0} > 1.2)
    {{
        1 => int i => int pick_dir;
        // how many times
	3 => int pick;
        0.2 => float pluck;
        0.4 / pick => float inc;
        // time loop
        for( ; i < pick; i++ )
        {{
            75::ms => now;
	    0.25 + i*inc => pluck;
            pluck + -.2 * pick_dir => shake.noteOn;
            // simulate pluck direction
            !pick_dir => pick_dir;
        }}
    }}
", error) );
    }


    void OnTriggerEnter(Collider other) // recognize collisions
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            other.gameObject.SetActive(false);
            //count++;
            //SetCountText();
        }
    }


}
public static class ExtendingVector3
{
    public static bool greater(this Vector3 local, Vector3 other)
    {
        if(local.x > other.x && local.y > other.y && local.z > other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool less(this Vector3 local, Vector3 other)
    {
        if(local.x < other.x && local.y < other.y && local.z < other.z)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}