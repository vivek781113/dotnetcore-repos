using System;
using WebAPI3_1.Services.Interfaces;

namespace WebAPI3_1.Services.Implementations
{
    public class Operation : IOperationTransient, IOperationScoped, IOperationSingleton, IOperationSingletonInstance
    {
        Guid _guid;
        public Operation(): this(Guid.NewGuid())
        {
            
        }
        public Operation(Guid guid)
        {
            _guid = guid;
        }
        public Guid OperationId => _guid;
    }
}