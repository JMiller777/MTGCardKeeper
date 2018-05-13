echo off

rem batch file to run a script to create MagicDB
rem Created 11/9/17 - John Miller
rem Last Updated 05/09/2018 - John Miller

sqlcmd -S localhost -E -i MagicDB.sql

rem sqlcmd -S localhost/sqlexpress -E -i MagicDB.sql
rem sqlcmd -S localhost/mssqlserver -E -i MagicDB.sql

ECHO .
ECHO if no error messages appear DB was created
PAUSE