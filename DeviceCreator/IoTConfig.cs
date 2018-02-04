using System;
using Microsoft.Azure.Devices;

namespace DeviceCreator
{
    public class IoTConfig
    {
        public string HostName { get; set; }

        public RegistryManager RegistryManager { get; set; }
    }
}
