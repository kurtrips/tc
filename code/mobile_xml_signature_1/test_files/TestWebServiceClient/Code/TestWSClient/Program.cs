using System;
using System.Collections.Generic;
using System.Text;
using TestWSClient.com.w3schools.www;

namespace TestWSClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TempConvert tc = new TempConvert();
            tc.UseDefaultCredentials = true;

            short res = tc.CelsiusToFahrenheit(48);
        }
    }
}
