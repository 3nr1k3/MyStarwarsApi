using System;
using System.Net;
using System.Threading.Tasks;

using System.Linq;

namespace MyStarwarsApi.Helpers
{
    public class NetworkHelper
    {
        public static async Task<String> getIpAddress()
        {
            IPHostEntry ipHostInfo = await Dns.GetHostEntryAsync(Dns.GetHostName());
            IPAddress iPAddress = ipHostInfo.AddressList[0];

            ipHostInfo.AddressList.ToList().ForEach(a => {
                Console.WriteLine(a.ToString());
            });

            return iPAddress.ToString();
        }
    }
}