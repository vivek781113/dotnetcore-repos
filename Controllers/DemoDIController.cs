using System.Dynamic;
using Microsoft.AspNetCore.Mvc;
using WebAPI3_1.Services.Interfaces;

namespace WebAPI3_1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DemoDIController : ControllerBase
    {
        private readonly IOperationTransient _transientOperation1;
        private readonly IOperationTransient _transientOperation2;
        private readonly IOperationTransient _transientOperation3;
        private readonly IOperationScoped _scopedOperation1;
        private readonly IOperationScoped _scopedOperation2;
        private readonly IOperationScoped _scopedOperation3;
        private readonly IOperationSingleton _singletonOperation1;
        private readonly IOperationSingleton _singletonOperation2;
        private readonly IOperationSingleton _singletonOperation3;

        public DemoDIController(
            IOperationTransient transientOperation1,
            IOperationTransient transientOperation2,
            IOperationTransient transientOperation3,
            IOperationScoped scopedOperation1,
            IOperationScoped scopedOperation2,
            IOperationScoped scopedOperation3,
            IOperationSingleton singletonOperation1,
            IOperationSingleton singletonOperation2,
            IOperationSingleton singletonOperation3
            )
        {
            _transientOperation1 = transientOperation1;
            _transientOperation2 = transientOperation2;
            _transientOperation3 = transientOperation3;
            _scopedOperation1 = scopedOperation1;
            _scopedOperation2 = scopedOperation2;
            _scopedOperation3 = scopedOperation3;
            _singletonOperation1 = singletonOperation1;
            _singletonOperation2 = singletonOperation2;
            _singletonOperation3 = singletonOperation3;
        }

        public IActionResult GetInstances()
        {
            dynamic result = new ExpandoObject();
            result.TransientInstacne1 = _transientOperation1.OperationId;
            result.TransientInstacne2 = _transientOperation2.OperationId;
            result.TransientInstacne3 = _transientOperation3.OperationId;
            result.ScopedInstacne1 = _scopedOperation1.OperationId;
            result.ScopedInstacne2 = _scopedOperation2.OperationId;
            result.ScopedInstacne3 = _scopedOperation3.OperationId;
            result.SingletonInstacne1 = _singletonOperation1.OperationId;
            result.SingletonInstacne2 = _singletonOperation2.OperationId;
            result.SingletonInstacne3 = _singletonOperation3.OperationId;
            return Ok(result);
        }

    }


}