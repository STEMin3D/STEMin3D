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
   - Drag the "Button" game object to the "Submit Button" field in the `Close` script on the `ScriptHandler` prefab.
    
4. **Set Up Input Fields**
   - Expand the `Input Fields` list in the `Close` script.
   - Drag each input field (a, e, i, o, w, v) to the `InputFields` list in the `Close` script in that order.

5. **Set Up Create Slider Script**
   - Drag the "Eccentricity" game object to the "Sliders Parent" field in the `CreateSlider` script on the `ScriptHandler` prefab.
   - Drag the `ValueText` prefab to the "Value Text Prefab" field in the `CreateSlider` script on the `ScriptHandler` prefab.
  
6. **Configure Button Click Event**
   - Select the "Button" game object in the `Canvas`.
   - In the "On Click" section, drag the `ScriptHandler` game object to the "Runtime Only" field.
   - Select "No Function" -> "Close" -> "Submit".

7. **Run the Project**
   - Save the scene and press "Play" to test the functionality.

## Troubleshooting
**Input fields have no placeholder text**
   - Press the "Play" button to run the scene and then press it again to stop it. This should make the placeholder text in Inputfields appear.

**Button and Input fields don't work**
   - Insure that an "Event system" gameobject is present in the Hierarchy window. UI componets won't work correctly without one.

**Sliders don't appear when the button is clicked**
   - Make sure that the gameobject named "Eccentricity" is used for the `SlidersParent`. If the eccentricity input field is used instead, the sliders won't appear as they won't have a `SlidersParent`.

## Result
The finished UI should look and work as follows:

   1. In the bottom right-hand side of the screen, there are six input fields with the symbol of its Orbital Element next to it. Below each input field, there should be a grayed-out slider. Bellow that, their is a button labled 'Set.'
   2. The user will then type out the value of each Orbital Element in the respective input field.
   3. Once the 'Set' button is pressed, the input fields, the 'Set' button, and the grayed-out sliders will disappear and be replaced with active sliders set to the inputted values, with their current values displayed right above them.
   4. The user can then move the sliders to adjust the values.

## How to access the Slider Values
To acces the values from the sliders in your own scripts, you can use the following example script:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueReader : MonoBehaviour
{
    // Reference to the sliders
    public Slider semiMajorAxisSlider;

    void Update()
    {
        // Access the current value of the slider
        float semiMajorAxis = semiMajorAxisSlider.value;

        // Do something with the values
    }
}
