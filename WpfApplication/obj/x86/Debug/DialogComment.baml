   M S B A M L   `   `   `  ����  WpfApplication"    WpfApplication.DialogComment   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  DialogComment��5   	    � SizeToContent$ WidthAndHeight=�6   $
��Left=�6>   $	��Top=�6Y   .��5      � $
��Left=�6   $	��Top=�6&    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��labelSelectedTool��5   ]   $��Tool: ��6   $��0��6!   $��Stretch=�6.   $��5,5,0,0q�6L   $	��Top=�6v   $��Bottom=�6�   �� 5   
   -   5        $��labelEmployee��5   _   $��Employee��6   $��3��6#   $��Stretch=�60   $��5,5,0,0q�6N   $	��Top=�6t   $��Bottom=�6�   �� 5   
   -   5        $��textBoxComment��5   j   $��2��6   $	��100��6    �� AcceptsReturn . $��Stretch=�6A   $��5q�6_   $	��Top=�6�   $	��160��6�    �� TextWrapping$
 Wrap=�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   �� 5   
   -   5        $��comboBoxEmployee��5   \   $��4��6   $��23��6!   $��Stretch=�6-   $��5,0,5,5q�6K   $	��Top=�6t   �� 5   
   -   5        $��stackPanelButtons��5   H   $��5��6   $
��Left=�6"   $��0q�6=   $	��Top=�6a    �� Orientation$ 
Horizontal=�6y   .+�5      �� -   5        $��buttonOk��5   .   $��Ok��6   $��23��6"   $��75��6>   $��5q�6I   6T   �� 5      -   5        $��buttonCancel��5   2   $��Cancel��6   $��23��6&   $��75��6F   $��5q�6Q    �� IsCancel . 6\   5      �� 5   
   -	   5        $��labelMakeComment��5   e   $��Make a comment��6   $��1��6)   $��Stretch=�66   $��5,5,0,0q�6T   $	��Top=�6}   $��5q�6�   5      5      