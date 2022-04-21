using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SEIU32BJEmpire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SampleEmpireServerController : Controller
    {

        [HttpGet("ping")]
        public string Ping()
        {
            return "pong";
        }

        [HttpPost("response")]
        public string AcceptResponse(string body)
        {
            var reader = new StreamReader(Request.Body);
            var rawMessage = reader.ReadToEnd();
            Debug.WriteLine("rawMessage => " + rawMessage);
            return "pong";
        }

        [HttpPost("notify-recieved")]
        public string AcceptNotifyRecieved()
        {
            var reader = new StreamReader(Request.Body);
            var rawMessage = reader.ReadToEnd();
            Debug.WriteLine("rawMessage => " + rawMessage);
            return "pong";
        }
    }
}