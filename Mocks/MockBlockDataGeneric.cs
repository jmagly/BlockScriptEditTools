namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    /// <summary>
    /// Generic wrapper for individual data objects representing block data to be mocked in a test
    /// </summary>
    /// <typeparam name="T">type of block</typeparam>
    public class MockBlockData<T> : MockBlockData, IBlockData<T> where T : class
    {
        internal MockBlockData() : base() { }
        public MockBlockData(string name, IDictionary<string, string> properties = null) : base(name, typeof(T), properties) { }
    }
}
