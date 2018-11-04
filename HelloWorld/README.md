This is an implementation of an OWIN middleware that returns "Hello World" response

1) You need to install an OWIN host (OwinHost.exe) to use this class library project.
2) Change the output directory of the dlls to \bin directory using Visual Studio
a) Right click the class project > Properties.
b) On left tool bar > Build. In Output section, change output path to "bin\"

To run the dll on OwinHost
1) Open command prompt
2) Navigate to the bin directory of your dll with cd command.
3) Type in "OwinHost"
4) The server will be started. Go to internet browser and type in http://localhost:5000/
5) A "Hello World" message will be there.
