@echo off
echo Copying plugins to PluginLoader...

mkdir PluginLoader\bin\Debug\net10.0\Plugins 2>nul

copy Plugin1\bin\Debug\net10.0\Plugin1.dll PluginLoader\bin\Debug\net10.0\Plugins\
copy Plugin2\bin\Debug\net10.0\Plugin2.dll PluginLoader\bin\Debug\net10.0\Plugins\
copy PluginWithDependencies\bin\Debug\net10.0\PluginWithDependencies.dll PluginLoader\bin\Debug\net10.0\Plugins\
copy PluginFramework\bin\Debug\net10.0\PluginFramework.dll PluginLoader\bin\Debug\net10.0\Plugins\

echo Done!
pause