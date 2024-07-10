# How to run Shadow Cones
1. Install ProBuilder from Unity package manager.
   - This is to ensure the cones render properly.
2. Copy the "./Scripts" folder and the "./ShadowConePrefab" folder to your project.
   - If you want to use our example planets copy the "./Planet Prefabs" folder and the "./Planets of the Solar System 3D" folder to your project.
3. Place "./Scripts/SystemManager.cs" script on Main Camera or separate GameObject component.
   - The SystemManager is in charge of sizing all the different planets as well as their orbits.
   - Assign a sun, moon, earth, and light source to the script. Make sure these are all in the scene.
     - Feel free to use the prefabs given in the "./Planet Prefabs" folder.
   - Default values regarding scaling and speed have been given, but feel free to change as you see fit.
4. Place "./Scripts/ConeMaker.cs" script on Main Camera or separate GameObject component.
   - The ConeMaker is in charge of displaying the shadow cones. 
   - Assign a sun, moon, and earth to the script. Make sure these are all in the scene.
   - Also provide a shadow cone prefab. One has been provided under "./ShadowConePrefab" (make sure this is from the project window, not the hierarchy).
5. Place "./Scripts/FreeFlyCamera.cs" script on the Main Camera.
   - FreeFlyCamera is in charge of allowing the user to move around the scene. 
   - All properties are self explanatory, so feel free to change as neccessary. 
   - Use left click to rotate camera and left click+WASD to move around You can also hold left click+QE to move up and down. Use mouse scroll wheel to zoom.

Note: If you want to toggle different eclipses, call the "ToggleEclipse" function in "./Scripts/ConeMaker.cs". An example is given in "./Scenes/simple_system_demo.unity".