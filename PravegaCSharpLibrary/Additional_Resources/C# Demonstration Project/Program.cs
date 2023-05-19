///
/// File: Program.cs
/// File Creator: John Sbur
/// Purpose:  A demonstration program of what this library could do. Not all features are in the demonstration. 
///
namespace Pravega
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;
    using Pravega;
    using Pravega.ClientFactoryModule;
    using Pravega.Config;
    using Pravega.Shared;
    using Pravega.Event;
    using Pravega.Byte;
    using Pravega.ControllerCli;
    using Pravega.Pathgen;
    using Pravega.Utility;
    using PravegaWrapperTestProject;
    using NUnit.Framework;
    using System.Security.Cryptography.X509Certificates;

    public static class Program
    {
        private static Random random = new Random();
        public static List<byte> RandomPayload(int length)
        {
            List<byte> returnList = new List<byte>();
            for (int i = 0; i < length; i++)
            {
                returnList.Add((byte)(random.Next() % 256));
            }
            return returnList;
 
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        static void Main()
        {
            // Local variables
            bool demoing = true;
            string option;
            string option2;
            string? holder = null;
            List<byte> demoPayload;
            byte[] demoPayload2;
            Scope demoScope;
            Stream demoStream;
            ScopedStream demoScopedStream;
            StreamConfiguration demoStreamConfig;
            ControllerClient demoClient;
            ByteWriter demoWriter;
            ByteReader demoReader;

            // Beginning of the demo
            Console.WriteLine("Starting of the demo...");

            // Initialize the Client Factory and Pathgen
            var cwd = System.IO.Directory.GetCurrentDirectory();
            String code_base = "Project_Code_Base";
            int indexTo = cwd.IndexOf(code_base);
            String return_string;
            // If IndexOf could not find code_base String
            if (indexTo == -1)
            {
                return_string = "";
            }
            else
            {
                return_string = cwd.Substring(0, indexTo + code_base.Length);
                return_string += @"\cSharpTest\PravegaCSharpLibrary\target\debug\deps\";
            }
            Environment.CurrentDirectory = return_string;
            ClientFactory.Initialize();

            // Demo Loop
            while (demoing)
            {

                // Display features to demonstrate
                Console.WriteLine("Welcome to the C# Pravega Demo. Select the feature you want to test:");
                Console.WriteLine("1) Create Scope");
                Console.WriteLine("2) Create Stream");
                Console.WriteLine("3) Send Data to a local server");
                Console.WriteLine("4) Recieve Data from a local server");
                Console.WriteLine("5) Exit");

                // Get user input
                holder = Console.ReadLine();
                Console.Clear();
                if (holder == null)
                {
                    option = "-1";
                }
                else
                {
                    option = holder;
                }

                // Perform a demo based on the user input
                switch (option)
                {
                    // Create scope demo
                    case "1":

                        // Get stream and scope name
                        Console.WriteLine("Creating Scope, enter a Scope name:");
                        holder = Console.ReadLine();
                        if (holder == null)
                        {
                            Console.WriteLine("An error occurred while recieving your input. It has been set to the default name 'testScope'");
                            option = "testScope";
                        }
                        else
                        {
                            option = holder;
                        }

                        // Create scope based on name inputted
                        Console.WriteLine("Input recieved. Processing...");
                        Console.WriteLine("Retrieving Controller Client...");
                        demoClient = ClientFactory.FactoryControllerClient;
                        Console.WriteLine("Controller Client recieved. Proceeding to create stream...");
                        demoScope = new Scope();
                        demoScope.NativeString = option;
                        demoClient.CreateScope(demoScope).GetAwaiter().GetResult();
                        Console.WriteLine("Scope successfully created under name: " + option);
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        // Explanation
                        Console.WriteLine("");
                        Console.WriteLine("So what just happened?");
                        Console.WriteLine("The short answer is that we created a scope on a local Pravega Server.");
                        Console.WriteLine("Scopes you can imagine as the file folder of the stream world and it gives");
                        Console.WriteLine("an environment to store streams under. To create the scope, our library");
                        Console.WriteLine("accessed Pravega Rust code in order to first make a ControllerClient,");
                        Console.WriteLine("a mechanism that helps create streams and scopes. Then, our library accessed ");
                        Console.WriteLine("Pravega Rust code in order to create the scope inputted. ");
                        Console.WriteLine("You can also see that this happened very fast!");

                        // Pause until user continues
                        Console.WriteLine(Environment.NewLine + "Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;


                    // Create Stream Demo
                    case "2":

                        // Get inputs
                        Console.WriteLine("Creating Stream, enter a Scope name first (This will be the environment the stream will be created in):");
                        holder = Console.ReadLine();
                        if (holder == null)
                        {
                            Console.WriteLine("An error occurred while recieving your input. It has been set to the default name 'testScope'");
                            option = "testScope";
                        }
                        else
                        {
                            option = holder;
                        }
                        Console.WriteLine("Now enter the Stream name:");
                        holder = Console.ReadLine();
                        if (holder == null)
                        {
                            Console.WriteLine("An error occurred while recieving your input. It has been set to the default name 'testStream'");
                            option2 = "testStream";
                        }
                        else
                        {
                            option2 = holder;
                        }

                        // Create scope based on name inputted
                        Console.WriteLine("Input recieved. Processing...");
                        Console.WriteLine("Retrieving Controller Client...");
                        demoClient = ClientFactory.FactoryControllerClient;
                        Console.WriteLine("Controller Client recieved. Proceeding to create stream configuration...");
                        demoStreamConfig = new StreamConfiguration();
                        demoStreamConfig.ConfigScopedStream.Scope = new CustomCSharpString(option);
                        demoStreamConfig.ConfigScopedStream.Stream = new CustomCSharpString(option2);
                        Console.WriteLine("Controller Client created. Proceeding to create stream...");
                        try
                        {
                            demoClient.CreateStream(demoStreamConfig).GetAwaiter().GetResult();
                            Console.WriteLine("stream successfully created under name: " + option2);
                        }
                        catch
                        {
                            Console.WriteLine("An error occurred while creating the stream. It is likely the cause of the scope inputted previously not");
                            Console.WriteLine("being present in the Pravega Server.");
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        // Explanation
                        Console.WriteLine("");
                        Console.WriteLine("So what just happened?");
                        Console.WriteLine("The short answer is that we created a stream on a local Pravega Server.");
                        Console.WriteLine("Scopes you can imagine as the file folder of the stream world and it gives");
                        Console.WriteLine("an environment to store streams under. To create the stream, our library");
                        Console.WriteLine("accessed Pravega Rust code in order to first make a ControllerClient,");
                        Console.WriteLine("a mechanism that helps create streams and scopes. Then, our library accessed ");
                        Console.WriteLine("Pravega Rust code in order to create the strean inputted. This stream was created");
                        Console.WriteLine("with default parameters according to a generated stream configuration, but the");
                        Console.WriteLine("behvaior of the stream can be changed in our library such as adjusting how fast");
                        Console.WriteLine("it can give and take data.");
                        Console.WriteLine("You can also see that this happened very fast!");

                        // Pause until user continues
                        Console.WriteLine(Environment.NewLine + "Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        break;

                    // Write case
                    case "3":

                        // Get inputs
                        Console.WriteLine("Byte Writing Demo:");
                        Console.WriteLine("Before starting, we need a scope and a stream to write to. Enter a Scope name:");
                        holder = Console.ReadLine();
                        if (holder == null)
                        {
                            Console.WriteLine("An error occurred while recieving your input. It has been set to the default name 'testScope'");
                            option = "testScope";
                        }
                        else
                        {
                            option = holder;
                        }
                        Console.WriteLine("Now enter the Stream name:");
                        holder = Console.ReadLine();
                        if (holder == null)
                        {
                            Console.WriteLine("An error occurred while recieving your input. It has been set to the default name 'testStream'");
                            option2 = "testStream";
                        }
                        else
                        {
                            option2 = holder;
                        }

                        Console.WriteLine("Proceeding to creating a ByteWriter...");
                        demoScopedStream = new ScopedStream();
                        demoScopedStream.Scope = new CustomCSharpString(option);
                        demoScopedStream.Stream = new CustomCSharpString(option2);

                        try
                        {
                            demoWriter = ClientFactory.CreateByteWriter(demoScopedStream).GetAwaiter().GetResult();
                            Console.WriteLine("Successfully created a ByteWriter to interact with your stream...");
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            Console.Clear();
                        }
                        catch
                        {
                            Console.WriteLine(Environment.NewLine + "An error occurred while creating the stream. It is likely the cause of the scope or stream inputted previously not");
                            Console.WriteLine("being present in the Pravega Server.");
                            Console.WriteLine("Press any button to continue...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }

                        Console.WriteLine("Now that we have created a mechanism to interact with the stream. We need a payload to send.");
                        Console.WriteLine("Bytes can be represented as numbers from 0 to 255, so we have randomly generated a payload of 1000 bytes to send to the server");
                        demoPayload = RandomPayload(1000);
                        Console.WriteLine("Demo payload contains: ");
                        for (int i = 0; i < 1000; i++)
                        {
                            Console.Write(demoPayload[i].ToString() + " ");
                        }
                        Console.WriteLine(Environment.NewLine + "Writing payload...");
                        demoWriter.Write(demoPayload).GetAwaiter().GetResult();
                        Console.WriteLine("Finished writing payload...");

                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        Console.WriteLine("");
                        Console.WriteLine("So what just happened?");
                        Console.WriteLine("Our library was able to write a small payload of bytes in blazing speed without using any code in C# to access the server!");
                        Console.WriteLine("All the server processing was done in Rust and C# was able to use language integration technology in order to indirectly");
                        Console.WriteLine("use that code.");
                        break;

                    // Read Case
                    case "4":

                        // 
                        Console.WriteLine("Unlike the other demonstrations, this will be very hands off and we'll only ask you to press a key to continue at different points");
                        Console.WriteLine("This demonstration will showcase the ability to read data previously written to a Pravega server" + Environment.NewLine);
                        Console.WriteLine("First we create a ByteWriter and ByteReader based on a unique scope and stream:");

                        // Create stream and scope for demo
                        demoScope = new Scope();
                        demoScope.NativeString = RandomString(8);
                        demoStream = new Stream();
                        demoStream.NativeString = RandomString(8);
                        demoScopedStream = new ScopedStream();
                        demoScopedStream.Stream = demoStream;
                        demoScopedStream.Scope = demoScope;
                        demoStreamConfig = new StreamConfiguration();
                        demoStreamConfig.ConfigScopedStream = demoScopedStream;
                        Console.WriteLine("Scope: " + demoScopedStream.Scope.NativeString);
                        Console.WriteLine("Stream: " + demoScopedStream.Stream.NativeString);
                        ClientFactory.FactoryControllerClient.CreateScope(demoScope);
                        ClientFactory.FactoryControllerClient.CreateStream(demoStreamConfig);
                        Console.WriteLine("Finished creating stream and scope...");
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        // Create Byte readers and writers
                        Console.WriteLine("Next we move to create a ByteReader and ByteWriter to interact with the stream");
                        demoWriter = ClientFactory.CreateByteWriter(demoScopedStream).GetAwaiter().GetResult();
                        demoReader = ClientFactory.CreateByteReader(demoScopedStream).GetAwaiter().GetResult();
                        Console.WriteLine("Finished creating byte writer and byte reader...");
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        // Create payload and write it to the stream
                        Console.WriteLine("Next we create a payload of 10 bytes to send to the stream with the byte writer");
                        demoPayload = RandomPayload(10);
                        Console.WriteLine("Demo payload contains: ");
                        for (int i = 0; i < 10; i++)
                        {
                            Console.Write(demoPayload[i].ToString() + " ");
                        }
                        Console.WriteLine(Environment.NewLine + "Writing payload...");
                        demoWriter.Write(demoPayload).GetAwaiter().GetResult();
                        Console.WriteLine("Finished writing payload...");
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();

                        // Read the data back and print
                        Console.WriteLine("Finally, we read the data back to verify...");
                        demoPayload2 = demoReader.Read(10).GetAwaiter().GetResult();
                        Console.WriteLine("Recieved payload contains: ");
                        for (int i = 0; i < 10; i++)
                        {
                            Console.Write(demoPayload2[i].ToString() + " ");
                        }
                        Console.WriteLine(Environment.NewLine + "Finished reading payload...");
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();

                        // Explanation
                        Console.WriteLine("");
                        Console.WriteLine("So what just happened?");
                        Console.WriteLine("Our library was able to read a small payload of bytes through utilizing Pravega's Rust code from C#.");
                        Console.WriteLine("While this is a small demonstration and a small payload, our library is also capable of editing the stream");
                        Console.WriteLine("in addition to reading and writing. Funcioning methods allow a developer to truncate a stream, seal a stream");
                        Console.WriteLine("flush data from a stream, and seek to a certain point on a stream.");
                        Console.WriteLine("Press any button to continue...");
                        Console.ReadKey();
                        Console.Clear();
                        break;

                    // Exit case
                    case "5":
                        demoing = false;
                        break;

                    // Catch case
                    default:
                        Console.WriteLine("Invalid option chosen");
                        Console.WriteLine("Valid options are: 1, 2, 3, 4");
                        break;
                }
            }



        }



    }
}