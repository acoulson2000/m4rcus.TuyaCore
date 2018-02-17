using m4rcus.TuyaCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace TuyaService.Controllers {
    public class TuyaController : Controller {
        // /tuya?ip={id}&device={device}& key={key}&action={action}
        [Route("tuya")]
        public string Get(string ip, string deviceId, string key, string operation) {

            Console.WriteLine("action=" + operation);

            var GetPlug = new Func<TuyaPlug>(() => new TuyaPlug() {
                IP = ip,
                LocalKey = key,
                Id = deviceId
            });

            if (operation == "status") {
                var device = GetPlug();
                if (device.GetStatus().Result.Powered) {
                    return "ON";
                } else {
                    return "OFF";
                }
            }

            if (operation == "on") {
                var device = GetPlug();
                device.SetStatus(true).Wait();
                Thread.Sleep(2000);
                return (device.GetStatus().Result.Powered) ? "ON": "OFF";
            }

            if (operation == "off") {
                var device = GetPlug();
                device.SetStatus(false).Wait();
                Thread.Sleep(2000);
                return (device.GetStatus().Result.Powered) ? "ON" : "OFF";
            }

            return "ERROR";

        }
    }
}
