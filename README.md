What is UIDemo?
---------------
UIDemo is a UI client intended to demonstrate one way of using QuickFIX/n in a UI application.  It uses FIX.4.2, which was an arbitrary choice.

Out of the box, it's configured to be able to connect to QuickFIX/n's "Executor" example.  UIDemo can connect and send orders to Executor, and Executor will respond by filling any order it receives.  (UIDemo can also send Cancels and CancelReplaces, but Executor will reject them.)

UIDemo is not meant to be a production-ready client.  It is merely an example, and will likely not work as-is on any real exchange, given that virtually all exchanges have custom modifications to the standard FIX data dictionary that UIDemo uses.  ([Connamara](http://Connamara.com) has created a modified version of UIDemo that successfully works against CME AutoCert+, demonstrating that UIDemo behavior is somewhat representative of production behavior.)

UIDemo uses the Microsoft's Windows Presentation Framework (aka WPF), and is designed more or less using the Model-View-ViewModel pattern (MVVM).


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


Run
---
Inside VisualStudio, you can simply run the "UIDemo" project, and the UI window should pop up.  The default configuration will connect to QuickFIX/n's Executor example running on your local machine.  If Executor is already running, click "Connect" to connect to it.

From a DOS prompt, cd into the directory that contains Executor.exe and run it.  Make sure this directory also contains the file quickfix.cfg, as Executor.exe will attempt to load it.

Once connected, you can change to the Orders tab and send orders.  Executor should accept your orders and respond with "Fill" execution reports.

The News tab allows you to send News messages (internally, this code demonstrates an example of creating a message that contains a repeating group).

The MessageView tab lets you see all FIX messages that were sent and received.  You could also see these messages in the "fixlogs" directory that UIDemo creates.


Help
----
Issues specific to UIDemo can be submitted at [Github](https://github.com/connamara/qfn_uidemo/issues).

For general UIDemo help, you should use the [general QuickFIX/n mailing list](https://groups.google.com/forum/?hl=en#!forum/quickfixn).

For help with QuickFIX/n in general, see [the QuickFIX/n help page](http://quickfixn.org/help).
