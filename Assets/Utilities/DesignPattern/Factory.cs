namespace MDesingPattern
{
    namespace MFactory
    {
        public interface IProduct
        {
            void InitProduct();
        }
        public interface IFactory
        {
            IProduct GetProduct();
        }

        public interface IAbstractFactory
        {
            IFactory GetFactory(string name);
            void AddFactory(string name, IFactory factory);
        }
    }
}