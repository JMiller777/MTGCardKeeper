   M S B A M L   `   `   `  ����  MagicCardCollectionSystem1    +MagicCardCollectionSystem.frmUpdatePassword   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Change Password��5   	   $	��260��6!   $	��440��6.    � WindowStartupLocation$ CenterScreen=�6:    � 
ResizeMode$ NoResize=�6_   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      �� 5	      �� 5
      �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� -   5         ��Name$ tblkMessage��5      
�����F�$
��Wrap=�6@    �� TextAlignment$ Center=�6T   $
��50 5q�6k   $��0��6y   $��0��6�   $��2��6�   $��14(�6�   $
��Bold"�6�   $��Segoe WP Block)�6�   USYour password has expired. You must enter a new password or you will be logged out.6;  �� 5   
   $��1��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�6U   .��6p   Old Password:6   G� 5   
   -   5         G�Name$ pwdOldPassword��5      $��1��6.   $��1��6;   $
��10 5q�6K    G� Password$ newuser��6Y   �� 5   
   $��2��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�6U   .��6p   New Password:6   G� 5   
   -   5        $ pwdNewPassword��5      $��2��6.   $��1��6;   $
��10 5q�6K   $ P@ssw0rd��6Y   �� 5   
   $��3��6   $��0��6   $��10q�6-   $��Right=�69   $��Center=�6U   .��6p   Retype Password:6�   G� 5   
   -   5        $ pwdRetypedPassword��5      $��3��62   $��1��6?   $
��10 5q�6O   $ P@ssw0rd��6]   �� 5   
   -   5        	 ��Name$	 	btnSubmit��5      $��4��6$   $��2��61   $��150 10q�6E   $��Submit��6U   6f   5      5      