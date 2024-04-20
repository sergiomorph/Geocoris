using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;






namespace FireBaseTest
{
    internal class Program
    {
        static void Main(string[] args)
        {


            Start();


            Console.ReadLine();
        }

        public static async void Start()
        {
            IFirebaseConfig config = new FirebaseConfig()
            {
                AuthSecret = "V4wbx1APT3FIGXolyeL4yoQQ3RgAMfQTygaoNP1o",
                BasePath = "https://geocoris-61934-default-rtdb.europe-west1.firebasedatabase.app/"
            };

            FirebaseClient client = new FirebaseClient(config);

            if (client != null)
            {
                Console.WriteLine("Connection bueno");

                await SetResponse(client); 
            }
            else
            {
                Console.WriteLine("Connection failed");
            }
        }

        static async Task SetResponse(IFirebaseClient client)
        {
            try
            {

                Point1 pt = new Point1(4, 3, 3.333);
                FirebaseResponse response = await client.SetAsync("control_points/" + 123, pt);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Response set successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to set response.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }


    }
}
