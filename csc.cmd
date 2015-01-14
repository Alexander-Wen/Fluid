@rem Run the C# compiler with the path set temporarily.
@rem George Freeman 2011-Sep-12/2013-Sep-08
@rem Notes:
@rem   All command-line arguments are passed through to csc.exe.
@rem   This finds any 64-bit version before a 32-bit version.
@rem   Versions are found in the order 4, 3, 2.

@setlocal

:bits64
@if not exist %SystemRoot%\Microsoft.NET\Framework64 goto :bits32

  :b64version4
  @if not exist %SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe goto :b64version3
    @set path=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\;%path%
    @echo.Launching the 64-bit version 4 C# compiler.
    @goto :compile

  :b64version3
  @if not exist %SystemRoot%\Microsoft.NET\Framework64\v3.5\csc.exe goto :b64version2
    @set path=%SystemRoot%\Microsoft.NET\Framework64\v3.5\;%path%
    @echo.Launching the 64-bit version 3.5 C# compiler.
    @goto :compile

  :b64version2
  @if not exist %SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\csc.exe goto :error2
    @set path=%SystemRoot%\Microsoft.NET\Framework64\v2.0.50727\;%path%
    @echo.Launching the 64-bit version 2 C# compiler.
    @goto :compile

:bits32
@if not exist %SystemRoot%\Microsoft.NET\Framework goto :error1

  :b32version4
  @if not exist %SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe goto :b32version3
    @set path=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\;%path%
    @echo.Launching the 32-bit version 4 C# compiler.
    @goto :compile

  :b32version3
  @if not exist %SystemRoot%\Microsoft.NET\Framework\v3.5\csc.exe goto :b32version2
    @set path=%SystemRoot%\Microsoft.NET\Framework\v3.5\;%path%
    @echo.Launching the 32-bit version 3.5 C# compiler.
    @goto :compile

  :b32version2
  @if not exist %SystemRoot%\Microsoft.NET\Framework\v2.0.50727\csc.exe goto :error2
    @set path=%SystemRoot%\Microsoft.NET\Framework\v2.0.50727\;%path%
    @echo.Launching the 32-bit version 2 C# compiler.
    @goto :compile

:error1
@echo.Could not find a .NET Framework directory.
@goto :end

:error2
@echo.Could not find a C# compiler.
@goto :end

:compile
@csc.exe %*

:end
@endlocal