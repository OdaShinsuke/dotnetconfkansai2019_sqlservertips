using Microsoft.SqlServer.Dac;
using System;

namespace _5_DacFX
{
    class Program
    {
        static string connstr = @"Data Source=MSI;Initial Catalog=dotnetconf2019;Integrated Security=True;Connect Timeout=5;Encrypt=False;";

        static void Main(string[] args)
        {
            // BACPAC作る();
            // BACPACからDB作る();
        }

        static void BACPAC作る()
        {
            var dac = new DacServices(connstr);
            dac.ExportBacpac("d:/dotnetconf2019.bacpac", "dotnetconf2019");
        }

        static void BACPACからDB作る()
        {
            var dac = new DacServices(connstr);
            using (var package = BacPackage.Load("d:/dotnetconf2019.bacpac"))
            {
                dac.ImportBacpac(package, "new_dotnetconf2019");
            }
        }
    }
}
