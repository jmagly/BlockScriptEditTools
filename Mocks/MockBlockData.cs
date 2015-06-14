namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    /// <summary>
    /// Block data instance containing information to be used in mock creation
    /// </summary>
    public class MockBlockData : IBlockData
    {
        private IDictionary<string, string> properties = new Dictionary<string, string>();

        internal MockBlockData() { }

        public MockBlockData(string name, Type blockType, IDictionary<string, string> properties = null)
        {
            Name = name;
            BlockType = blockType;
            Properties = properties ?? Properties;
        }

        /// <summary>
        /// Gets or sets the name of the block 
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the block
        /// </summary>
        public virtual Type BlockType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the properties of the block
        /// </summary>
        public virtual IDictionary<string, string> Properties
        {
            get
            {
                return properties;
            }
            set
            {
                properties = value;
            }
        }

        /// <summary>
        /// Casts block data from one compatible type to the other
        /// </summary>
        /// <remarks>this call has no protection, the user muse know that the cast will be successful or unexpected exceptions are likely to occur</remarks>
        /// <typeparam name="T">type to cast to</typeparam>
        /// <returns>version of MockBlockData`1 with the newly provided type parameter</returns>
        public MockBlockData<T> As<T>() where T : class
        {
            return new AsBlockData<T>(this);
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Name, BlockType.ToString());
        } 
    }
}
