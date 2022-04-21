using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Generators
{
    [TestClass]
    public class ClassGenerator
    {
        [TestMethod]
        public void GenerateFromSql()
        {
            string result = "";
            var lines = Samples.CRM_Form_SQL.Split(',');
            foreach(var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var cleanLine = line.Replace("[", "");
                cleanLine = cleanLine.Replace("]", "");
                var parts = cleanLine.Split(' ');
                var name = parts[0];
                var type = parts[1];
                var start = type.IndexOf('(');
                var end = type.IndexOf(')');
                if (start > 0)
                    type = type.Remove(start, end - start + 1);
                if (Samples.Types.TryGetValue(type, out string replacedType))
                    result += Samples.ModelTemplate.Replace("column_name", name).Replace("type", replacedType);
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GenerateToJsonFromCSharp()
        {
            string result = "{";
            var lines = Samples.CSharp_OBJECT.Split('\n');
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;
                var type = line.Split(' ').First();
                var name = line.Split(' ')[1];
                var jsonType = "";
                switch(type)
                {
                    case "int":
                        jsonType = "number";
                        break;
                    case "bool":
                        jsonType = "boolean";
                        break;
                    default:
                        jsonType = "string";
                        break;
                }
                result += $"\"{name}\": \"{jsonType}\",";
                result += "\n";
            }
            Assert.IsNotNull(result);
        }
    }
}
