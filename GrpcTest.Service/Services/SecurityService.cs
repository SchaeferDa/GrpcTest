using Grpc.Core;
using GrpcTest.Shared;
using System.Security.Cryptography;

namespace GrpcTest.Service.Services
{
    public class SecurityService : Shared.SecurityService.SecurityServiceBase
    {
        public override Task<SecurityHashSha512Response> HashSha512(SecurityHashSha512Request request, ServerCallContext context)
        {
            return Task.FromResult(new SecurityHashSha512Response
            {
                Hash = Google.Protobuf.ByteString.CopyFrom(SHA512.HashData(request.Input.ToByteArray()))
            });
        }
    }
}
