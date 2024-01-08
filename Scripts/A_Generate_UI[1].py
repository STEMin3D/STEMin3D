# Credit: BM-H and YuXiZa
import bpy
import csv
import os
from . import B_Preparing_the_Blender_Scene
from . import C_Convert_star_info
from . import D_Generate_the_stars
from . import E_Generate_Sun_Moon_Planet

# https://gitee.com/pampa666/blender_ui


# About Class, naming rules of bl_idname:
# https://devtalk.blender.org/t/how-do-i-use-bl-idname-correctly/19688/2
# https://blender.stackexchange.com/questions/5784/
# https://blender.stackexchange.com/questions/124736/




class SPAS_PT_PanelContent(bpy.types.Panel): 
    bl_idname = 'SPAS_PT_PanelContent' 
    bl_label = 'Generation Settings'
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = 'SPAS'

    def draw(self, context):

        row = self.layout.row()     # The content written by several ROWs will be squeezed into a line as a number of contents of this line
        col = self.layout.column()  # The content written by several column) will be squeezed into a column as a number of lines of this column 
                                    # No matter how much ROW writes, it will be squeezed in a line ... So if you have to start another line, you must declare the new Row, such as row1 = self.Layout.Row () 
                                    # The same way of finding that the order of Columb is turbulent, it will also declare a new COL+number, and then define the content with the new color
        row0 = self.layout.row()
        row1 = self.layout.row()
        row2 = self.layout.row()
        row3 = self.layout.row()
        row4 = self.layout.row()
        row5 = self.layout.row()
        row6 = self.layout.row()
        row7 = self.layout.row()
        row8 = self.layout.row()
        row9 = self.layout.row()
        row10 = self.layout.row()
        row11 = self.layout.row()
        row12 = self.layout.row()
        row13 = self.layout.row()
        row14 = self.layout.row()
        
        col1 = self.layout.column()
        col2 = self.layout.column()

        # col.label(text='Or everything already existed will be deleted.')
        
        col.label(text='Scientific and Precision Astronomical System')
        col.label(text=' - Real World Celestial Simulation')
        col.label(text='All the generated things are stored in collection "SPAS"')

        col.label(text='')
        
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
        # The following code is not needed due to a lack of need to generate stars
        '''
        row9.label(text='How many stars do you want (Sorted by visual magnitude)?') 
        row10.prop(context.scene.spas_properties, 'SPAS_amount', text="amount (max 258997)")
        row11.label(text='500~2000 is recommended, which may take less than 1 minute.')
        row12.label(text='(Or you can try more if you\'re confident in your device)')
        '''
        row13.label(text='Select what additional celestial objects you need.')
        row14.prop(context.scene.spas_properties, 'SPAS_sun', text="Sun")
        row14.prop(context.scene.spas_properties, 'SPAS_moon', text="Moon")
        row14.prop(context.scene.spas_properties, 'SPAS_planet', text="Planets & Pluto")
        col1.label(text='Set the radius of the celestial (100~10000m).')
        col1.prop(context.scene.spas_properties, 'SPAS_radius', text="Radius (100~10000m)")
        col1.label(text='')
        col1.label(text='First click this button to prepare the world.')
        col1.operator("spas.button1", text='Initialize World', icon='WORLD')
        col1.label(text='Then click this to Generate adapted celestial (wait 1~2 min. if amount>3000)')
        col1.operator("spas.button2", text='Generate Celestial', icon='MATSHADERBALL')

        

class SPAS_OT_PanelButton1(bpy.types.Operator): #Define button 1 action
    '''Initialize start interface, store indexes, create children object, material, and collection'''
    bl_idname = 'spas.button1'
    bl_label = ' '

    def execute(self, context):
        #Organize the parameters and options obtained from the UI and export them to an external .csv file
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

        
        # 根据用户选择的时区，处理用户输入的当地时间，即直接用当地时间的小时数减去时区数，得到格林威治时间
        for TZ in range(0,30): # 从西12区到东14区，逐一匹配名字
            if indexes.SPAS_tz == str(TZ): # 匹配选项名字
                print(f'UTC: {TZ-13}')
                if hour-int(TZ-13)>=0: # 小时减完时区不会导致日期向前变更（像26:49:00这种要向后变更日期的不会出错）
                    hour -= int(TZ-13) # 当地之间转换为格林威治时间

                else: # 会导致日期向前一天变更，那就不能用负小时数来写
                      # 如 UTC+3 2023.7.28 1:49，会换算成 UTC+0 2023.7.28 -2:49，进而变成 UTC+0 2023.7.27 21:11，这是错的
                      # 这是在 UTC+0 2023.7.27 24:00 基础上减去了2:49，而不是预期的在 UTC+3 2023.7.28 1:49 上直接扣除三小时变成 UTC+0 2023.7.27 22:49
                      
                      # 此外，ephem算法中，向前跨月，向前跨年的结果也都是错的，如2023.05.00 会变成 2023.05.01 而非正确的 2023.04.30
                      # 又如 2023.01.00 会变成 2023.01.01 而非正确的 2022.12.31；但到了2023.01.-1 ，则会变成正确的 2022.12.30
                      # 由于每月天数不一，2月天数又受到闰年影响，闰年也不稳定，在这里复现C语言程序设计的时间紊流题复杂且没有必要
                      # 有一种方法可以正确实现年月日的倒退，即带小数的负数小时，如 2023.05.01 -1.25:00:00 → 2023.04.30 22:45:00
                      # 这个 -1.25:00:00，其实就是 1.75:00:00 倒退三小时的结果，也就是 01:45:00 倒退三小时
                      # 所以预先把分钟和秒分别除以60和3600，做为小数部分放进小时即可，然后自身归零

                      hour += minute/60
                      hour += second/3600
                      hour -= int(TZ-13) # 当地之间转换为格林威治时间
                      minute = 0
                      second = 0
                    
                break

        # 路径绝对化，详见C文件注释
        currentPyFilePath = os.path.abspath(__file__) # 当前.py文件的路径
        parentPyFilePath = os.path.dirname(currentPyFilePath) # 当前.py文件所在的文件夹（整个插件所有文件都在这里）
        file004 = os.path.join(parentPyFilePath,'Database','004_Parameters obtained from UI panel.csv')
        fw=open(file004,'w',newline='') # 写入存储用户输入的文件

        csv.writer(fw).writerow ([year,month,day,hour,minute,second]) # 向数据库写入用户输入的时间
        csv.writer(fw).writerow ([lat,lon,alt])
        csv.writer(fw).writerow ([radius,amount])
        csv.writer(fw).writerow ([sun,moon,planet])
        fw.close
        B_Preparing_the_Blender_Scene.classB.mainB() # 运行B文件中的mainB函数，即关于准备世界环境的一切
        print('Button 1 operation finished.\n')
        return {'FINISHED'}
    
class SPAS_OT_PanelButton2(bpy.types.Operator): # 定义按钮2动作
    '''Generate the celestial according to user's settings'''
    bl_idname = 'spas.button2'
    bl_label = ' '

    def execute(self, context):
        C_Convert_star_info.classC.mainC() # 运行C文件中的mainC函数，即关于计算恒星参数映射的一切
        indexes = context.scene.spas_properties # 提取三个布尔类型的结果，即是否生成该物体
        sun     = indexes.SPAS_sun
        moon    = indexes.SPAS_moon
        planet  = indexes.SPAS_planet

        if sun == True: # 选择了太阳，则生成太阳   
            E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE,pl='sun')
        if moon == True: # 选择了月亮，则生成月亮
            E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE,pl='moon')
        if planet == True: # 选择了行星，要逐一生成
            planet_list=["mercury","venus","mars","jupiter","saturn","uranus","neptune","pluto"]
            for body in planet_list:
                E_Generate_Sun_Moon_Planet.classE.generate_bodies(E_Generate_Sun_Moon_Planet.classE,pl = body)
        
        E_Generate_Sun_Moon_Planet.classE.append_skyTexture(E_Generate_Sun_Moon_Planet.classE) # 追加天空纹理
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



# The following register and Unregister functions are written from the prefs.py of the official plugin add_camera_rigs
def register():
    from bpy.utils import register_class
    register_class(SPAS_PT_PanelContent)
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