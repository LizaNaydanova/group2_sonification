# Preinjection Rotation

## Approach
This approach deals with the rotation of the needle before injection into the retina, but assumes that the needle has already been inserted into the cornea / sclera. In this case, upwards/downwards rotation of the needle before retinal injection can easily be seen by the surgeon, but leftwards/rightwards rotation is less clear and more difficult to discern. In order to give the surgeons better feedback and to guide them to the correct left/right rotation, we used the concept of sonification. In our project, we have a scene that takes in user input from the keyboard as a means to move the needle and as the needle moves away from the center (the set ideal position), it will result in a sound with gradually greater complexity.

## Scene and Functionality
The initial Unity 3D scene with the two retinal meshes (RetinalLayers) and needle (41G Injection Needle) were provided to us by our mentors. The final unity scene also has two camera views (main camera and side camera) that switch using the space bar. The up and down arrow keys are used to move the needles into (using the up key) and out (using the down key) of the retina layer. The left and right arrow keys are used to adjust the speed that the needle is moving with (by 0.1 mm/s with each key press) where the left key decreases speed and right as an increase in speed. The A and D keys are used for the rotation of the needle with A rotating the needle more leftwards and D rotating the needle more rightwards.

In order to allow for a leftwards and rightwards rotation to occur, we created a pivotPoint game object so that the needle could pivot and perform leftward / rightward rotations around the Needle Sclera Point, which represents the point at which the needle intersects with the sclera / cornea of the eye. The pivot point makes sure that the rotation only happens at the part of the needle that has been inserted into the sclera as it is dangerous to move the part of the needle that is not inserted into the eye because that may cause tears in the sclera. Thus, the needle must pivot at the sclera, allowing the surgeon to rotate before retinal insertion.

Additionally, to perform the sonification, the ChucK programming language was used and an object named TheChuck was added to help perform robust sonification. To do so, ChucK had to be downloaded into the assets, which was done using the setup directions here: http://chuck.stanford.edu/chunity/tutorials/ 

Finally to put the rotation and perform sonification, we wrote a script called NeedleControl to control the 41G Injection Needle.

## Functions in NeedleControl Script

void Start(): initializes all elements for beginning of the game at play

void Update(): runs every frame and updates the needle movement as well as the timer

void SwitchCamera(): switches between the main camera and side camera view

void PlaySound(): plays sound according to the error distance from the current needle position to the needle center. As it gets further away, the complexity of the sound will increase.

void OnTriggerEnter(): recognizes collision
