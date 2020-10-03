echo off
:loop  
timeout -t 1 >nul  
for %%i in (Assets\Graphics\%1.psd) do echo %%~ai|find "a">nul || goto :loop
echo file was changed
convert Assets\Graphics\%1.psd Assets\Graphics\%1.psb
echo convert Assets\Graphics\%1.psd Assets\Graphics\%1.psb
echo file was converted
rem do workload
attrib -a Assets\Graphics\%1.psd
goto :loop