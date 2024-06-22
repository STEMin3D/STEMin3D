# Orbital Elements UI

## Steps to implement
1. **Download and Import the Package**
   - Right-click in the "Assets" panel.
   - Select "Import Package" -> "Custom Package".
   - Select `OrbitalElements_UI.unitypackage` and click "Open".
   - Click "Import" to add all the assets.
    
2. **Set Up the Scene**
   - Drag the `Canvas` prefab from the "Assets" panel into the "Hierarchy" panel.
   - Drag the `ScriptHandler` prefab from the "Assets" panel into the "Hierarchy" panel.
   - When prompted, click "Import TMP Essential".
   - Right-click in the "Hierarchy" panel and select "UI" -> "Event System" if one is not already present.

3. **Link the Submit Button**
   - Expand the `Canvas` prefab in the "Hierarchy" panel.
   - Drag the "Button" game object to the "Submit Button" field in the `ScriptHandler` component on the `ScriptHandler` prefab.
    
4. **Set Up Input Fields**
   - Expand the `ScriptHandler` prefab.
   - Drag each input field (a, e, i, o, w, v) to the `InputFields` list in the `Close` script in that order.

5. **Set Up Create Slider Script**
   - Drag the "Eccentricity" game object to the "Sliders Parent" field in the `CreateSlider` component on the `ScriptHandler` prefab.
  
6. **Configure Button Click Event**
   - Select the "Button" game object in the `Canvas`.
   - In the "On Click" section, drag the `ScriptHandler` game object to the "Runtime Only" field.
   - Select "No Function" -> "Close" -> "Submit".

7. **Run the Project**
   - Save the scene and press "Play" to test the functionality.

## Troubleshooting
**Input fields have no placeholder text**
Press the "Play" button to run the scene and then press it again to stop it. This should make the placeholder text in Inputfields appear.

**Button and Input fields don't work**
Insure that an "Event system" gameobject is present in the Hierarchy window. UI componets won't work correctly without one.
