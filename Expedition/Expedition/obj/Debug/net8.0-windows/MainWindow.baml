   M S B A M L   `   `   `  ����E  AExpedition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:Expedition
Expedition  5   	       Expedition.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   *localclr-namespace:Expedition     5   	   -   5          � Title$  
MainWindow��5   	   $	��600��5	   	   $	��800��5
   	   ���#00272B � WindowStyle$
 None=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � AllowsTransparency$
 True��5   	    � 
ResizeMode$ NoResize=�5   	   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� 
������  �� CornerRadius$ 15��6*   ._�5      � $��0��6    � ColumnDefinitions 5      �� 5      $��20 �6'   �� 5      �� 5      $
��Auto �6'   .+�5      �� $��1��6   $��Экспедеция��6,   $
��Left=�6>   $��Center=�6Y   $��14(�6t      MainText#��[  6�   6�   �� 5      $��2��6    �� Orientation$ 
Horizontal=�6-   $��0 5 5 0q�6F   .+�5       �� -   5        $��18(�5    !   #��[  6/   	 � Cursor$
	 Hand��5!      
 �� TextAlignment$
 Center=�5"      $��Center=�63   $��Center=�6N   $��5,5,5,5q�5#      $��30��6-   $��30��68   _6G   �� 5$      -   5        $��18(�5$       #��[  6.   $
	 Hand��5%      $
 Center=�5&      $��Center=�63   $��Center=�6N   $��5,5,5,5q�5'      $��30��6-   $��30��68   ✕6G   5(      5)      5*      � 5+   
   $��1��6    5,      �� 5-      $
��Auto �6    60   �� 5.      6"   �� 5/      $
��Auto �6    60   .+�51      � $��0��6   $��Stretch=�6    $��Center=�6>   $��10, 5q�6Y   .+�52      �� -   5        $��txtPlayesStats��52      #��[  62   $�� ��6T   $��14(�6\   $
��Left=�6j   $��Center=�6�   6�   �� 53      -   5        $��btnSave��53        
MainButton#��[ 6(   $	��120��6L   $��10, 5q�6X   $��Сохранить��6g   $��Right=�6{   $��Center=�6�   $y�Hidden=�6�   54      �� 55      -   5        $��OutputTextBox��55      $��1��6+    �� TextWrapping$
 Wrap=�68    �� 
IsReadOnly$
 True��6L   " �� VerticalScrollBarVisibility$
 Auto=�6^   $��10q�56       �� AcceptsReturn$
 True��6     CustomTextBoxStyle#��[ 63   �� 57      $��2��6   $ 
Horizontal=�6&   $��10q�6?   $��Center=�6K   $��Center=�6h   .+�58      �� -   5        $��InputTextBox��58      #��[ 6.   $��40��6Z   $	��350��6f   $��10q�6r   $ Disabled=�6~   6�   �� 59      -   5        #��[ 59      $	��150��6=   $��10q�6I   $��Ввести��6U   6f   5:      5;      5<      5=      