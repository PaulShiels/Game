   M S B A M L   `   `   `  ����B  >WPF.MDI, Version=1.1.0.0, Culture=neutral, PublicKeyToken=null2&clr-namespace:WPF.MDI;assembly=WPF.MDIWPF.MDI  5      B >Example, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:ExampleExample 5   	      Example.Main   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      6mdi&clr-namespace:WPF.MDI;assembly=WPF.MDI     5      $myclr-namespace:Example     5   	   -   5        $��ContainerWindow��5   	     � Title$  WPF.MDI Example��5       � WindowState$ 	Maximized=�6    � WindowStyle$
 None=�65   .��5      ]� .+�5	   
      WPF.MDI.MdiContainer  -   5        $��	Container��5	         Menu 5
      o� 5      -   5        $
��menu��5      $��20��6#   $	��Top=�6/   .@�5      m� $	��Add��6   .@�5      m� -   5        $��Normal Window��5   #   6:   m� 5      -   5        $��Fixed Window��5   #   69   m� 5      -   5        $��Scroll Window��5   #   6:   m� 5      -   5        $
��Exit��5   #   61   5      m� 5      $��Theme��6   .@�5      m� -   5        $��Generic��5   4   $��Generic��6#    � ToolTip$* $Generic Visual Studio designer theme��6C   6r   m� 5      -	   5        $
��Luna��5   1   $
��Luna��6#   $ Blue Windows XP theme��6=   6]   m� 5      -
   5        $
��Aero��5   1   $
��Aero��6#   $ Windows Vista/7 theme��6=   6^   5      m� 5      -   5         m�Name$ WindowsMenu��5   0   $��Windows��6   6E   5         Children. 5         WPF.MDI.MdiChild  -   5        $��PlayingWindow��5      $y�Hidden=�60      Title$ Playing Area��6D      Content. 5      � .+�5      �� -   5        $��	imgCanvas��5      .+�5      �� -   5        $��cnvsPresents��5   !   5      �� 5       -   5        $��spBtns��5    %   	 �� Orientation$	 
Horizontal=�63   .+�5!      �� -   5        $��btnQuit��5!   %   $��30(�64   $��Kristen ITC)�6B   $
��Quit��6[   $��10q�6j   $��10q�6w   6�   �� 5#      -   5        $��btnSaveGame��5#   %   $��30(�68   $��Kristen ITC)�6F   $
��Save��6_   $��10q�6n   $��10q�6{   6�   5$      �� 5%      -   5        $��imgSanta��5%       63   5&      5'      5(        5*      -   5        $��StartupWindow��5*      
   	Resizable
 .     WindowState$ Normal=�6`   . 5+      ]� .+�5,      �� -   5        $��spStartupWindow��5,   6   $	��Top=�6!   .+�5-      �� -   5        $��
btnNewGame��5-   !   $
��10,5q�63   $��Kristen ITC)�6A   $��35(�6Z   .��6�   
New Game6�   �� 5.      -   5        $��btnLoadGame��5.   !   $
��10,5q�64   $��Kristen ITC)�6B   $��35(�6[   .��6�   	Load Game6�   �� 5/      -   5        $��
lbxPlayers��5/   "   $��10q�64    �� DisplayMemberPath$
 Name��6@   $��18(�6Y   $��Kristen ITC)�6g   $y�Hidden=�6�   6�   �� 50      -   5        $��btnExit��50   !   $��10q�60   $��Kristen ITC)�6<   $��35(�6U   .��6y   Exit6   51      �� 52      -   5        $��btnBack��52      $��Bottom=�6,   $
��10,5q�6D   $��Kristen ITC)�6R   $��45��6k   $��28(�6z   $y�Hidden=�6�   .��6�   Back6�   53      54        56      -   5        $��LoadingWindow��56      $y�Hidden=�6N   $	��350��6b   $	��200��6n   $ Normal=�6|   . 57      �� -   5        $��spLoadingWindow��57   6   $	 
Horizontal=�6   .+�58      �� $��
Loading...��6   $��28(�61   $��10q�6?   $��Center=�6K   $��Kristen ITC)�6f   6�   �� 59      -   5        $��
imgLoading��59      $��Center=�6.   $��Center=�6K    J� RenderTransformOrigin$ 0.5, 0.5&�6f   6�   5:      5;        5=      -   5        $��ScoringWindow��5=   /   $y�Hidden=�6   $ Scoring Area��6E      Icon$ OriginalLogo.png��6Z   . 5?      �  � ColumnDefinitions 5@      �� 5A      $��4* �6+   �� 5B      $��4* �6+    � RowDefinitions 5D      �� 5E      $��6* �6(   �� 5F      $��1* �6(   �� 5G      $��1* �6(   �� 5H      $��6* �6(   .+�5K      �� -   5        $��spList��5K   !   $��10q�6/    J� Opacity$	 0.7Q�6;   $��3��6I   ���	#FFF6F9E1.+�5L      �� $��Santa's List��6    $��Center=�67   ���	#FF972200$��Snap ITC)�6r   $��28(�6�   �� 5M      -   5        $��lstPresents��5M   '   $
��5, 0q�6:   ���	#FFF30000$
��Bold"�6_   $��0q�6r   $��Tempus Sans ITC)�6�   $��25(�6�   5N      �� 5P      $��1��6!   $	 0.8Q�61   .+�5Q      �� $��Score��6    $��Center=�60   $��55(�6M   $��Kristen ITC)�6[   
���  ��6�   �� 5R      -    5        $��lblScore��5R       $��Center=�60   $��79(�6M   $
��Bold"�6[   $��Kristen ITC)�6m   
���  ��6�   5S      �� 5T      $	 0.8Q�6!   ���	#FFFFEFEF$��1��6F   $��1��6V   $	 
Horizontal=�6c   .+�5U      �� $��Correct:   ��6    $��Kristen ITC)�66   $��30(�6O   ���	#FFF300006w   �� 5V      -!   5        $��
lblCorrect��5V       $��Kristen ITC)�62   $��30(�6K   ���	#FFF300006r   5W      �� 5X      $	 0.8Q�6!   ���	#FFFFEFEF$��1��6F   $��2��6V   $	 
Horizontal=�6c   .+�5Y      �� $��
Incorrect:��6    $��Kristen ITC)�65   $��30(�6N   ���	#FFF300006u   �� 5Z      -"   5        $��lblIncorrect��5Z       $��Kristen ITC)�64   $��30(�6M   ���	#FFF300006t   5[      �� 5\      -#   5        $��	imgSleigh��5\      $��0��6-   $��3��6=   $��4��6Q   $��Bottom=�6^   $
��10,0q�6y    J� 	AllowDrop . 6�   5]      5^      5_      5`      5a      