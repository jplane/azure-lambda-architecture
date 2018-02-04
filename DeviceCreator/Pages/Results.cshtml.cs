using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace DeviceCreator.Pages
{
    public class ResultsModel : PageModel
    {
        private readonly IHostingEnvironment _hosting;

        public ResultsModel(IHostingEnvironment hosting)
        {
            _hosting = hosting;
        }

        [TempData]
        public string devId { get; set; }

        [TempData]
        public string devConnectionString { get; set; }

        public string Script { get; set; }

        private string Filter(string line)
        {
            line = line.Replace("[DEVICE-CONNECTION-STRING]", this.devConnectionString);
        
            line = line.Replace("[DEVICE-ID]", this.devId);

            return line;
        }

        public void OnGet()
        {
            var webRoot = _hosting.WebRootPath;
            this.Script = Filter(System.IO.File.ReadAllText(webRoot + "/js/device-logic.js"));
        }
    }
}
