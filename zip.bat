MKDIR \Build\King.Azure.Imaging

XCOPY King.Azure.Imaging.WebJob\bin\Release\*.dll Build\King.Azure.Imaging\ /Y
XCOPY King.Azure.Imaging.WebJob\bin\Release\*.exe Build\King.Azure.Imaging\ /Y
XCOPY King.Azure.Imaging.WebJob\bin\Release\*.config Build\King.Azure.Imaging\ /Y

ECHO COMPRESS