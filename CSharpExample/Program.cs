using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp;
using FireSharp.Interfaces;
using FireSharp.Config;
using FireSharp.Response;
using System.Threading;
using Newtonsoft.Json;






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



                Dictionary<string, Point> pointsDict = CreatePointsWithKeys();


                //WIP Listener
                EventStreamResponse update = await client.OnAsync("chat", (sender, args, context) => {
                    System.Console.WriteLine(args.Data);
                });



                await ClearControlPointsChildren(client);
                await SetResponse(client, pointsDict);


                Dictionary<string, Point> response = new Dictionary<string, Point> {};

                response = await GetResponse(client);         

                PrintPoints(response);
            }
            else
            {
                Console.WriteLine("Connection failed");
            }
        }


        


        //HTTP Methods

        /// <summary>
        /// DELETE method for cleaning the control points storage
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task ClearControlPointsChildren(IFirebaseClient client)
        {
            try
            {
                // First, get all children keys under 'control_points'
                FirebaseResponse response = await client.GetAsync("control_points");
                if (response.StatusCode == System.Net.HttpStatusCode.OK && !string.IsNullOrEmpty(response.Body) && response.Body != "null")
                {
                    Dictionary<string, object> data = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Body);
                    List<Task> deleteTasks = new List<Task>();

                    // Iterate over the keys and delete each
                    foreach (var key in data.Keys)
                    {
                        deleteTasks.Add(client.DeleteAsync($"control_points/{key}"));
                    }

                    // Await all delete tasks
                    await Task.WhenAll(deleteTasks);
                    Console.WriteLine("All children of control points cleared successfully.");
                }
                else
                {
                    Console.WriteLine("No control points to clear or failed to fetch data.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }


        /// <summary>
        /// SET method
        /// </summary>
        /// <param name="client"></param>
        /// <param name="pointsWithKeys"></param>
        /// <returns></returns>
        static async Task SetResponse(IFirebaseClient client, Dictionary<string, Point> pointsWithKeys)
        {     

            try
            {
                foreach (var pt in pointsWithKeys)
                {                    
                    FirebaseResponse response = await client.SetAsync("control_points/" + pt.Key, pt);
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Console.WriteLine($"Point set successfully at key: {pt.Key}");
                    }
                    else
                    {
                        Console.WriteLine("Failed to set point at key: " + pt.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        /// <summary>
        /// GET Method
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public static async Task<Dictionary<string, Point>> GetResponse(IFirebaseClient client)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("control_points/");

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Console.WriteLine("Success retrieving data.");
                    var result = response.ResultAs<Dictionary<string, PointWrapper>>();

                    // Transforming the result to remove the wrapper
                    var transformedResult = new Dictionary<string, Point>();
                    foreach (var kvp in result)
                    {
                        transformedResult.Add(kvp.Key, kvp.Value.Value);
                    }

                    return transformedResult;

                }
                else
                {
                    Console.WriteLine("Failed to retrieve data.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }
        }




        //Helper Methods


        /// <summary>
        /// Creates a dictionary of keys and point for testing purposes
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Point> CreatePointsWithKeys()
        {
            var pointsWithKeys = new Dictionary<string, Point>();

            // This loop will create 8 points with unique keys
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {                
                // Generating a simple key. In a real application, consider a more robust method to generate unique keys
                string key = $"point_A{i + 1}";
                                
                double X = random.NextDouble() * random.Next(0,200); // Random X between 1 and 100
                double Y = random.NextDouble() * random.Next(0, 200); // Random Y between 1 and 200
                double Z = 0; // Random Z between 0 and 10

                // Create a new point with random values (or specific values)
                Point point = new Point(X, Y, Z);                

                pointsWithKeys.Add(key, point);
            }

            for (int i = 0; i < 4; i++)
            {
                // Generating a simple key. In a real application, consider a more robust method to generate unique keys
                string key = $"point_B{i + 1}";

                double X = random.NextDouble() * random.Next(200, 400); // Random X between 1 and 100
                double Y = random.NextDouble() * random.Next(200, 400); // Random Y between 1 and 200
                double Z = 500; // Random Z between 0 and 10

                // Create a new point with random values (or specific values)
                Point point = new Point(X, Y, Z);

                pointsWithKeys.Add(key, point);
            }

            for (int i = 0; i < 4; i++)
            {
                // Generating a simple key. In a real application, consider a more robust method to generate unique keys
                string key = $"point_C{i + 1}";

                double X = random.NextDouble() * random.Next(400, 600); // Random X between 1 and 100
                double Y = random.NextDouble() * random.Next(400, 600); // Random Y between 1 and 200
                double Z = 1000; // Random Z between 0 and 10

                // Create a new point with random values (or specific values)
                Point point = new Point(X, Y, Z);

                pointsWithKeys.Add(key, point);
            }

            return pointsWithKeys;
        }
        public static void PrintPoints(Dictionary<string, Point> retrievedPoints)
        {
            // Print the retrieved points
            if (retrievedPoints != null)
            {
                Console.WriteLine("Retrieved data from Firebase:");
                foreach (var pointEntry in retrievedPoints)
                {                    
                    Console.WriteLine($"Key: {pointEntry.Key}, X: {pointEntry.Value.x}, Y: {pointEntry.Value.y}, Z: {pointEntry.Value.z}");
                }
            }
            else
            {
                Console.WriteLine("Failed to retrieve data from Firebase.");
            }
        }

        public class PointWrapper
        {
            public Point Value { get; set; }
        }
        
    }



}
