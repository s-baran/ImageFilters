   M S B A M L   `   `   `  ����G  CImageFilters, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:ImageFiltersImageFilters  5   	       ImageFilters.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:ImageFilters     5   	     � Title$  ImageFilters��5   	   $	��600��6   $	��800��6+   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��70* �6   �� 5      $	��30* �6   .+�5   
   ]� $��0��6   .+�5      �� -   5         ��Name$ ImageViewer��5   )   $	��Top=�6   $
��Left=�6>   $
��Auto��6Y   $��	10,10,0,0q�6g   $	��Top=�6z   $
��Auto��6�   5      �� 5   
   
�������$��5q�6,   $��1��67   .+�5      �� -   5         ��Name$ FileName��5      $
��Auto��6&   $
��Auto��63   $��Ścieżka do pliku:��6A   �� 5      -   5        $��5q�5      $��Bottom=�6!   $��75��6=   .��6c   
Wyszukaj6m   �� 5      �� 5      $��Thresholding:��6   ]� 5      .+�5      �� $��	Threshold��6   $��Center=�6,   �� 5      $��Right=�6   $��5q�66   $��Center=�6A   ��6\   �� *Value+ �� ElementName$ ThresholdSlider�� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5      �� 5      -   5         ��Name$ ThresholdSlider��5       � Minimum$ 0Q�6.   	 � Maximum$		 100Q�6:   
 � Value$
 15Q�6H    �� IsSnapToTickEnabled .  �� TickFrequency$	 0.1Q�6n   ]� 5      .+�5      �� $
��Size��6   $��Center=�6'   �� 5      $��Right=�6   $��5q�66   $��Center=�6A   ��6\   �� *Value+$ 
SizeSlider��$ TwoWay=�$ PropertyChanged=�5      �� 5      -   5        $ 
SizeSlider��5      $ 0Q�6)   $		 100Q�65   $
 15Q�6C    . $ 1Q�6i   �� 5       -   5         ��Name$ BradleyButton��5       $��5q�6,   $��75��6S   .��6^   	Bradley6g   �� 5!      �� 5"      $��Blur:��6   ]� 5#      .+�5$      �� $
��Size��6   $��Center=�6'   �� 5%      $��Right=�6   $��5q�66   $��Center=�6A   ��6\   �� *Value+$ BlurSizeSlider��$ TwoWay=�$ PropertyChanged=�5&      �� 5'      -   5        $ BlurSizeSlider��5'      $ 0Q�6-   $		 100Q�69   $
 11Q�6G    . $ 1Q�6m   ]� 5(      .+�5)      �� $��Sigma��6   $��Center=�6(   �� 5*      $��Right=�6   $��5q�66   $��Center=�6A   ��6\   �� *Value+$ SigmaSlider��$ TwoWay=�$ PropertyChanged=�5+      �� 5,      -   5        $ SigmaSlider��5,      $	 0.5Q�6*   $	 5Q�68   $	
 1.4Q�6D    . $	 0.1Q�6k   ]� 5-      $��Center=�6   .+�5.      �� -	   5        $ 
BlurButton��5.      $��5q�6-   $��75��6Q   .��6\   Blur6b   �� 5/      -
   5        $ GaussBlurButton��5/      $��5q�62   $��85��6[   .��6f   Gaussian Blur6u   50      �� 51      �� 52      $��Custom Kernel��6   �� 53      -   5        $ IsLoadedLabel��53      $y�	Collapsed=�6+   ]� 54      $��Center=�6   .+�55      �� -   5        $ KernelCreatorButton��55   $   $��5q�6   $��75��6]   .��6h   Kernel6p   �� 56      -   5        $ ApplyKernelButton��56   $   $��5q�6   {�.  $��75��6k   .��6v   Apply6}   57      ]� 58      .+�59      �� -   5        $ ClearButton��59      $��5q�6.   $��65��69   .��6^   Clear6e   �� 5:      -   5        $ 
SaveButton��5:      $��Right=�6-   $��5q�6I   $��65��6U   .��6y   Save6   5;      5<      5=      5>      