System Setup
------------
This project requires MSBuild and NUnit.


Build
-----
To build the project, run:

    build.bat

You can also override the default target, configuration, and .NET framework version by giving command line arguments:

    build.bat Rebuild Release v3.5


The build.bat script expects MSBuild.exe to be on your PATH.  If you run it
from a Visual Studio cmd shell, this should not be a problem.  However, if you
run it from some other shell (e.g. cygwin), you may need to append something
like:

    C:\WINDOWS\Microsoft.NET\Framework\v3.5

to your PATH environment variable.


