# Orbital Elements UI

## Steps to implement

1. **Open Unity Project in HDRP**
   - Ensure you have a Unity project set up with the High Definition Render Pipeline (HDRP).

2. **Download and Import the Package**
   - Right-click in the "Assets" panel.
   - Select "Import Package" -> "Custom Package".
   - Select `OrbitalElements_UI.unitypackage` and click "Open".
   - Click "Import" to add all the assets.
    
3. **Set Up the Scene**
   - Drag the `Canvas` prefab from the "Assets" panel into the "Hierarchy" panel.
   - Drag the `ScriptHandler` prefab from the "Assets" panel into the "Hierarchy" panel.
   - When prompted, click "Import TMP Essential".
   - Right-click in the "Hierarchy" panel and select "UI" -> "Event System" if one is not already present.

4. **Link the Submit Button**
   - Expand the `Canvas` prefab in the "Hierarchy" panel.
   - Drag the "Button" game object to the "Submit Button" field in the `Close` script on the `ScriptHandler` prefab.
    
5. **Set Up Input Fields**
   - Expand the `Input Fields` list in the `Close` script.
   - Drag each input field (a, e, i, o, w, v) to the `InputFields` list in the `Close` script in that order.

6. **Set Up Create Slider Script**
   - Drag the "Eccentricity" game object to the "Sliders Parent" field in the `CreateSlider` script on the `ScriptHandler` prefab.
   - Drag the `ValueText` prefab to the "Value Text Prefab" field in the `CreateSlider` script on the `ScriptHandler` prefab.
  
7. **Configure Button Click Event**
   - Select the "Button" game object in the `Canvas`.
   - In the "On Click" section, drag the `ScriptHandler` game object to the "Runtime Only" field.
   - Select "No Function" -> "Close" -> "Submit".

8. **Run the Project**
   - Save the scene and press "Play" to test the functionality.

## Troubleshooting
**Input fields have no placeholder text**
   - Press the "Play" button to run the scene and then press it again to stop it. This should make the placeholder text in Inputfields appear.

**Button and Input fields don't work**
   - Insure that an "Event system" gameobject is present in the Hierarchy window. UI componets won't work correctly without one.

**Sliders don't appear when the button is clicked**
   - Make sure that the gameobject named "Eccentricity" is used for the `SlidersParent`. If the eccentricity input field is used instead, the sliders won't appear as they won't have a `SlidersParent`.
