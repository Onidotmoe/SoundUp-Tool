   M S B A M L   `   `   `  ����J  FXamlAnimatedGif, Version=1.1.2.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:XamlAnimatedGif;assembly=XamlAnimatedGifXamlAnimatedGif  5   	    SoundUp Tool   SoundUp_Tool.GifPlayer   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Fgif6clr-namespace:XamlAnimatedGif;assembly=XamlAnimatedGif     5   	   -   5          �Name$  GifPlayerWindow��5   	    � Title$ 
Processing��5   	   $	��300��5   	   $	��200��5   	   $	��300��5	   	   $	��200��5
   	   $
��1200��5   	   $	��800��5   	    � AllowsTransparency .  � Icon$ Resources/tibsun.ico��5   	    � 
ResizeMode$ 	CanResize=�5   	    � WindowStartupLocation$ CenterOwner=�5   	    � WindowStyle$
 None=�5   	   ��5      �� 5   
    �� 
TargetType" �6   .�5      �� '  !System.Windows.Shell.WindowChrome �� Property �@ WindowChrome	 �� Value	 5        5      
   CornerRadius$
 0��6#      GlassFrameThickness$ 0q�5   #      ResizeBorderThickness$ 10q�5   #      UseAeroCaptionButtons .  5   #   5      5       ��5#      �� 5$   
    �� ImageSource$ Resources/Engineer.gif��6    j� Stretch$ Uniform=�6:   .��5'      (� -   5         (�Name$ 
GifPicture��5'      .��5(   
   � $	��300��6   $	��200��6   .+�5)      �� '   !XamlAnimatedGif.AnimationBehavior   	SourceUri$ Resources/Engineer.gif?�6   �� 5+      -   5         ��Name$ 
TimerLabel��5+      $��200,160,15,10q�5,      $��00:00:00��5-      $��Verdana)�5.      $��15(�5/      $
��Bold"�50      
������� J� IsHitTestVisible .  52      �� 54      $	��300��6   $	��200��55      
���   �$	��0.5q�57      58      59      5:      