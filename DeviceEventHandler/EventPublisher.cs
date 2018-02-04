
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeviceEventHandler
{
    public static class EventPublisher
    {
        [FunctionName("EventPublisher")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "post")]HttpRequestMessage req, TraceWriter log)
        {
            dynamic events = await req.Content.ReadAsAsync<object>();

            var sasKey = ConfigurationManager.AppSettings["aeg-sas-key"];

            var topicUri = ConfigurationManager.AppSettings["topic-uri"];

            var http = new HttpClient();

            http.DefaultRequestHeaders.Add("aeg-sas-key", sasKey);

            foreach (var evt in events)
            {
                var wrapperEvt = new
                {
                    id = evt.msgid,
                    eventType = "deviceInfo",
                    subject = evt.deviceid,
                    eventTime = evt.processedts,
                    data = evt
                };

                var response = await http.PostAsync(topicUri, new[] { wrapperEvt }, new JsonMediaTypeFormatter());

                log.Info("Published device event.");
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
