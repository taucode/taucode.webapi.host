﻿using Microsoft.Extensions.Configuration;
using TauCode.WebApi.Host.Tests.App;

namespace TauCode.WebApi.Host.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }
    }
}
