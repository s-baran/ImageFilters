   M S B A M L   `   `   `  ����G  CImageFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ImageFiltersImageFilters  5   	       ImageFilters.KernelWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:ImageFilters     5   	     � 
ResizeMode$  NoResize=�5   	    � Title$ Kernel��5	   	   $	��500��6   $	��500��6%   .��5
      �  � RowDefinitions 5   
   �� 5      $	��80* �6   �� 5      $	��20* �6   .+�5   
   �� $��20q�6   
����  �$��23q�66   $
��Auto=�6K   .��5      �� -   5         ��Name$ kernelAsText��5   =    �� AcceptsReturn .  �� 
AcceptsTab . $
��Left=�6S   $	��Top=�6n   $��0q�5      5      �� 5   
   $��1��6   $��Center=�6"   $��Center=�6?    �� Orientation$ Vertical=�6Z   .+�5      �� $
��Left=�6   $��Center=�64   $ 
Horizontal=�6O   .+�5      �� .��6   Size:6   �� 5      -   5        $ SizeTextBox��5      $��5q�6/   $��50��6:   6G   5      �� 5      -   5         ��Name$ ValidationLabel��5      �� 5      $ 
Horizontal=�6   .+�5      �� -   5         ��Name$ 	NewButton��5      $��5q�6,   $	��100��67   $	��New��6C   6Q   �� 5      -   5        $ 
LoadButton��5      $��5q�6-   $	��100��68   $
��Load��6D   6S   �� 5      -   5        $ 
SaveButton��5      $��5q�6-   $	��100��68   $
��Save��6D   6S   �� 5      -   5        $ ApplyButton��5      $��5q�6.   $	��100��69   $��Apply��6E   6U   5      5      5       5!      