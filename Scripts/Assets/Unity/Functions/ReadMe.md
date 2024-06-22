# Orbital Elements UI

## Steps to implement
1. Download the package.
2. Open up a Unity project in HDRP.
3. Right click and select 'Import package' -> 'custom package'.
4. Import OrbitalElements_UI.
5. Go to the assets folder and the drag the canvas and ScriptHandler prefabs to the Hierarchy window.
6. Import TextMesh Pro (TMP) Essentials if you haven't already.
7. Add an event system.
8. Expand the canvas and drag the 'Button' gameobject from the Hierarchy to the Submit Button field in the inspector of the ScriptHandler.
9. In the Close script component on the ScriptHandler GameObject, expand the Input Fields list.
10. Drag the Inputfields from the hierarchy window into the list in this order: a, e, i, o, w, v.
11. In the CreateSlider script component on the ScriptHandler GameObject, drag the Eccentricity GameObject from the Hierarchy to the Sliders Parent field.
12. Select the Button GameObject in the Hierarchy, and scroll down to the On Click () section in the Inspector.
13. Drag the ScriptHandler GameObject to the Runtime Only field.
14. Select 'No Function' -> 'Close' -> 'Submit'.
