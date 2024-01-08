"""
    This is a Blender project developed by Michael Cai and used the code of BMH and YuXiZa to help. Part of this project was purchased on Blender Market
    with order number cu0d6q (#1786437).
    
    Please do not reprint, copy, or resell without permission.

    The below comments were made by BMH and YuXiZa

    It has been made possible through published resources like the SAO Star Catalog, Planetary and Lunar 3d-model from NASA, as well as open-source Python 
package "pyephem" and algorithms. We have noticed that there are already excellent astronomical computing and simulating programs such as Stellarium, 
but it seems that no one has ported them to Blender, making it difficult to meet the need to obtain real sky backgrounds. We believe that such needs are 
always existing, for instance, the movie special effect of Titanic has been pointed out by astronomer Neil deGrasse Tyson for applying wrong background 
starry sky; moreover, for users performing some night scenes, aviation, and Low Earth Orbit projects, using correct and accurate star background is always 
the icing on the cake.
    This project is planned to be released in an open-source format, and the code structure and implementation methods of the project may be explained 
in the form of documents or videos later. We sincerely appreciate every user of the add-on, and developer who has proposed optimization ideas for the code 
of this add-on.
    This is the first version of the add-on, we believe its functionality and inclusion of celestial bodies are limited, there are also some possibilities 
for code optimization. So in the future, we are continually developing this project according to the increasing needs and release new versions. Stay tuned!
"""
# -*- coding: UTF-8 -*-
import os
import sys
import bpy

# Import submodules one by one
from . import A_Generate_UI

bl_info = {
    "name" : "Astronomical System Tracker", 
    "author" : "Michael Cai",
    "description" : "Tracks the motion of the stars and solar system given user input.",
    "blender" : (3, 2, 0),
    "version" : (0, 1, 0),
    "location" : "",
    "warning" : "Only Use with NEW BLENDER FILE (BLANK)! Please ensure that you have thoroughly read the document's guidelines before using the add-on",
    "category" : "Generic",
    "doc_url" : "https://github.com/STEMin3D/STEMin3D/tree/main/Scripts", 
}

# Except for a, which needs to be registered here because it has parameters to read in and out, other classes do not have this need.
def register():
    print("SPAS started successfully.")
    currentPyFilePath = os.path.abspath(__file__) # The path to the current .py file
    parentPyFilePath = os.path.dirname(currentPyFilePath) # The folder where the current .py file is located (all the files of the entire plug-in are here)
    sys.path.append(parentPyFilePath) # Add the current folder to the system path of Blender Python

    print(f'Current absolute path is: {os.path.abspath(__file__)}') # Check the current path and use it later to make the path absolute.
    # print(f'sys.path is: {sys.path}') # Check the system path of Blender Python
    print('Parent path has been appended to sys.path\n')
    A_Generate_UI.register()
    ...

def unregister():
    A_Generate_UI.unregister()
    print("SPAS terminated successfully.")
    ...

# Activate while debugging
'''
if __name__ == "__main__":
    register()
'''