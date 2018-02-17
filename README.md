# Tuya Smart Plug REST Service (.NET Standard 2.0 & Raspberry Pi)

I forked this project from https://github.com/Marcus-L/m4rcus.TuyaCore because I want to control Tuya (and compatible) devices from OpenHab2 running on a Raspberry Pi (Raspbian distro) 
and I found that using Marcus' executable via the exec binding, or even using this plugin https://github.com/unparagoned/njsTuya resulted in pretty long response times, since there
is significant overhead loading up the executable environments to run the executable (or script runtime in the case of the njsTuya plugin). The njsTuya script also seemed a little flakey.

To eliminate the startup time, I built a simple REST service wrapper around Marcus' core component. I also tweeked the console .csproj file slightly so that when you build it, 
it pulls all the dependencies into the build dir, so that if you want to use it on the Pi all the dependencies are includeds.

## Requirements
* .NET Core 2.0 and SDK on build machine
* .NET Core 2.0 on Pi

  
## Rapberry Pi installation
To build and install on Rapsberry Pi:
* Install the .Net SDK on your desktop: https://www.microsoft.com/net/learn/get-started/windows
* Install .Net Core on the Pi. I followed the "Install the .NET Core Runtime on the Raspberry Pi" instructions under Step 3 on [this page](https://blogs.msdn.microsoft.com/david/2017/07/20/setting_up_raspian_and_dotnet_core_2_0_on_a_raspberry_pi/). 
Note that there is currently no .NET Core SDK available for Raspian Linux running on the Raspberry Pi (i.e. ARM32). This means that the .NET SDK components necessary to run my service are not installed. This isn't a problem though, since I
modified the project to build all the dependencies into the target directory when you build it.
* Build the web service: At command line, go to the TuyaService directory under this solution, and run "dotnet publish". This will build the service and copy all the runtime dependencies to <solution_home>\TuyaService\bin\HPD\Debug\netcoreapp2.0.
* Tar everything in that directory and scp it to your Raspberry Pi. Unpack it into a target directory, like /opt/TuyaService.  Since I was using it with OpenHab2, which runs as the user, "openhabian", I did a chmod -R 755 * on the target directory to make everything readable by all users.
* Launch the service via something like: "dotnet /opt/TuyaService/TuyaService.dll"
* For convenience, I've inlcuded a rudimentary init.d script in the project root. Copy it to your /etc/init.d directory as "tuya" and symlink it to your /etc/rc<x>.d directories for automatic startup and shutdown.

## Usage

For information on supported devices, obtaining the IP, device ID, and key that are necessary to control your device,
see the [README](https://github.com/Marcus-L/m4rcus.TuyaCore/blob/master/README.md) at [Marcus' repo](https://github.com/Marcus-L/m4rcus.TuyaCore)

Perform http GET operations using the following URL:

	http://localhost:5000/tuya?ip=<ip_address>&deviceId=<deviceId>&key=<key>&operation=<op>
	
where <ip_address>, <deviceId>, and <key> can be obtained as described in Marcus' README 
and <op> is one of: on, off, or status

For example:
 wget -o out.txt "http://localhost:5000/tuya?ip=192.168.0.101&deviceId=0120015260091453a970&key=5f5f784cd82d449b&operation=status"

This makes it very easy to use in item definitions in OpenHab2 using the HTTP binding. For example, here are two items from my default.items file, one which obtains the device status as a string every 60 seconds, and a Switch, which turns my switch on or off:
```
String TuyaArtLamp                      "Art Lamp"                                                      {http="<[tuyaservice:60000:REGEX((.*))]"} /* defined in services/http.cfg */
Switch Light_Living_ArtLamp             "Art Lamp switch"                       (GF_Living)             {http=">[ON:GET:http://localhost:5000/tuya?ip=192.168.2.41&deviceId=002003295ccf7f393512&key=f1bd1c710bbd699b&operation=on] >[OFF:GET:http://localhost:5000/tuya?ip=192.168.2.41&deviceId=002003295ccf7f393512&key=f1bd1c710bbd699b&operation=off]"}
``` 
 and in my /etc/openhab2/services/http.cfg file:
``` 
tuyaservice.url=http://localhost:5000/tuya?ip=192.168.2.41&deviceId=002003295ccf7f393512&key=f1bd1c710bbd699b&operation=status
tuyaservice.updateInterval=120000
```

## Credits
* https://github.com/Marcus-L/m4rcus.TuyaCore
* https://github.com/unparagoned/njsTuya
