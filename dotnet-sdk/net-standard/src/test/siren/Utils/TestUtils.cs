using System;
using System.Collections.Generic;
using System.Text;

namespace StagwellTech.SirenTests.Utils
{
    public static class TestUtils
    {

        public static string ResourcesDirectory => $@"{Environment.CurrentDirectory}\..\..\..\..\resources\";
        public static string DefaultConfigPath => $@"{ResourcesDirectory}siren.config.yaml";

    }
}
