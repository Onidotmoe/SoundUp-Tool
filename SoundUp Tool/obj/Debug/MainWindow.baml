   M S B A M L   `   `   `  ����  SoundUp Tool    SoundUp_Tool.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	     � Title$  SoundUP Tool��5   	   $��436.4��5   	   $��150.8��5   	    � Icon$ Resources/tibsun.ico��5	   	    � 
ResizeMode$ CanMinimize=�5
   	    � WindowStartupLocation$ CenterScreen=�5   	   .��5      � -   5        $��MainGrid��5       � 	Resources 5   
    �� 
TargetType �� Property �� Value��%_   '��       �� 5      " ��6   .�5      ��  �@^�ShowOnDisabled . 6B   5      5      .+�5   
   �� -   5         ��Name$ TxtB_INIPath��5      $	��304��5      $��20��5      $��	20,55,0,0q�5      $
��Left=�5      $	��Top=�5      	 �� 
IsReadOnly	 . $-��'Click to select your INI File Directory��5      
 �� TextAlignment$
 Center=�5       �� TextWrapping$ NoWrap=�5       � ToolTip$  The path to your INI files��5      �� 5    
   -   5        $ TxtB_XMLPath��5       $	��304��5!      $��20��5"      $��	20,30,0,0q�5#      $
��Left=�5$      $	��Top=�5%      	 . $#��Click to select your XML File��5(      $
 Center=�5)      $ NoWrap=�5*      $ The path to your XML file��5+      �� 5,   
   -   5         ��Name$ 	Btn_Start��5,      $��75��5-      $��20��5.      $��
329,85,0,0q�5/      $
��Left=�50      $	��Top=�51      ���	#FFDDDDDD$��Start��54      $! Start the insertion process��55      �� 56   
   -   5        $ 
Btn_Switch��56      $��75��57      $��20��58      $��
249,85,0,0q�59      $
��Left=�5:      $	��Top=�5;      ���	#FFDDDDDD$��
XMl to INI��5=      $ Switch -> INI to XML��5>      �� 5?   
   -   5         ��Name$ ChBx_New��5?      $��
329,32,0,0q�5@      $
��Left=�5A      $	��Top=�5B      $��
Create New��5D      $] WCheck this to create a new export file. Files that don't exist will be created anyways.��5E      5F      �� 5G   
   -   5        $ ChBx_Multiline��5G      $��
329,57,0,0q�5H      $
��Left=�5I      $	��Top=�5J      $��	Multiline��5L      {�.  $� �Check this to have key values appear on a new line for each value, in the exported XML file. Keep in mind that the file will be very long.��5N      5O      �� 5P   
   -   5         ��Name$ Label_Me��5P      $��20,5,0,0q�5Q      $
��Left=�5R      $	��Top=�5S      $��©VampireMonkey��5T       � Cursor$
 Hand��5U      $��10(�5V       J� Opacity$	 0.5Q�5X      ��5Y      �� 5Z       �� Color# � ��6"   5\      5]      5^      