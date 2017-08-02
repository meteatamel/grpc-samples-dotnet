// Copyright 2017, Google Inc.
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are
// met:
//
//     * Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above
// copyright notice, this list of conditions and the following disclaimer
// in the documentation and/or other materials provided with the
// distribution.
//     * Neither the name of Google Inc. nor the names of its
// contributors may be used to endorse or promote products derived from
// this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
// LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
// A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
// OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
// SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
// LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
// DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
// THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
// OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

using System;
using Grpc.Core;
using GreeterProtos;
using System.Threading.Tasks;

namespace GreeterClient
{
    class Program
    {
        const string DefaultHost = "localhost";
        const int Port = 50051;

        public static void Main(string[] args)
        {
            RunAsync(args).Wait();
        }

        private static async Task RunAsync(string[] args)
        {
            var host = args.Length == 1 ? args[0] : DefaultHost;
            var channelTarget = $"{host}:{Port}";

            Console.WriteLine($"Target: {channelTarget}");

            // Create a channel
            var channel = new Channel(channelTarget, ChannelCredentials.Insecure);

            try
            {
                // Create a client with the channel
                var client = new GreetingService.GreetingServiceClient(channel);

                // Create a request
                var request = new HelloRequest
                {
                    Name = "Mete - on C#",
                    Age = 34,
                    Sentiment = Sentiment.Happy
                };

                // Send the request
                Console.WriteLine("GreeterClient sending request");
                var response = await client.GreetingAsync(request);

                Console.WriteLine("GreeterClient received response: " + response.Greeting);
            }
            finally
            {
                // Shutdown
                await channel.ShutdownAsync();
            }
        }
    }
}