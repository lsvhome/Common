@ECHO OFF

: fix for user profile path contains spaces like "C:\Users\FirstName LastName" - local variables should be fixed by using short filenames.
@SET USERPROFILE=C:\Users\lsv

PowerShell -NoProfile -NoLogo -ExecutionPolicy unrestricted -Command "[System.Threading.Thread]::CurrentThread.CurrentCulture = ''; [System.Threading.Thread]::CurrentThread.CurrentUICulture = '';& '%~dp0run.ps1' default-build %*; exit $LASTEXITCODE"
