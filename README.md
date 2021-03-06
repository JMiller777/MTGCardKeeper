# Magic: The Gathering Card Collection Management System
Author: John Miller   
2018/05/13

This desktop/web application allows users to create, retrieve, update 
and delete cards and card images. It was created using the Visual Studio 
IDE, and implements the Microsoft Model View Controller (MVC) code 
architecture, as well as a web front end with ASP.NET web controls and 
script integration. 

The Create scripts for the application's Microsoft SQL Server Database 
can be found in the 'sql' folder. These files should be run prior to
attempting to build and run the application itself, as all card and
user information is accessed and manipulated by the data access layer
using its stored procedures.

The project still contains test code and commenting to minimize password
complexity to make signing in easier for testing purposes. After updating 
the database and performing the necessary code-first migrations with the 
console Package Manager, you should be able to access all functions of the
application with the following user credentials:

WEB: 
username: admin@magic.com
password: P@ssw0rd

DESKTOP:
username: jen@magic.com
password: newuser (for first sign-on, then change it when prompted)


Background of the Project: 
I began working of version 1 of this application during my first .NET 
course at Kirkwood Community College in the Spring semester of 2017 and
I've been rewriting and making improvements to it ever since. 
I think it does a good job of showcasing the skills I've gained through 
my studies so far. I hope you enjoy using and working with this program
as much as I have enjoyed creating it.

Thanks for taking the time to look at one of the larger projects I've
been working on for the last year and a half. 

- John Miller
