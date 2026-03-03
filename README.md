# GDIM32-Final
## Check-In
### Frances Kim
1. Contributions
- Imported 3D assets I modeled into Unity and made them into Prefabs to set up the scene
- Made the terrain GameObject and set up the textures for different terrain colors
- Found a toon shader online and extracted the materials from the assets and applied the shader to them.
- Built the animation controller for the Squirrel NPC, the Traveler NPC, and the Player.
- Coded the player's movement in the Witch class. This was done with a Vector3 variable called direction which takes in the horizontal and vertical input. This variable is multiplied by a speed variable and Time.deltaTime to move the character controller. There's also some math stuff to smooth the rotation so  the character doesn't snap to the WASD directions. I also normalized the diagonal movement in direction so going diagonally wouldn't be faster
- Player plays interaction animation when picking up collectable
- Set up the camera and cinemachine camera for a top down-ish view of the player
- Set up a new scene that held the cutscene (which is composed of different panels overlayed on top of each other, drawn by Rebecca) and coded the cutscene function. This was done with a class called SlideTransition, which is placed on the canvas containing all the slides. Using a foreach statement, I added all of the child GameObjects of the Canvas to a list of slides, setting the first one active. A method called MovetoNextSlide()  iterates through the slides until the selectedSlide is at 10, when it uses the SceneManager to load the Main scene which contains the game. I set up a button that covered the whole screen which called MovetoNextSlide() when clicked on
- Added background music to the game by attaching an audio source to the player which plays a looping audio on start
- Made a typewriter effect by modifying Landon's dialogue bubble script with an IEnumerator IncreaseMaxVisibleChar(string dialogue) which adds onto how many characters textmeshpro shows over time. When the interact button is pressed again while the typing effect is active, it sets the bool fullText to true, which makes all the text show at once
- Added a sound effect to play during the typewriter effect with an audioclip that's called within the while statement in the IEnumerator
- Added two point lights to the oven to make a fire effect



2. Reflection

I think overall, the proposal and break-down were helpful. It gave us a good idea of what the game should look like as we developed it, including specific mechanics like the how the interactions should look. There are some things I think we still need to implement, as we don't have a locator in the game yet. I think adding one and cleaning up some of the code could make it less tightly coupled and make things clearer. We used a google sheets document to keep track of progress.







### Landon Her
I contributed to coding player interactions. PlayerInteractor, IInteractable, InteractPromptUI are a few I’ve contributed too. PlayerInteractor handles the detection of and interaction with NPCs and collectables using Physics.SphereCastAll. The class included variables similar to those seen in some of the recent in-class demos being rayOrigin, sphereRadius, and interactDistance. The breakdowns always help in giving me a place to start as they outline what scripts/objects we are going to have and what’s in them like their components for example. The proposal helped in the sense that it was a more structured form of the breakdown. It gave me the gist of what our game was gonna be about and its theme. It immersed me in a way. Whereas the breakdown gave me a point to start, the proposal gave me the next step. Thinking back on it though, our proposal wasn’t the most detailed, ending up with me having to address them later as I was coding. But like I said, the proposal gave me the gist and enough to get me started somewhere. I think planning the proposal in the way we do breakdowns would help improve my planning process for future projects. For the finite state machine in the proposal for example, it would’ve helped if we listed everything that would have a finite state machine and what states each thing would have in a bullet point like list or something that’s easy to read.

### Team Member Name 3
Put your individual check-in Devlog here.


## Final Submission
### Group Devlog
Put your group Devlog here.


### Team Member Name 1
Put your individual final Devlog here.
### Team Member Name 2
Put your individual final Devlog here.
### Team Member Name 3
Put your individual final Devlog here.

## Open-Source Assets
Cite any open-source assets here. Put them in a LIST, and use correctly formatted LINKS.
