Examples here belong to "OWIN and Microsoft Katana 101" book.

This is an implementation of an OWIN middleware that returns "Hello World" response

<h1>OwinHost.exe method of hosting</h1>
1) You need to install an OWIN host (OwinHost.exe) to use this class library project.<br/>
a) Install chocolately.<br/>
b) Run command "cinst OwinHost" to get OwinHost.exe<br/>
c) You can run "OwinHost /?" to find out more.<br/>

2) Open package manager console, type in "Install-Package Owin" for HelloWorld project.
3) Change the output directory of the dlls to \bin directory using Visual Studio<br/>
a) Right click the class project > Properties.<br/>
b) On left tool bar > Build. In Output section, change output path to "bin\"<br/>

To run the dll on OwinHost
1) Open command prompt
2) Navigate to the bin directory of your dll with cd command.
3) Type in "OwinHost"
4) The server will be started. Go to internet browser and type in http://localhost:5000/
5) A "Hello World" message will be there.

<h1>Hosting using console application (custom host)</h1>
1) Create a new console application project (I created MyConsoleAppHost project)
2) Open package manager console, type in "Install-Package Microsoft.Owin.SelfHost"
3) Type in code for Program.cs
4) Build and run console application.
5) The server will be started. Go to internet browser and type in http://localhost:5000/

<h1>Hosting in ASP.NET web application, IIS (non ASP.NET core)</h1>
This focuses on the Microsoft.Owin.Host.SystemWeb namespace, not Microsoft.Owin.Host.IIS.
1) Create Empty web application project (MyIISHost), without adding any libraries.
2) Set it as startup project.
3) In package manager console, type in "Install-Package Microsoft.Owin.Host.SystemWeb"
4) Add reference to HelloWorld class library project.
5) Add HTML files and configuration in web.config
6) Rebuild and run.