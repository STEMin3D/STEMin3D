# How to run Shadow Cones
1. Place SystemManager.cs script on Main Camera or separate GameObject component.
   - The SystemManager is in charge of sizing all the different planets as well as their orbits.
   - Assign a sun, moon, earth, and light source to the script.
   - Default values regarding scaling and speed have been given, but feel free to change as you see fit.
2. Place ConeMaker.cs script on Main Camera or separate GameObject component.
   - The ConeMaker is in charge of displaying the shadow cones. 
   - Assign a sun, moon, and earth to the script.
   - Also provide a shadow cone prefab. One has been provided under Assets/ShadowConePrefab.
3. Place FreeFlyCamera.cs script on the Main Camera.
   - FreeFlyCamera is in charge of allowing the user to move around the scene. 
   - All properties are self explanatory, so feel free to change as neccessary. 

Note: If you want to toggle different eclipses, call the ToggleEclipse function in ConeMaker.cs. An example is given in Scenes/simple_system_demo.unity.