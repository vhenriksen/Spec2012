   M S B A M L   `   `   `  ����  WpfApplication"    WpfApplication.DialogToolCUD   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Create new tool��5   	   $
��Left=�6!   $	��Top=�6<    � SizeToContent$ WidthAndHeight=�6T   .��5      � -   5        $��gridToolCUD��5      $	��Top=�6   $
��Left=�66    � RowDefinitions 5   
   �� 5      -   5         �Name$ RowId��5      $
��Auto �6)   �� 5      $
��Auto �6   �� 5	      $
��Auto �6   �� 5
      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��Auto �6   .+�5   
   �� -   5        $��labelToolId��5   :   $
��ID: ��6   $
��Left=�6   $	��Top=�6M   $��0��6e   $��0��6u   $	��5,0q�6�   $��Bottom=�6�   �� 5   
   -   5        $��labelDescription��5   R   $��Description��6   $
��Left=�6&   $��5,0,5,0q�6A   $	��Top=�6j   $��0��6�   $��1��6�   $��Bottom=�6�   �� 5   
   -   5        $��textBoxDescription��5   \   $��75��6    �� AcceptsReturn . $��Stretch=�63   $��5q�6Q   $	��Top=�6v   $��0��6�   $��2��6�    �� TextWrapping$
 Wrap=�6�   " �� VerticalScrollBarVisibility$
 Auto=�6�   $	��180��6�   �� 5   
   -   5        $��stackPanelButtons��5   2   $��Center=�6   $��Stretch=�6K   $��0��6g   $��6��6w    �� Orientation$ 
Horizontal=�6�   .+�5      �� -   5        $��buttonOkUpdate��5   .   $��Ok��6   $��23��6"   	 �� 	IsDefault	 . $��75��6r   $��5q�6}   �� 5      -   5        $��buttonCancel��5   2   $��Cancel��6   $��23��6&   $��75��6F   $��5q�6Q   
 �� IsCancel
 . 6\   5      �� 5   
   -	   5        $��labelBuyDate��5   V   $��DATE BOUGHT��6   $��Center=�6&   $��	10,0,10,0q�6C   $	��Top=�6j   $��0��6�   $��4��6�   $
��Left=�6�   &   System.Windows.Controls.Calendar  5   
   -
   5        $��calendarBuydate��5   O   $	��170��6   $��Stretch=�6    $��5,0,5,0q�6>   $	��Top=�6f   $	��180��6~   $��0��6�   $��5��6�   �� 5   
   -   5        $��checkBoxIsInUse��5   T   $��
 is in use��6   $
��Left=�6(   $��5,0,0,0q�6C   $��0��6k   $��3��6{   $	��Top=�6�   5      5      