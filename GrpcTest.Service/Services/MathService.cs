using Grpc.Core;
using GrpcTest.Shared;

namespace GrpcTest.Service.Services
{
    public class MathService : Shared.MathService.MathServiceBase
    {
        public override Task<MathAddResponse> Add(MathAddRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MathAddResponse
            {
                Sum = request.A + request.B
            });
        }
    }
}
