# MetalSlugX-QLearning
I will try to apply QLearning for an agent to play Metal Slug X

Here is an image of the game that will be processed, which contains the important values for the reward/punishment system.

![alt tag](https://github.com/IsmailAlaouiAbdellaoui/MetalSlugX-QLearning/blob/master/Demo.JPG)

* RED : number of lifes remaining
* BLUE : current score
* YELLOW : number of hostages rescued

Range of possible actions : LEFT,RIGHT,UP,DOWN,FIRE,BOMB,JUMP ( 7 )

There are 2 ways to get important values : OCR or Memory Address Scanning ( easiest option will be used )

Dependencies :
* pynput
* opencv
* csume emulator
* Metal Slug X iso
* Tesseract OR memorpy

Methodology : 
* Open the game executable
* Send inputs to go to main menu and choose the character
* Display the game and do a grayscale transformation using OpenCV
* Inside the loop displaying the game, perform QLearning algorithm

