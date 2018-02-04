using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace DeviceCreator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IoTConfig _iotConfig;

        public IndexModel(IoTConfig config)
        {
            _iotConfig = config;
        }

        [BindProperty]
        public string DeviceId { get; set; }

        [TempData]
        public string devId { get; set; }

        [TempData]
        public string devConnectionString { get; set; }

        public string Error { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var device = new Device(this.DeviceId);
            
            try
            {
                device = await _iotConfig.RegistryManager.AddDeviceAsync(device);

                devId = this.DeviceId;
                
                devConnectionString = $"HostName={_iotConfig.HostName};DeviceId={this.DeviceId};SharedAccessKey={device.Authentication.SymmetricKey.PrimaryKey}";

                return RedirectToPage("Results");
            }
            catch (DeviceAlreadyExistsException)
            {
                this.Error = "Device id already exists, please try another.";
                return Page();
            }
        }
    }
}
