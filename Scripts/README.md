# This directory holds various 3-D model scripts from Blender, Unity, or Unreal Engine
# Astronomical Sky Tracker

The Astronomical Sky Tracker is a Blender add-on designed for tracking and visualizing the motion of celestial bodies based on user inputs. This add-on creates a realistic simulation of stars, the Sun, the Moon, and planets in Blender's 3D environment.

## Features

- **Celestial Body Tracking**: Track the motion of various celestial bodies including the Sun, Moon, and planets.
- **Customizable Settings**: Users can input local time, date, geographical coordinates, and altitude for precise celestial positioning.
- **Dynamic Visualization**: Generate accurate representations of celestial bodies in Blender based on user-specified parameters.
- **Sky Texture Integration**: Append realistic sky textures to enhance the visual experience.

## Installation

1. Ensure you have Blender version 3.2.0 or newer installed.
2. Download the `Scientific and Precision Astronomical System` zip file.
3. Open Blender and go to `Edit > Preferences > Add-ons`.
4. Click `Install` and navigate to where you downloaded `Scientific and Precision Astronomical System`.
5. Select the file and click `Install Add-on`.
6. Enable the add-on by ticking the checkbox next to its name.

## Usage

1. Open a new blank blender file
2. Open the Astronomical Sky Tracker (SPAS) panel in the 3D Viewport's UI sidebar.
3. Set your local date, time, geographical coordinates, and altitude.
4. Choose the celestial bodies you wish to visualize (Sun, Moon, Planets).
5. Click `Initialize World` to set up the environment.
6. Click `Generate Celestial` to create the celestial bodies in Blender.
7. Adjust your view and settings as needed to explore the astronomical simulation.

## Notes

- It's recommended to use this add-on in an empty Blender project as existing elements might be affected.
- The add-on converts local time to Greenwich Mean Time (GMT) for calculations, considering user-input time zones.
- The generated celestial bodies are based on user inputs and real astronomical data for accurate positioning.

## Credits

- Developed by Michael Cai.
- Used code and help from B-MH from Peking University to help with some parts of the code
- This project utilizes astronomical algorithms from the `ephem` library for precise calculations of celestial positions.
