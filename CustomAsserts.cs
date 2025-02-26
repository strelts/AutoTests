using ColvirAutoTests;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    class CustomAsserts
    {
        public static bool IsBoolean(JValue value)
        {
            return value.Type == JTokenType.Boolean;
        }

        public static bool IsNumeric(JValue value)
        {
            return value.Type == JTokenType.Integer || value.Type == JTokenType.Float;
        }

        public static bool IsString(JValue value)
        {
            return value.Type == JTokenType.String;
        }

        public void CheckIsString(JValue value)
        {
            Assert.IsTrue(CustomAsserts.IsString(value));
        }

        public void CheckIsBoolean(JValue value)
        {
            Assert.IsTrue(CustomAsserts.IsBoolean(value));
        }
        public void CheckIsNumeric(JValue value)
        {
            Assert.IsTrue(CustomAsserts.IsNumeric(value));
        }
    }
}
