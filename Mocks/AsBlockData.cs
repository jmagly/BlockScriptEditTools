namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Wrapper for switching block data types between compatible interface sub-types
    /// </summary>
    /// <typeparam name="T">type to cast to</typeparam>
    public class AsBlockData<T> : MockBlockData<T> where T : class
    {
        private MockBlockData parent;

        public AsBlockData(MockBlockData parent)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        public override string Name
        {
            get
            {
                return parent.Name;
            }
            set
            {
                parent.Name = value;
            }
        }

        /// <summary>
        /// Gets or sets the properties of the block
        /// </summary>
        public override IDictionary<string, string> Properties
        {
            get
            {
                return parent.Properties;
            }
            set
            {
                parent.Properties = value;
            }
        }
    }
}
