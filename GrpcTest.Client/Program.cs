using Grpc.Net.Client;
using System.Buffers.Text;
using System.Diagnostics;

namespace GrpcTest.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var a = int.Parse(args[0]);
            var b = int.Parse(args[1]);
            var input = new byte[int.Parse(args[2])];
            new Random().NextBytes(input);

            var c = int.Parse(args[3]);

            long swSum = 0;
            var sw = new Stopwatch();

            #region Math/gRPC

            swSum = 0;

            using (var channel = GrpcChannel.ForAddress("https://localhost:7106"))
            {
                var client = new Shared.MathService.MathServiceClient(channel);

                for (int i = 0; i < c; i++)
                {
                    sw.Restart();

                    await client.AddAsync(new()
                    {
                        A = a,
                        B = b
                    });

                    sw.Stop();

                    swSum += sw.Elapsed.Microseconds;
                }
            }

            Console.WriteLine($"Math/gRPC: {swSum / c}µs");

            #endregion MathgRPC

            #region Math/REST

            swSum = 0;

            using (var client = new RestClient("https://localhost:7106"))
            {
                for (int i = 0; i < c; i++)
                {
                    sw.Restart();

                    await client.AddAsync(new()
                    {
                        A = a,
                        B = b
                    });

                    sw.Stop();

                    swSum += sw.Elapsed.Microseconds;
                }
            }

            Console.WriteLine($"Math/REST: {swSum / c}µs");

            #endregion Math/REST

            #region Security/gRPC

            swSum = 0;

            using (var channel = GrpcChannel.ForAddress("https://localhost:7106"))
            {
                var client = new Shared.SecurityService.SecurityServiceClient(channel);

                for (int i = 0; i < c; i++)
                {
                    sw.Restart();

                    await client.HashSha512Async(new()
                    {
                        Input = Google.Protobuf.ByteString.CopyFrom(input)
                    });

                    sw.Stop();

                    swSum += sw.Elapsed.Microseconds;
                }
            }

            Console.WriteLine($"Security/gRPC: {swSum / c}µs");

            #endregion gRPC

            #region Security/REST

            swSum = 0;

            using (var client = new RestClient("https://localhost:7106"))
            {
                for (int i = 0; i < c; i++)
                {
                    sw.Restart();

                    await client.Hashsha512Async(new()
                    {
                        Input = System.Convert.ToBase64String(input)
                    });

                    sw.Stop();

                    swSum += sw.Elapsed.Microseconds;
                }
            }

            Console.WriteLine($"Security/REST: {swSum / c}µs");

            #endregion REST
        }
    }
}