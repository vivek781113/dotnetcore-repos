using System;

namespace WebAPI3_1.Utils
{
    public class QueryParameters
    {
        const int MAX_SIZE = 100;
        private int _size = 50;

        public int Page { get; set; }

        public int Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = Math.Min(MAX_SIZE, value);
            }
        }
    }

}