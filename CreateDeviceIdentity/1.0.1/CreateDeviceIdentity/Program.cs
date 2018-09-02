using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        static RegistryManager registryManager;
        static string connectionString = "HostName=trafficIoT.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=gnpmxqawgwF4uybTebpFsh3XpFvHq5XbA40I5RIjNNA=";

        static void Main(string[] args)
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
            AddDeviceAsync().Wait();
            Console.ReadLine();

            // Generated Edison01 Device ID: WxVdxNDST9AVqtr9W1g48CR9f3MZXWITs+hZjtIr+L0=
            // Generated Edison02 Device ID: 28FwXl3gY6k76DayzTWSLOcTmte7oG+zMIYFLoKW9Z8=
            // Generated Lenovo01 Device ID: NtN3QbBlBJ/o88+Pf+vNu3fuFA8buNb0nx7hH1lqk60=
        }

        private async static Task AddDeviceAsync()
        {
            string deviceId = "Lenovo01";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync(new Device(deviceId));
            }
            catch (DeviceAlreadyExistsException)
            {
                device = await registryManager.GetDeviceAsync(deviceId);
            }
            Console.WriteLine("Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey);
        }
    }
}
