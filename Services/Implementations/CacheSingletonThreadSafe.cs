using System;

namespace WebAPI3_1.Services.Implementations
{
    public class CacheSingletonThreadSafe
    {
        private static CacheSingletonThreadSafe _instance;
        private static readonly object _padLock = new object();
        private CacheSingletonThreadSafe()
        {

        }

        public static CacheSingletonThreadSafe Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_padLock)
                    {

                        if (_instance == null)
                        {
                            _instance = new CacheSingletonThreadSafe();
                            _instance.CachcedId = Guid.NewGuid();
                        }

                    }
                }
                return _instance;
            }

        }

        public Guid CachcedId { get; set; }

    }
}