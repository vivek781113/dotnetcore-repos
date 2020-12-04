using WebAPI3_1.Services.Interfaces;

namespace WebAPI3_1.Services.Implementations
{
    public class DispatchOrder : IOrderCalculator
    {
        public double CalculateOrder(string orderCode)
        {
            return 2.8;
        }
    }
}