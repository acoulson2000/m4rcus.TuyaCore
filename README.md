# Tuya Smart Plug Service (.NET Standard 2.0 & Raspberry Pi)

I forked this project from https://github.com/Marcus-L/m4rcus.TuyaCore because I want to control Tuya (and compatible) devices from OpenHab2
and I found that using Marcus' executable via the exec binding, or even using this plugin https://github.com/unparagoned/njsTuya

<img src="tuya-plug.jpg">

## Requirements
* .NET Core 2.0

For information on supported devices, obtaining the IP, device ID, and key that are necessary to control your device,
see the [README](https://github.com/Marcus-L/m4rcus.TuyaCore/blob/master/README.md) at [Marcus' repo](https://github.com/Marcus-L/m4rcus.TuyaCore)
  
## Rapberry Pi installation
To build and install on Rapsberry Pi:
* Install the .Net SDK on your desktop: https://www.microsoft.com/net/learn/get-started/windows
* Install .Net Core on the Pi. I followed the "Install the .NET Core Runtime on the Raspberry Pi" instructions under Step 3 on this page: https://blogs.msdn.microsoft.com/david/2017/07/20/setting_up_raspian_and_dotnet_core_2_0_on_a_raspberry_pi/
* Build the web service: At command line, go to the TuyaService directory under this solution, and run "dotnet publish". This will build the service and copy all the runtime dependencies to <solution_home>\TuyaService\bin\HPD\Debug\netcoreapp2.0.
* Tar everything in that directory and scp it to your Raspberry Pi. Unpack it into a target directory, like /opt/TuyaService.  Since I was using it with OpenHab2, which runs as the user, "openhabian", I did a chmod -R 755 * on the target directory to make everything readable by all users.
* Launch the service via something like: "dotnet /opt/TuyaService/TuyaService.dll"

## Usage


## Credits

Protocol details from @codetheweb and @clach04:
* https://github.com/Marcus-L/m4rcus.TuyaCore
* https://github.com/unparagoned/njsTuya
