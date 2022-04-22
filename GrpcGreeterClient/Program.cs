﻿using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeterClient;

using var channel = GrpcChannel.ForAddress("https://localhost:7022");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(new HelloRequest { Name = "GhfDllT" });

Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();