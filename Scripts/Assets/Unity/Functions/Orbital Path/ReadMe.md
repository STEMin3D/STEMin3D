# Displaying an Orbital Path

## Steps to implement

1. **Download and Import the Package**
   - Right-click in the "Assets" panel.
   - Select "Import Package" -> "Custom Package".
   - Select `OrbitalTrail.unitypackage` and click "Open".
   - Click "Import" to add the particle system.
    
2. **Set Up the trail**
   - Drag the `Orbital trail` prefab from the "Assets" panel into the "Hierarchy" panel.
   - Make the `Orbital trail` prefab a child of the gameobject you want to have a trail.

3. **Run the Project**
   - Save the scene and press "Play" to test the functionality.
   - The trail won't show up unless the gameobject is moving.
   - The trail won't show up in the scene view, if the `Orbital trail` is a child of some gameobject.

## Result
As the parent of the `Orbital trail` prefab moves around in the game view, a blue trail should appear behind the gameobject and disappear after a few seconds.
