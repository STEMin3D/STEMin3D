# -*- coding: UTF-8 -*-
# Comments are translated to Chinese to English. Please correct if there are any translational errors.
import bpy
import os
import csv
import math
from . import ephem as ep

# By adding the celestial body from the pre-constructed model-node library (3DMODMODELAPEND.BLEND) to achieve the seven major planets of the sun, the moon, except the earth, the creation of Pluto Star, the creation of Pluto Star 
 # The file contains the model of the celestial body, the material node, and the geometric nodes used for alignment.

# Body model path
# APPEND_BLEND = "./3DModelScale.blend"

# Regarding the calculation position of celestial bodies, the category of diameter, the polar axis, etc.
class classE:
    # Static database
    planet_list = ["moon","mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"] # The list of celestial bodies provided
        # Here is a polar axis (self -turning axis, local+Z axis in Blender in Blender) under the E "of the Earth J2000.0 
         # The method of acquisition is to set the position directly as the celestial body at Stellarium, and obtain the star of the North Celestial Polar, that is, "the Arctic Star in the perspective of the celestial body" 
         # The redness of the redness of the "Earth Coordinates J2000.0 Era", instead of the redness of the Red Classic of the "Celestial Equivalence", otherwise it can only be arbitrarily at the redness, 90 ° in redtrayal, 90 °

    polar_axis = {"sun":['19:02:55',63.9],
                  "moon":['17:54:29',67.7],
                  "mercury":['18:44:10',61.5],
                  "venus":['18:10:56',67.2],
                  "mars":['21:10:00',53.0],
                  "jupiter":['17:51:42',64.4],
                  "saturn":['2:45:16',83.6],
                  "uranus":['17:09:19',-15.3],
                  "neptune":["19:57:30",43.0],
                  "pluto":["08:52:21",-6.3]} 

    bodies = {}
    bodies["sun"] = ep.Sun()
    bodies["moon"] = ep.Moon()
    bodies["mercury"] = ep.Mercury()
    bodies["venus"] = ep.Venus()
    bodies["mars"] = ep.Mars()
    bodies["jupiter"] = ep.Jupiter()
    bodies["saturn"] = ep.Saturn()
    bodies["uranus"] = ep.Uranus()
    bodies["neptune"] = ep.Neptune()
    bodies["pluto"] = ep.Pluto()

    # Local information and celebrity declarations are global variables
    local = ep.Observer()
    r=0
    alt=0

    def __init__(self):
        
        # The path is absolute, see the comments of the C file for details
        currentPyFilePath = os.path.abspath(__file__) # The path of the current .py file
        parentPyFilePath = os.path.dirname(currentPyFilePath) # The current .py file is located (the entire plug -in all files are here)
        file004 = os.path.join(parentPyFilePath,'Database','004_Parameters obtained from UI panel.csv')
        findex=open(file004,"r") #Open the CSV of the storage user input
        
        row=0 # Operation performed in the ROW line
        for line in csv.reader(findex):
            if row >= 3: # The third line in CSV will not read in the future
                break
            elif row == 0: # cSV No. 0
                year    =int(line[0])
                month   =int(line[1])
                day     =int(line[2])
                hour    =float(line[3])
                minute  =float(line[4])
                second  =float(line[5])
            elif row == 1: # CSV Line 1
                lat = float(line[0])
                lon = float(line[1])
                alt = int(line[2])
            elif row == 2: # CSV Line 2
                self.r = int(line[0]) # 天体半径

            row+=1
        
        time = str(year) + '/' + str(month) + '/' + str(day) + ' ' + str(hour) + ':' + str(minute) + ':' + str(second)
        findex.close
       # From the user input to update local information
        self.local.lon = str(lon)
        self.local.lat = str(lat)
        self.local.elevation = alt
        self.local.date = time
        print('Updated local information.')


    # Added selected celestial body PL from the file
    def append_bodies(self,pl:str):
        '''
        pl from ["sun","moon","mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"]
        '''
        # Select SPAS as the event collection before the addition
        bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children['SPAS']
        # file_path = APPEND_BLEND
        # inner_path = "Object"
        # bpy.ops.wm.append(filepath=os.path.join(file_path,inner_path,pl),directory=os.path.join(file_path,inner_path), filename=pl)
        
        # 参考：https://blender.stackexchange.com/questions/38060/
        # blendfile = "3DModelAppend.blend" # The path is absolute, see the comments of the C file for details

        currentPyFilePath = os.path.abspath(__file__) # The path of the current .py file
        parentPyFilePath = os.path.dirname(currentPyFilePath) # The current .py file is located (the entire plug -in all files are here)
        blendfile = os.path.join(parentPyFilePath,'3DModelAppend.blend')
        
        section   = "\\Object\\"
        object    = pl

        filepath  = blendfile + section + object
        directory = blendfile + section
        filename  = object

        bpy.ops.wm.append(
            filepath=filepath, 
            filename=filename,
            directory=directory)
        
        print(f'Appended {pl}')

    # Added celestial body to SPAS collection (this step is not necessary. Selecting SPAS as the event collection before adding it, the additional things will naturally go in)
    # def link_collection(self,pl:str):
    #     '''
    #     pl from ["sun","moon","mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"]
    #     '''
    #     self.link_collection(pl)
    #     bpy.context.view_layer.active_layer_collection = bpy.context.view_layer.layer_collection.children['SPAS']
    #     bpy.data.collections["SPAS"].objects.link(bpy.context.active_object) 

    # Move the additional celestial PL to the specified location, and zoom in the diameter according to the diameter
    def adjust_bodies(self,pl:str):
        '''
        pl from ["sun","moon","mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"]
        '''
        # Based on a certain place, calculate the relevant information of celestial bodies
        print(f'{self.local.lat},{self.local.lon},UTC+0: {self.local.date}')
        self.bodies[pl].compute(self.local)
        print('%s %f %f' %(pl,self.bodies[pl].az,self.bodies[pl].alt))
        print(f'Calculated {pl}\'s information.')

        # Considering the obstruction relationship of celestial bodies, different types of celestial bodies are generated on the sphere of different radius R.

        if pl == "moon":    # The moon blocks everything, placed on the smallest radius
            R = 0.94 * self.r
        elif pl == "sun":   # Except for the phenomenon of the planet in the inner earth of the earth, it will appear in front of the sun. In the rest of the case, if it appears in the same position of the sky, it will be blocked by the sun; the outer planet may only be blocked.
            R = 0.96 * self.r    # So take the radius of the bearing of the sun less than the planet
        else:
            R = 0.98 * self.r

        # Modify the location of the new celestial body, refer to：https://blender.stackexchange.com/questions/120026/
        # Acting angle to take the opposite number！！！！！！！！！！！
        ##### Because the horizontal coordinate direction angle AZ Zhengbei (+X) rotates to Zhengdong (-y) to the positive, and the azimal angle of the ball coordinate system specifies the rotation from+x axis+y-axis to positive The time corner should be reversed, otherwise the celestial bodies generated are about Xoz flat -faced symmetry
        x = R * math.sin(math.pi/2-self.bodies[pl].alt+0.0) * math.cos(-self.bodies[pl].az+0.0)
        y = R * math.sin(math.pi/2-self.bodies[pl].alt+0.0) * math.sin(-self.bodies[pl].az+0.0)
        z = R * math.cos(math.pi/2-self.bodies[pl].alt+0.0)                                   
        bpy.data.objects[pl].location = (x,y,z)

        # Actify the celestial body according to the diameter of the diameter 
         # SELF.BODIES [PL] .size returns the visual diameter of the angle seconds (ArcSEC). 
         # Because the diameter (unit ARCSEC) is small, the arc length can be approximately equal to the visual diameter (unit Meter), in 3DModelScale.blend, all celestial diameter have been initialized to 1m, so how many times the diameter of the visual diameter can be zoomed in how many times the diameter is zoomed in. 
         # Converted curved, a total of 360Degrees * 3600 Arcsec/Degree, a total of 2π arc
        rad = self.bodies[pl].size / (360 * 3600) * (2 * math.pi)
        scale = rad * R # Calculate the diameter (unit meter)
        
        # But although this is true, it looks too small, let the diameter expand the diameter, right?
        if pl == 'sun' or pl == 'moon':
            scale *= 2
        else:
            scale *= 5
        bpy.data.objects[pl].scale = (scale,scale,scale) #Magnify the object according to the diameter

        print(f'Adjusted {pl}\'s location and size.')


        # By adding a light and radius light to a certain location to simulate the effect of the solar system, except the other celestial bodies of the sun are illuminated by the sun

        if pl == 'sun':
            pass #The sun does not need to add additional lights to simulate and illuminate
        else:

            sunIndependent = ep.Sun()            # Independence on the self.bodies ['sun'] instance created when generating the sun, creating an instance of a solar separately here
            sunIndependent.compute(self.local)   # In this way, whether the user chooses to generate the sun, when other calculations need to obtain the solar data, it will not report an error because the sun instance is not defined

            #It is not the situation of the sun, first calculate the right -angle coordinates of the sun and the celestial body PL. Among them, the XYZ of the celestial body PL has been given above. The following is the sun. 
             # The opposite number! Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection See the comment before, and I won't repeat it.
            x_sun = R * math.sin(math.pi/2-sunIndependent.alt+0.0) * math.cos(-sunIndependent.az+0.0)
            y_sun = R * math.sin(math.pi/2-sunIndependent.alt+0.0) * math.sin(-sunIndependent.az+0.0)
            z_sun = R * math.cos(math.pi/2-sunIndependent.alt+0.0)

            vector = [x_sun-x, y_sun-y, z_sun-z] # This is the vector as of the celestial body PL to the sun 
             # In the Blener test, it was found that in the connection between the sun and the celestial body, the position of 5kW was placed at the position of the celestial body 2A. Half half the effect
            vector_length = math.sqrt(vector[0]*vector[0] + vector[1]*vector[1] + vector[2]*vector[2]) # Vector Model Long | As |
            vector[0] /= vector_length # The original vector is divided by the mold length, and the unit vector AS/| AS | = ASE, the direction is still the celestial body PL points to the sun
            vector[1] /= vector_length
            vector[2] /= vector_length

            vector[0] *= 2*scale # Change the length of the vector to 2A, here A = scale, the point light source is 2A from the celestial body PL, ASE*2A = AP.
            vector[1] *= 2*scale
            vector[2] *= 2*scale

            x_light = vector[0] + x # Celestial → point light source vector A, plus the original point → celestial veil diameter OA, get the origin → point light source votal diameter OP
            y_light = vector[1] + y
            z_light = vector[2] + z
            
            # Create some light, move to the position of the veil diameter OP, 
             #Scale is a diameter, so except for 2 get the radius, multiplied by 4 is because in Blender, the Radius data must be filled in the result of 4, such as Radius = 4, and the actual radius is 1m 1m
            if pl == 'moon':
                bpy.ops.object.light_add(type='POINT', radius=4*1.2*scale/2, location=(x_light, y_light, z_light))
                bpy.context.object.data.energy = 100000 # The lunar brightness adjustment is 6000W, and the adjustment radius is 1.2 times that of the celestial PL radius, which can just illuminate half
            elif pl == 'mercury' or pl == 'venus':
                bpy.ops.object.light_add(type='POINT', radius=4*1.2*scale/2, location=(x_light, y_light, z_light))
                bpy.context.object.data.energy = 500000   # Internal planet's brightness adjustment is 3000W, and the adjustment radius is 1.2 times the PL radius of the celestial body, which can just illuminate half
            else:
                bpy.ops.object.light_add(type='POINT', radius=4*4*scale/2, location=(x_light, y_light, z_light))
                bpy.context.object.data.energy = 1000   # External planet's brightness adjustment 3000W, adjust the radius to 4x celestial PL radius to illuminate most of the star body
            bpy.context.object.name= pl + '\'s sunlight' #Who is it?

        
    
    ##########
  # Core code
    ##########
    """
   The following discussions of right ascension and declination are based on the Earth's equatorial coordinate system J2000.0 epoch
    In order to restore the true rotation axis inclination of the celestial body pl (this is clearly reflected in the annual seasonal changes of Saturn), 
    it is necessary to align the polar axis (rotation axis, local +z axis of the celestial body model) of each celestial body with its own north celestial pole.
    That is, the right ascension and declination recorded in polar_axis above. Since each major celestial body pl is generated with coordinates determined 
    according to the local horizon coordinate system (azimuth angle, elevation angle) at that time, its polar axis should also be aligned with its own north 
    pole direction at that time, or in other words, "aligned with the local coordinate system at that time" The celestial body's North Star"
    This requires obtaining the local position of the "Polaris" at that time, but ephem does not preset the relevant data of all stars, so here we 
    manually add these "North Celestial Pole" as a virtual star -"Polaris". This step is implemented through the FixedBody function, which provides 
    Just right ascension and declination. Then use ephem to calculate the local azimuth and elevation angle of this "virtual Polaris" at that time, 
    and further convert it into rectangular coordinates, such as ArtificialPolar -> P(x0,y0,z0)
    Then, the vector OP=(x0,y0,z0) pointing from the center of the earth (0,0,0) to the "virtual Polaris" can approximately replace the vector from
    the center of mass of the pl celestial body pointing to the "virtual Polaris" (due to the earth and The distance to pl of other celestial bodies
    in the solar system is much smaller than the distance to the stars. Furthermore, the +z-axis direction vector of the pl celestial body should be
    made parallel to this OP vector. This step is achieved through the "Align Euler to Vector" geometry node
    """
    
    # Align the polar axis of 极 对 l
    def calibrate_axis(self,pl:str):
        '''
        Assuming the target is in the SPAS collection
        '''
        # Read the northern Tianji red meridians, and generate virtual Arctic stars
        polar_point = ep.FixedBody()
        polar_point._ra = self.polar_axis[pl][0]
        polar_point._dec = self.polar_axis[pl][1]
        polar_point.compute(self.local)

        # For the moon, two axes need to be aligned.
        if pl == "moon":
            
            # Align the polar shaft, here the coordinate of the ball is r = 1, that is, the unit vector of the polar axis direction vector
            # Horizon coordinate system turning the ground space right -angle coordinate system, calculate the "virtual Arctic star" coordinates
            # Take the opposite number of angle! Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection See the comment before, and I won't repeat it.
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[0] = math.sin(polar_point.alt+0.0) * math.cos(-polar_point.az+0.0) # x
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[1] = math.sin(polar_point.alt+0.0) * math.sin(-polar_point.az+0.0) # y
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[2] = math.cos(polar_point.alt+0.0)                                # z
            
            # Align the+X axis to make the moon facing the earth facing the earth, the tide is locked
            # From the ball coordinates of the moon in the horizon coordinate system, converted into the spatial right -angle coordinates of the moon
            # Here is R = -1, that is, the reverse unit vector of the axis direction vector, because the position vector of the moon is pointing to the moon, but the+X axis is directed to the earth.
            # Take the opposite number of angle! Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection See the comment before, and I won't repeat it.
            bpy.data.node_groups[pl].nodes["Align Euler to Vector.001"].inputs[2].default_value[0] = -1 * math.sin(math.pi/2-self.bodies[pl].alt+0.0) * math.cos(-self.bodies[pl].az+0.0) # x
            bpy.data.node_groups[pl].nodes["Align Euler to Vector.001"].inputs[2].default_value[1] = -1 * math.sin(math.pi/2-self.bodies[pl].alt+0.0) * math.sin(-self.bodies[pl].az+0.0) # y
            bpy.data.node_groups[pl].nodes["Align Euler to Vector.001"].inputs[2].default_value[2] = -1 * math.cos(math.pi/2-self.bodies[pl].alt+0.0)                                   # z
        
        else:
            # Align with the moon with the polar axis of the moon
            # Take the opposite number of angle! Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection Intersection See the comment before, and I won't repeat it.
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[0] = math.sin(polar_point.alt+0.0) * math.cos(-polar_point.az+0.0) # x
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[1] = math.sin(polar_point.alt+0.0) * math.sin(-polar_point.az+0.0) # y
            bpy.data.node_groups[pl].nodes["Align Euler to Vector"].inputs[2].default_value[2] = math.cos(polar_point.alt+0.0)                                # z

        print(f'Calibrated {pl}\'s polar axis.')

# These are really useless. Even if the user does not enter, the CSV below will read the default value I set from the Property of Blender
# local = ep.Observer()
# local.lat = '39.904214'
# local.lon = '116.407413'
# local.date = "2023/7/30 20:43:40"

    # Here are all functions that generate a full process required for a celestial PL
    def generate_bodies(self,pl):
        self.__init__(classE)
        self.append_bodies(classE,pl) # Additional celestial body PL required from 3DMODELSCALE.BLEND
        self.adjust_bodies(classE,pl) # Adjust the position, size of the places of celestial body PL
        self.calibrate_axis(classE,pl) # Align the polar axis of the celestial body PL, tidal lock (only the moon)
        print(f'{pl} added successfully.\n')


        
    """
    天体模型来源:
    https://solarsystem.nasa.gov/resources/2352/sun-3d-model/
    https://solarsystem.nasa.gov/resources/2369/mercury-3d-model/
    https://solarsystem.nasa.gov/resources/2343/venus-3d-model/
    https://solarsystem.nasa.gov/resources/2372/mars-3d-model/
    https://solarsystem.nasa.gov/resources/2375/jupiter-3d-model/
    https://solarsystem.nasa.gov/resources/2355/saturn-3d-model/
    https://solarsystem.nasa.gov/resources/2344/uranus-3d-model/
    https://solarsystem.nasa.gov/resources/2364/neptune-3d-model/
    https://solarsystem.nasa.gov/resources/2357/pluto-3d-model/
    https://open3dmodel.com/3d-models/moon-from-nasa_595918.html
    """

    # 追加 3DModelAppend.blend 中的 世界材质-天空纹理
    def append_skyTexture(self):
        #reference：https://blender.stackexchange.com/questions/38060/
        # blendfile = "3DModelAppend.blend" # The path is absolute, see the comments of the C file for details
        currentPyFilePath = os.path.abspath(__file__) # The path of the current .py file
        parentPyFilePath = os.path.dirname(currentPyFilePath) # The current .py file is located (the entire plug -in all files are here)
        blendfile = os.path.join(parentPyFilePath,'3DModelAppend.blend')

        section   = "\\World\\"
        object    = 'World Sky Texture'
        filepath  = blendfile + section + object
        directory = blendfile + section
        filename  = object
        bpy.ops.wm.append(
            filepath=filepath, 
            filename=filename,
            directory=directory)
        
        # The world material that has just been added as the current world environment, refer to：https://blenderartists.org/t/how-to-set-a-worlds-shader-to-the-world-with-python-please/636247/3
        bpy.context.scene.world = bpy.data.worlds['World Sky Texture']

        sunIndependent = ep.Sun()            # Independence on the self.bodies ['sun'] instance created when generating the sun, creating an instance of a solar separately here
        sunIndependent.compute(self.local)   # In this way, whether the user chooses to generate the sun, when other calculations need to obtain the solar data, it will not report an error because the sun instance is not defined

        bpy.data.worlds["World Sky Texture"].node_tree.nodes["Sky Texture"].sun_elevation = sunIndependent.alt          # Set the height angle of the sun, the unit arc
        bpy.data.worlds["World Sky Texture"].node_tree.nodes["Sky Texture"].sun_rotation = sunIndependent.az+math.pi/2  # Set the corner of the sun, the unit arc 
         # Because Blender's medium sky texture, the+Y axis is 0 degrees, and the horizon coordinate system is 0 degrees at the north of the+X axis, so it is to add 90 degrees, that is, π/2 arc, which does not involve horizontal coordinates. Angle corner does not require negative
        bpy.data.worlds["World Sky Texture"].node_tree.nodes["Sky Texture"].altitude = self.alt                             # Set the altitude height to the user input

        print(f'Appended World Sky Texture')