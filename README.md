# Preinjection Rotation

## Approach
This approach deals with the rotation of the needle before the injection. In this case, upwards/downwards rotation can easily be detected, but leftwards/upwards rotation may not be. Here, we have a scene that takes in user input from the keyboard as means to move the needle and as the needle moves away from the center (ideal position), it will result in a sound with gradually greater complexity.

## Scene and Functionality
The initial Unity 3D scene with the two retinal meshes and needle were provided to us by our mentors. The final unity scene also has two camera views (main camera and side camera) that switch using the space bar. The up and down arrow keys are used to move the needles into and out of the retina layer with up as into and down as out of. The left and right arrow keys are used to adjust the speed that the needleis moving with left as a decrease of speed and right as an increase in speed. The A and D keys are used for the rotation of the needle with A rotating the needle more leftwards and D rotating the needle more rightwards.

## Functions

void Start(): initializes all elements for beginning of the game at play

void Update(): runs every frame and updates the needle movement as well as the timer

void SwitchCamera(): switches between the main camera and side camera view

void PlaySound(): plays sound according to the error distance from the current needle position to the needle center. As it gets further away, the complexity of the sound will increase.

void OnTriggerEnter(): recognizes collision
