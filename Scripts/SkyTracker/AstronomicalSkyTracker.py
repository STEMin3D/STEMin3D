bl_info = {
    "name" : "Astronomical System Tracker", 
    "author" : "Michael Cai",
    "description" : "Tracks the motion of the stars and solar system given user input.",
    "blender" : (3, 2, 0),
    "version" : (0, 1, 0),
    "location" : "",
    "category" : "Generic",
}

# -*- coding: UTF-8 -*-
import bpy
import csv
import os

class Test_PanelContent(bpy.types.Panel): # Class naming needs to follow the format ProjectName_TypeName(PT, MT, OT...)_purpose of the class (e.g., this class declares panel contents)
    bl_idname = 'PT_TestPanel' # bl_idname should correspond to the class name
    bl_label = 'Test Pane'
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = 'Test'

    def draw(self, context):
        # Content defined using several rows will be squeezed into one line, as multiple columns of that line
        # Content defined using several columns (column) will be squeezed into one column, as multiple rows of that column
        # Regardless of how many rows are written, they will be squeezed into one line... So if you want to start a new line, you need to declare a new row, like row1 = self.layout.row()
        # Similarly, if the order of columns becomes disordered, declare a new col+number, and then use the new col to define content
        # Definitions for multiple rows and columns

        row = self.layout.row()    
        col = self.layout.column() 
        row0 = self.layout.row()
        row1 = self.layout.row()
        row2 = self.layout.row()
        row3 = self.layout.row()
        row4 = self.layout.row()
        row5 = self.layout.row()
        row6 = self.layout.row()
        row7 = self.layout.row()
        row8 = self.layout.row()
       
        row13 = self.layout.row()
        row14 = self.layout.row()
        
        col1 = self.layout.column()
        col2 = self.layout.column()

        # col.label(text='This add-on can only perform in empty .blend file!!!')
        # col.label(text='Or everything already existed will be deleted.')
        col.label(text='Real World Celestial Simulation')
        
        col.label(text='Enter LOCAL TIME, location, and altitude.')
        col.label(text='Date: No leading "0" (2004.7.30 is ok, 2004.01.29 isn\'t)')
        row0.prop(context.scene.spas_properties, 'SPAS_tz', text="time zone") # TIME ZONE
        row1.prop(context.scene.spas_properties, 'SPAS_year', text="year")
        row1.prop(context.scene.spas_properties, 'SPAS_month', text="month")
        row1.prop(context.scene.spas_properties, 'SPAS_day', text="day")
        row2.label(text='Time: No leading "0" (0:0:0~23:59:59)')
        row3.prop(context.scene.spas_properties, 'SPAS_hour', text="hour")
        row3.prop(context.scene.spas_properties, 'SPAS_minute', text="minute")
        row3.prop(context.scene.spas_properties, 'SPAS_second', text="second")
        row4.label(text='Location: Latitude Longitude')
        row5.label(text='"+" for North & East, "-" for South & West')
        row6.prop(context.scene.spas_properties, 'SPAS_lat', text="latitude")
        row6.prop(context.scene.spas_properties, 'SPAS_lon', text="longitude")
        row7.label(text='Altitude: (meters)')
        row8.prop(context.scene.spas_properties, 'SPAS_alt', text="altitude(meters)")
        
        row13.label(text='Select what additional celestial objects you need.')
        row14.prop(context.scene.spas_properties, 'SPAS_sun', text="Sun")
        row14.prop(context.scene.spas_properties, 'SPAS_moon', text="Moon")
        row14.prop(context.scene.spas_properties, 'SPAS_planet', text="Planets")
        
        col1.label(text='First click this button to prepare the world.')
        col1.operator("spas.button1", text='Initialize World', icon='WORLD')
        col1.label(text='Then click this to Generate adapted celestial (wait 1~2 min. if amount>3000)')
        col1.operator("spas.button2", text='Generate Celestial', icon='MATSHADERBALL')

        

class SPAS_OT_PanelButton1(bpy.types.Operator): # Definition of Button 1 actions
    '''Initialize start interface, store indexes, create children object, material, and collection'''
    bl_idname = 'spas.button1'
    bl_label = ' '

    def execute(self, context):
        # Organize the parameters and options obtained from the UI and export them to an external .csv file
        indexes = context.scene.spas_properties
        year    = indexes.SPAS_year
        month   = indexes.SPAS_month
        day     = indexes.SPAS_day
        hour    = indexes.SPAS_hour
        minute  = indexes.SPAS_minute
        second  = indexes.SPAS_second
        lat     = indexes.SPAS_lat
        lon     = indexes.SPAS_lon
        alt     = indexes.SPAS_alt
        amount  = indexes.SPAS_amount
        sun     = indexes.SPAS_sun
        moon    = indexes.SPAS_moon
        planet  = indexes.SPAS_planet
        radius  = indexes.SPAS_radius

        
       # Process the local time input by the user according to the selected timezone, which involves subtracting the timezone number from the local time hours to get Greenwich Mean Time
        for TZ in range(0,30): # Iterating from Western UTC-12 to Eastern UTC+14 to match time zones
            if indexes.SPAS_tz == str(TZ): # Match the option name
                print(f'UTC: {TZ-13}')
                if hour-int(TZ-13) >= 0: # If subtracting the timezone doesn't cause the date to change backwards (like how 26:49:00 would change the date forward)
                    hour -= int(TZ-13) # Convert local time to Greenwich Mean Time

                else: # If it causes the date to move back one day, then negative hours cannot be used
                      # For example, UTC+3 2023.7.28 1:49, would convert to UTC+0 2023.7.28 -2:49, and then to UTC+0 2023.7.27 21:11, which is wrong
                      # This is based on reducing 2:49 from UTC+0 2023.7.27 24:00, instead of directly subtracting three hours from UTC+3 2023.7.28 1:49 to get UTC+0 2023.7.27 22:49
                      
                      # Additionally, in the ephem algorithm, results of moving backwards across months or years are also incorrect. For example, 2023.05.00 would become 2023.05.01 instead of the correct 2023.04.30
                      # Similarly, 2023.01.00 would become 2023.01.01 instead of the correct 2022.12.31; but 2023.01.-1 would correctly become 2022.12.30
                      # Since the number of days in each month varies, and February's days are affected by leap years, which are not consistent, it's complicated and unnecessary to replicate the time turbulence problem from C language program design here
                      # There is a method to correctly implement the rollback of year, month, and day, which is using fractional negative hours, like 2023.05.01 -1.25:00:00 → 2023.04.30 22:45:00
                      # This -1.25:00:00 is actually the result of 1.75:00:00 moving back three hours, which is 01:45:00 moving back three hours
                      # Therefore, divide minutes and seconds by 60 and 3600 respectively, add them as fractional parts to the hour, and then reset them to zero

                      hour += minute/60
                      hour += second/3600
                      hour -= int(TZ-13) # Convert local time to Greenwich Mean Time
                      minute = 0
                      second = 0

                    
                break

        # Make the file path absolute, see comments in file C for details
        currentPyFilePath = os.path.abspath(__file__) # Path of the current .py file
        parentPyFilePath = os.path.dirname(currentPyFilePath) # Folder where the current .py file is located (all files of the entire plugin are here)
        file004 = os.path.join(parentPyFilePath,'Database','004_Parameters obtained from UI panel.csv')
        fw = open(file004,'w', newline='') # Open the file to store user input

        # Write user input time data to the database
        csv.writer(fw).writerow([year, month, day, hour, minute, second]) 
        csv.writer(fw).writerow([lat, lon, alt])
        csv.writer(fw).writerow([radius, amount])
        csv.writer(fw).writerow([sun, moon, planet])
        fw.close
        B_Preparing_the_Blender_Scene.classB.mainB() # Run the mainB function in file B, which is about preparing the world environment
        print('Button 1 operation finished.\n')
        return {'FINISHED'}
    
class SPAS_OT_PanelButton2(bpy.types.Operator): # Define the action for Button 2
    '''Generate the celestial according to user's settings'''
    bl_idname = 'spas.button2'
    bl_label = ' '

    def execute(self, context):
        C_Convert_star_info.classC.mainC() # Run the mainC function in file C, which is about calculating star parameter mapping
        D_Generate_the_stars.classD.mainD() # Run the mainD function in file D, which is about generating stars in Blender and rotating the celestial sphere

        indexes = context.scene.spas_properties # Extract three Boolean results, i.e., whether to generate the object
        sun     = indexes.SPAS_sun
        moon    = indexes.SPAS_moon
        planet  = indexes.SPAS_planet

        if sun == True: # If the sun is selected, then generate the sun
            E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE, pl='sun')
        if moon == True: # If the moon is selected, then generate the moon
            E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE, pl='moon')
        if planet == True: # If planets are selected, generate them one by one
            planet_list=["mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"]
            for body in planet_list:
                E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE, pl = body)
        
        E_Generate_Sun_Moon_Planet.classE.append_skyTexture(E_Generate_Sun_Moon_Planet.classE) # Append sky texture
        print('Button 2 operation finished.')
        return {'FINISHED'}


class SPAS_PanelProperties(bpy.types.PropertyGroup):

    # Define time zone drop-down menu
    SPAS_tz: bpy.props.EnumProperty(
        name="SPAS_tz",
        description='your local time zone',
        items=[
            ('1', 'UTC-12', 'UTC-12'), # (option name, option content, option comment)
            ('2', 'UTC-11', 'UTC-11'),
            ('3', 'UTC-10', 'UTC-10'),
            ('4', 'UTC-09', 'UTC-09'),
            ('5', 'UTC-08', 'UTC-08'),
            ('6', 'UTC-07', 'UTC-07'),
            ('7', 'UTC-06', 'UTC-06'),
            ('8', 'UTC-05', 'UTC-05'),
            ('9', 'UTC-04', 'UTC-04'),
            ('10', 'UTC-03', 'UTC-03'),
            ('11', 'UTC-02', 'UTC-02'),
            ('12', 'UTC-01', 'UTC-01'),
            ('13', 'UTC+00', 'UTC+00'),
            ('14', 'UTC+01', 'UTC+01'),
            ('15', 'UTC+02', 'UTC+02'),
            ('16', 'UTC+03', 'UTC+03'),
            ('17', 'UTC+04', 'UTC+04'),
            ('18', 'UTC+05', 'UTC+05'),
            ('19', 'UTC+06', 'UTC+06'),
            ('20', 'UTC+07', 'UTC+07'),
            ('21', 'UTC+08', 'UTC+08'),
            ('22', 'UTC+09', 'UTC+09'),
            ('23', 'UTC+10', 'UTC+10'),
            ('24', 'UTC+11', 'UTC+11'),
            ('25', 'UTC+12', 'UTC+12'),
            ('26', 'UTC+13', 'UTC+13'),
            ('27', 'UTC+14', 'UTC+14'),]
        )


    SPAS_year: bpy.props.IntProperty(name="SPAS_year", min=1, max=9999, default=2023)
    SPAS_month: bpy.props.IntProperty(name="SPAS_month", min=1, max=12, default=7)
    SPAS_day: bpy.props.IntProperty(name="SPAS_day", min=1, max=31, default=28)
    SPAS_hour: bpy.props.IntProperty(name="SPAS_hour", min=0, max=23, default=19)
    SPAS_minute: bpy.props.IntProperty(name="SPAS_minute", min=0, max=59, default=49)
    SPAS_second: bpy.props.IntProperty(name="SPAS_second", min=0, max=59, default=00)

    SPAS_lat: bpy.props.FloatProperty(name="SPAS_lat", min=-90.0, max=90.0, default=39.9086, step=0.01, precision=4)
    SPAS_lon: bpy.props.FloatProperty(name="SPAS_lon", min=-180.0, max=180.0, default=116.3975, step=0.01, precision=4)
    SPAS_alt: bpy.props.IntProperty(name="SPAS_alt", min=0, max=10000, default=0)

    SPAS_amount: bpy.props.IntProperty(name="SPAS_amount", min=1, max=258997, default=500)

    SPAS_sun: bpy.props.BoolProperty(name="SPAS_sun", default=True)
    SPAS_moon: bpy.props.BoolProperty(name="SPAS_moon", default=True)
    SPAS_planet: bpy.props.BoolProperty(name="SPAS_planet", default=True)

    SPAS_radius: bpy.props.IntProperty(name="SPAS_radius", min=100, max=10000, default=1000)



# The following register and unregister functions are written from prefs.py of the official blender plug-in add_camera_rigs.
def register():
    from bpy.utils import register_class
    register_class(Test_PanelContent)
    register_class(SPAS_OT_PanelButton1)
    register_class(SPAS_OT_PanelButton2)
    register_class(SPAS_PanelProperties)
    bpy.types.Scene.spas_properties = bpy.props.PointerProperty(type=SPAS_PanelProperties)

def unregister():
    from bpy.utils import unregister_class
    unregister_class(SPAS_PT_PanelContent)
    unregister_class(SPAS_OT_PanelButton1)
    unregister_class(SPAS_OT_PanelButton2)
    unregister_class(SPAS_PanelProperties)
    
if __name__ == "__main__":
    register()
