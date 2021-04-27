using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NeedleControl : MonoBehaviour
{

    float speed = 0.8f;
    float pitchVal = 0.5f;
    Quaternion rotation;
    Quaternion needleCenter;
    public Camera mainCamera;
    public Camera sideCamera;
    public Transform customPivot;
    public AudioSource soundBitty;
    public AudioSource dingBitty;
    private float currTime;
    private float maxTime;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera.enabled = true;
	sideCamera.enabled = false;
        soundBitty = GetComponent<AudioSource>();
        soundBitty.pitch = pitchVal;
	dingBitty = GetComponent<AudioSource>();
	needleCenter = this.transform.rotation;
	currTime = 0;
	maxTime = 0.5f; // time in sec
     }

    // Update is called once per frame
    void Update()
    {

        rotation = this.transform.rotation;
        //prevPos = this.transform.position;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += transform.forward * Time.deltaTime * speed;
            //transform.Translate(0, Time.deltaTime * speed, 0);
         
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            //transform.Translate(0, -1 * Time.deltaTime * speed, 0);
            transform.position -= transform.forward * Time.deltaTime * speed;
	    // float distance = (go2.transform.position - go1.transform.position).magnitude;
        }

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

        // change camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }

        if (Input.GetKey(KeyCode.D)) // rightward angle
        {
            transform.RotateAround(customPivot.position, Vector3.up, speed * Time.deltaTime);

	    // code obtained from StackOverflow

	 }

	    else if (Input.GetKey(KeyCode.A)) // leftward angle
        {

            transform.RotateAround(customPivot.position, Vector3.down, speed * Time.deltaTime);
	}
	currTime += Time.deltaTime;
	if (currTime > maxTime) 
	{
		currTime = 0;
	if((transform.rotation.y - needleCenter.y) < 0){
		float err = -1 * Quaternion.Angle(needleCenter, transform.rotation);
		PlaySound(err);
	}
	else {
		float err = Quaternion.Angle(needleCenter, transform.rotation);
		PlaySound(err);
	}
	}
	
    }
    public void PlaySound(double error)
    {
	error = (error)/4;
	GetComponent<ChuckSubInstance>().RunCode(string.Format( @" // our patch
Shakers shake => JCRev r => dac;
// set the gain
.95 => r.gain;
// set the reverb mix
.025 => r.mix;

// our main loop

    // frequency..
    // note: Math.randomf() returns value between 0 and 1
    <<< {0} >>>;
    if( {0} < 0.0000 || {0} > 0.0000 )
    {{
        //Math.random2( 0, 22 ) => shake.which;
        //Std.mtof( Math.random2f( 0.0, 128.0 ) ) => shake.freq;
        //Math.random2f( 0, 128 ) => shake.objects;
        2 => shake.which;
        50 => shake.freq;
        1 => shake.objects;
	100 => shake.energy;
	8 => shake.preset;
        <<< shake.which(), shake.freq(), shake.objects() >>>;
	
    }}
    <<< {0} >>>;
    // shake it!
    //Math.random2f( 0.8, 1.3 ) => shake.noteOn;
    // note: Math.randomf() returns value between 0 and 1
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

    
        //else if( Math.randomf() > .05 )
    //{{ .125::second => now; }}
    //else 
    //{{
    if( {0} < 0 || {0} > 0)
    {{
        1 => int i => int pick_dir;
        // how many times
        //20 * {0} => float pick;
	3 => int pick;
        0.2 => float pluck;
        0.4 / pick => float inc;
        // time loop
        for( ; i < pick; i++ )
        {{
            75::ms => now;
            //Math.random2f(.2,.3) + i*inc => pluck;
	    0.25 + i*inc => pluck;
            pluck + -.2 * pick_dir => shake.noteOn;
            // simulate pluck direction
            !pick_dir => pick_dir;
        }}
    }}
        // let time pass for final shake
        //75::ms => now;
    /*}}*/", error) );
    }

    public void SwitchCamera()
    {
	mainCamera.enabled = !(mainCamera.enabled);
	sideCamera.enabled = !(sideCamera.enabled);
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

/*    
    void OnCollisionEnter( Collision collision )
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            soundBitty.Play();
	}
    }
*/

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