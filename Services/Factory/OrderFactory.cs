using WebAPI3_1.Services.Implementations;
using WebAPI3_1.Services.Interfaces;

namespace WebAPI3_1.Services.Factory
{
    public static class OrderFactory
    {
        public static IOrderCalculator GetOrderCalculator(string orderCode)
        {
            if (orderCode == "A")
             return new ParcelOrder();
            return new DispatchOrder();

        }
    }
}