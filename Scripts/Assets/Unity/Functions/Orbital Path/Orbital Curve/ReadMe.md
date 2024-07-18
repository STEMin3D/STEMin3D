# Orbital Curve

## Objective

The user will be able to create a curve to represent the orbital path of an object. The orientation of the curve will depend on the inputted orbital elements.

## Steps to implement

1. **Download and Import the Package**
   - Right-click in the "Assets" panel.
   - Select "Import Package" -> "Custom Package".
   - Select `OrbitalCurve.unitypackage` and click "Open".
   - Click "Import" to add all the assets.
    
2. **Create the GameObject**
   - Right-click in the "Hierarchy" panel.
   - Click "Create Empty".

3. **Add the line renderer**
   - Click the "Add Component" button in the "Inspector" panel.
   - Type `Line Renderer` and select it when it shows up.
    
4. **Add the script**
   - Click the "Add Component" button in the "Inspector" panel.
   - Type `Orbit Path` and select it when it shows up.

5. **Add the material**
   - Drag and drop the `Blue` material to the bottom of the "Inspector" panel.

6. **Run the Project**
   - Save the scene and press "Play" to test the functionality.

## How to modify
 - Change the `Semi Major Axis` to change the length of the path.
 - Change the `Eccentrcity` to change how circular the path is.
 - Change the `Inclination` to change the tilt of the path.
 - Change the XYZ values of the `Offset` variable to change the center of the path.
 - To change the color of the curve, create a new material with the prefered color and replace the `Blue` material with your own.
 - To change the width of the curve, grab the node located in the "Positions" section of the `Line Renderer` component and move it up or down.
