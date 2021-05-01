# AR Final Project: Sonification of Medical Procedures
**Students:** Ruby Liu, Disha Mishra, Liza Naydanova, Samantha Tam

**Mentors:** Alejandro Martin-Gomez and Michael Sommersperger

Our overall goal for this project was to sonify a subretinal injection procedure. **Sonification** is the use of non-speech audio to convey information or perceptualize data. 
Auditory perception has spatial, amplitude, frequency resolution that vision does not have, so sonification can be used to complement or replace visual cues. 

All the demo videos for our project can be found in a Google Drive folder [here](https://drive.google.com/drive/folders/161tLKwtKBPFnPOiE0MvRCIiiMQCtMvhs?usp=sharing).

Our project was developed in Unity version 2019.4.21f1. In theory, if you have that version, you should be able to download the two projects and hit the play button to run them.

We have two branches in this repository, which contain the two sonification implementations we developed.
## Pre-Retinal Injection
**Goal:** Guide the surgeons using sound to the correct leftwards/rightwards rotation for proper injection set-up 
**General set-up:** Assume corneal injection has occurred and needle is being prepared for insertion 
**Unity scene:** End of needle in front of retinal layer, starts in correct position, and alerts surgeons if it moves from that position 

For more information about the implementation, see the “preinjection_rotation” branch: https://github.com/LizaNaydanova/group2_sonification/tree/preinjection_rotation
## Subretinal Injection (Depth Sonification)
**Goal:** Guide the surgeons using sound to the correct injection depth in the retina
**General set-up:** Assume needle has been correctly inserted into the cornea and has the correct alignment  
**Unity scene:** End of needle in front of retinal layer Needle starts in correct orientation prior to entering the retina; uses different sounds
to signal different depth states. 

For more information about the implementation, see the “subretinal_injection” branch: https://github.com/LizaNaydanova/group2_sonification/tree/subretinal_injection
