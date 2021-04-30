using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class NeedleControl : MonoBehaviour
{

    public TextMeshProUGUI speedText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI distText;
    public GameObject tip;
    public GameObject backBox;
    public Camera mainCamera;
    public Camera sideCamera;
    public int startingPitch = 1;
    public int timeToDecrease = 5;

    float speed = 1;
    float nextSound = 0.0f;
    float soundDelay = 0.5f;
    float ambDelay = 3.5f;
    float nextAmb = 0.0f;

    bool showDist = false;

    // Start is called before the first frame update
    void Start()
    {
        SetSpeedText();
        mainCamera.enabled = true;
        sideCamera.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {

        //if (mainCamera.enabled)
        //{
            // move needle
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.Translate(-1 * Vector3.forward * Time.deltaTime * speed);
            }

            // change speed
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (speed <= 1)
                {
                    speed /= 2;
                    SetSpeedText();
                }
                else
                {
                    speed--;
                    SetSpeedText();
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (speed < 1)
                {
                    speed *= 2;
                    SetSpeedText();
                }
                else
                {
                    speed++;
                    SetSpeedText();
                }

            }
        //}

        // change camera
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
            speedText.text = "";
            SetInstructions();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            showDist = !showDist;
        }

        double distFac = distFactor();
        if (distFac < 1)
        {
            playSound();
        }

        if (showDist)
        {
            float dist = (float)getDistance();
            distText.text = "Dist: " + (Mathf.Round(dist * 100) / 100.0f).ToString();
        }
        else
        {
            distText.text = "";
        }
        

    }

    /// <summary>
    /// Play the sonification sound, which changes depending on distance.
    /// </summary>
    void playSound()
    {
        float distFac = 1 - (float)distFactor();
        float pitch = 0;
        if (distFac < 0.65)
        {
            // play bell 
            // frequency increases as you get closer to red
            soundDelay = 0.5f - .25f * (1 - (float)distFac);
            pitch = -0.5f / 0.7f * distFac + 1;
            bellSound(pitch);
        } 
        else if (distFac < 0.7)
        {
            // play pleasant sound
            //soundDelay = 1;
            //pitch = -0.5f / 0.7f * distFac + 1;
            //bellSound(pitch, 0.25f);
            ambSound();
        }
        else if (distFac < 1)
        {
            // play alarming sound
            // frequency increases as you get closer to red
            soundDelay = 0.5f - .25f * ((float)distFac - .5f);
            soundDelay /= 2;
            pitch = 0.5f / 0.3f * distFac + (0.5f - 0.35f / 0.3f);
            alarmSound(pitch);
        }
        else // greater than 1, past red
        {
            soundDelay = .4f;
            veryAlarmSound();
        }


    }

    /// <summary>
    /// Play the bell sound, which corresponds to the first needle zone.
    /// </summary>
    /// <param name="pitch">The factor to change the pitch.</param>
    /// <param name="intensity">The loudness of the sound.</param>
    void bellSound(float pitch, float intensity=0.5f)
    {
        // only play if enough time has passed
        if (Time.time > nextSound)
        {
            nextSound = Time.time + soundDelay;
            float distFac = 1 - (float)distFactor();

            // float pitch = 0.5f + Mathf.Abs(0.5f - distFac);

            GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
		        SndBuf impactBuf => dac;
		        me.dir() + ""BellFXs.wav"" => impactBuf.read;

		        // start at the beginning of the clip
		        0 => impactBuf.pos;
	
		        // set rate: least intense is fast, most intense is slow; range 0.4 to 1.6
		        {0} => impactBuf.rate;

		        // set gain: least intense is quiet, most intense is loud; range 0.05 to 1
		        0.05 + 0.95 * {1} => impactBuf.gain;

		        // pass time so that the file plays
		        impactBuf.length() / impactBuf.rate() => now;

	        ", pitch, intensity));

        }
        
    }

    /// <summary>
    /// Play the alarming sound, which corresponds to the latter needle zone before crossing the red mesh.
    /// </summary>
    /// <param name="pitch">The factor to change the pitch.</param>
    void alarmSound(float pitch)
    {
        // only play if enough time has passed
        if (Time.time > nextSound)
        {
            nextSound = Time.time + soundDelay;
            float intensity = 0.5f;
            float distFac = 1 - (float)distFactor();

            // pitch = 1 + pitch;

            GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
		        SndBuf impactBuf => dac;
		        me.dir() + ""AlarmFX.wav"" => impactBuf.read;

		        // start at the beginning of the clip
		        0 => impactBuf.pos;
	
		        // set rate: least intense is fast, most intense is slow; range 0.4 to 1.6
		        {0} => impactBuf.rate;

		        // set gain: least intense is quiet, most intense is loud; range 0.05 to 1
		        0.05 + 0.95 * {1} => impactBuf.gain;

		        // pass time so that the file plays
		        impactBuf.length() / impactBuf.rate() => now;

	        ", pitch, intensity));

        }

    }

    /// <summary>
    /// Play the very alarming sound, which corresponds to the zone after crossing the red mesh.
    /// </summary>
    void veryAlarmSound()
    {
        // only play if enough time has passed
        if (Time.time > nextSound)
        {
            nextSound = Time.time + soundDelay;
            float intensity = 0.5f;
            float pitch = 1;

            GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
		        SndBuf impactBuf => dac;
		        me.dir() + ""VeryAlarmFX.wav"" => impactBuf.read;

		        // start at the beginning of the clip
		        0 => impactBuf.pos;
	
		        // set rate: least intense is fast, most intense is slow; range 0.4 to 1.6
		        {0} => impactBuf.rate;

		        // set gain: least intense is quiet, most intense is loud; range 0.05 to 1
		        0.05 + 0.95 * {1} => impactBuf.gain;

		        // pass time so that the file plays
		        impactBuf.length() / impactBuf.rate() => now;

	        ", pitch, intensity));

        }

    }

    /// <summary>
    /// Play the ambient sound, which corresponds to the "good zone."
    /// </summary>
    void ambSound()
    {
        // only play if enough time has passed
        if (Time.time > nextAmb)
        { 
            nextAmb = Time.time + ambDelay;
            float intensity = 0.5f;
            float pitch = 2f;
            

            GetComponent<ChuckSubInstance>().RunCode(string.Format(@"
		        SndBuf impactBuf => dac;
		        me.dir() + ""AmbienceFX.wav"" => impactBuf.read;

		        // start at the beginning of the clip
		        0 => impactBuf.pos;
	
		        // set rate: least intense is fast, most intense is slow; range 0.4 to 1.6
		        {0} => impactBuf.rate;

		        // set gain: least intense is quiet, most intense is loud; range 0.05 to 1
		        0.05 + 0.95 * {1} => impactBuf.gain;

		        // pass time so that the file plays
		        impactBuf.length() / impactBuf.rate() => now;

	        ", pitch, intensity));

        }

    }

    /// <summary>
    /// Set the text to the needle's speed factor.
    /// </summary>
    void SetSpeedText()
    {
        speedText.text = "Speed: " + speed.ToString();
    }

    /// <summary>
    /// Set the instruction text depending on which camera is active.
    /// </summary>
    void SetInstructions()
    {
        if (mainCamera.enabled)
        {
            instructionText.text = "UP: Forward\n" 
                                   + "DOWN: Backward\n"
                                   + "LEFT: Decrease Speed\n"
                                   + "RIGHT: Increase Speed\n\n"

                                   + "SPACE: Switch Camera\n"
                                   + "D: Show/Hide Distance";
        }
        else
        {
            instructionText.text = "SPACE: Switch Camera\n"
                                   + "D: Show/Hide Distance"; ;
        }
    }

    /// <summary>
    /// Get the distance of the needle tip to the red mesh in Unity units. 
    /// </summary>
    /// <returns>
    /// The distance.
    /// </returns>
    double getDistance()
    {
        // Distance of yellow mesh: 292.5874
        // Distance of red mesh: 280.2726
        // Distance of optimal location: 283.98
        double redDist = 280.2726;
        var collider = backBox.GetComponent<Collider>();
        Vector3 closest = collider.ClosestPoint(tip.transform.position);
        double distance = Vector3.Distance(tip.transform.position, closest) * 100 - redDist;

        return distance;
    }

    /// <summary>
    /// Get the distance of the needle tip to the collider object in the Unity scene, in Unity units. 
    /// </summary>
    /// <returns>
    /// The distance.
    /// </returns>
    double getAbsoluteDistance()
    {
        var collider = backBox.GetComponent<Collider>();
        Vector3 closest = collider.ClosestPoint(tip.transform.position);
        double distance = Vector3.Distance(tip.transform.position, closest) * 100;

        return distance;
    }

    /// <summary>
    /// Get the "distance factor" of the needle tip's current position, which goes from 1 at the start of the yellow
    /// mesh to 0 at the start of the red mesh.
    /// </summary>
    /// <returns>
    /// The distance factor.
    /// </returns>
    double distFactor()
    {
        // Distance of yellow mesh: 292.5874
        // Distance of red mesh: 280.2726
        double yellowDist = 292.5874;
        double redDist = 280.2726;
        var collider = backBox.GetComponent<Collider>();
        Vector3 closest = collider.ClosestPoint(tip.transform.position);
        double distFac = (Vector3.Distance(tip.transform.position, closest) * 100 - redDist) / (yellowDist - redDist);

        return distFac;
    }

    /// <summary>
    /// Switch the active camera. 
    /// </summary>
    public void SwitchCamera()
    {
        mainCamera.enabled = !(mainCamera.enabled);
        sideCamera.enabled = !(sideCamera.enabled);
    }

}
