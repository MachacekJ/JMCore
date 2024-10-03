// using ACore.Client.Services.Http;
// using MediatR;
//
// namespace ACore.Tests.Client.CQRST.TestOpenGenericT;
//
// public class Fruit<TCode>
// {
//     public TCode Code { get; } = default!;
//     public virtual int Weight { get; } = 0;
// }
//
// public class Apple : Fruit<AppleCode>
// {
//     public override int Weight { get; } = 10;
// }
//
// public class Orange : Fruit<OrangeCode>
// {
//     public override int Weight { get; } = 12;
// }
//
// public class AppleCode : FruitCode;
//
// public class OrangeCode : FruitCode;
//
// public class FruitCode;
//
// public class CountFruitCommand<TFruit, TCode> : IRequest<TFruit>
// {
//     public TCode Code { get; } = default!;
//     public int Count { get; set; }
// }
//
// public class CountFruitRequestHandler<TFruit, TCode> : IRequestHandler<CountFruitCommand<TFruit, TCode>, TFruit>
//     where TFruit : Fruit<TCode>, new()
//     where TCode : FruitCode
// {
//     private readonly IJMHttpClientFactory _cache;
//
//     // public CountFruitRequestHandler()
//     // {
//     //     
//     // }
//     public CountFruitRequestHandler(IJMHttpClientFactory cache)
//     {
//         _cache = cache;
//     }
//
//     public Task<TFruit> Handle(CountFruitCommand<TFruit, TCode> command, CancellationToken cancellationToken)
//     {
//         var res = new TFruit();
//         _cache.CreateAuthClientAsync();
//         _ = res.Code;
//         _ = command.Code;
//         _ = command.Count;
//         return Task.FromResult(res);
//         //return Task.FromResult($"{typeof(TFruit).Name} Count: {request.Count}");
//     }
// }