
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

        public interface IFactoryGroup
        {
            IFactory GetFactory(string name);
            void AddFactory(string name, IFactory factory);
        }

        #region Unity Mono Factory
        public interface IMonoProduct
        {
            UnityEngine.GameObject gameObject {get;}
            void InitMonoProduct();
        }

        public interface IMonoFactory
        {
            IMonoProduct GetMonoProduct();
        }

        public interface IMonoFactoryGroup
        {
            IMonoFactory GetMonoFactory(string name);
            void AddMonoFactory(string name, IMonoFactory monoFactory);
        }
        #endregion
        // End of Unity Mono Factory
    }
}