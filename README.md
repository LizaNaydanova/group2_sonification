# AR Final Project: Sonification of Medical Procedures
Approach 2: Subretinal Injection (Sonification of Depth)
Developed by Ruby Liu and Liza Naydanova

## Requirements
Our approach was developed in Unity version 2019.4.21f1, used alongside Visual Studio Community 2019. Unity must be installed to run our project, which should include Visual Studio. 

## Development
We have installed the Input System and Chunity packages in the project. 
The Chunity package is for the sound system; the package and instructions to install and use can be found here: http://chuck.stanford.edu/chunity/tutorials/ 

Our mentors Alejandro Martin-Gomez and Michael Sommersperger provided us with the initial Unity 3D scene with the two retinal meshes and needle. We added a 3D block as a collider and an empty game object at the needle tip, using the distance between the two to calculate the needle’s current position relative to the red mesh. 

![Unity scene setup.](https://cdn.discordapp.com/attachments/756716713663528961/837718009841188884/unknown.png)

We used a script to add various functions to the needle. This script, NeedleControl, is our only script for the project. It allows the user to control the movement and speed of the needle and change the camera; the script also updates the UI and determines what sound to play depending on the needle’s current position.

The script has the following functions:
- **Start:** initialize cameras and speed text
- **Update:** detect user input to move needle (up/down arrows), change speed (left/right arrows), switch camera (spacebar), or show/hide distance (D key). It also determines the needle’s current position and calls functions to update the UI and play sounds. 
- **playSound:** Play the sonification sound, which changes depending on distance. 
  - Gets the distance factor and uses it to determine which section the needle is in. 
  - In the Approaching Target and Past Target sections, the program calculates the frequency and pitch depending on the distance factor. Frequency and pitch decrease as the target is approached, and then they increase with a different sound as the needle moves past the target. 
  - The intention with these sounds was to have an informative sound to find the target, a “pleasant” sound at the target, and alarming sounds as the user went into dangerous zones (i.e. nearing and past the RPE). 
  - This function also calculates the sound delay based on the distance, which determines how much time must pass before another sound plays.
  - A different sound (.wav file) is played for each section, as seen in the image below: 

![Sounds of different needle positions.](https://cdn.discordapp.com/attachments/756716713663528961/837721186175418428/unknown.png)
- **bellSound, alarmSound, veryAlarmSound, ambSound:** different functions that each play a different sound in a similar manner. If enough time has passed since the last sound was played, the function will update the time tracker and use intensity/pitch variables (which are taken as function arguments for bellSound and alarmSound) to alter the sound being played. 
- **SetSpeedText:** Set the text to the needle's speed factor. Uses a global variable that is updated in Update(). 
- **SetInstructions:** Set the instruction text depending on which camera is active.
- **getDistance:** Get the distance of the needle tip to the red mesh in Unity units. Uses hard-coded red mesh distance for the calculation.
- **getAbsoluteDistance:** Get the distance of the needle tip to the collider object in the Unity scene, in Unity units. Used for getting the distance of the yellow and red meshes, which were in turn hard-coded into getDistance and distFactor. 
- **distFactor:** Get the "distance factor" of the needle tip's current position, which goes from 1 at the start of the yellow mesh to 0 at the start of the red mesh.
- **SwitchCamera:** Switch the active camera. 

## Operation
To run this project, open the project in Unity. Press the “play” icon to enter Game mode. 

### User-Interface
On the UI screen, the interactive elements are described:

![UI Screen](https://media.discordapp.net/attachments/837727729897046029/837727779356934174/179984789_2846923262234073_847794144307321709_n.png)  


The default camera is of the front view of the scene. The speed with which the needle moves is indicated. The user can interact with the system through the following keyboard controls:
* **UP/DOWN arrow keys**: these control the direction in which the needle moves. **UP** moves the needle into the retinal tissue along the axis of the needle. **DOWN** moves the needle backwards out of the retinal tissue
* **LEFT/RIGHT arrow keys**: these control the speed with which the needle moves when advancing or retreating it. **LEFT** decreases the speed and **RIGHT** increases the speed. Changes to the speed are reflected by the displayed speed value
* **SPACE key**: this controls the camera view. There are two camera views: the front view seen in the screenshot above, and the side view, which shows how far the needle is advanced into the tissue:

![Side view of model](https://cdn.discordapp.com/attachments/756716713663528961/837728326985842749/180534237_324576425679213_1534438114250488495_n.png) 


* **D key**: this is a toggle for showing distance. The distance is a metric of how far the tip of the needle is away from the red layer. The distance doesn’t have units, but is simply a relative metric calculated by Unity based on the sizes of our models



