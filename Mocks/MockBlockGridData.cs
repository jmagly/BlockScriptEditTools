namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;    
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    /// <summary>
    /// Base helper for implementers of <see cref="IBlockGridData" />
    /// </summary>
    public abstract class MockBlockGridData : IBlockGridData
    {
        private List<IBlockData> blocks = new List<IBlockData>();
        private List<Type> availableBlockTypes = new List<Type>();

        public MockBlockGridData(bool autoCreate = false)
        {
            AutoCreate = autoCreate;
            Initialize();
        }

        /// <summary>
        /// Flag to determine if the data set should populate itself upon construction
        /// </summary>
        protected bool AutoCreate
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the list of unique block types available within the dataset.
        /// </summary>
        public List<Type> AvailableBlockTypes
        {
            get
            {
                return availableBlockTypes;
            }
            set
            {
                availableBlockTypes = value;
            }
        }

        /// <summary>
        /// Gets or sets the list of block data to be used in testing
        /// </summary>
        public List<IBlockData> Blocks
        {
            get
            {
                return blocks;
            }
            set
            {
                blocks = value;
            }
        }

        /// <summary>
        /// Initializes the block data object
        /// </summary>
        protected virtual void Initialize()
        {
            FindAvailableBlockTypes();
            CreateGrid(blocks);
        }

        /// <summary>
        /// Lookup method for determining the available block types that can be used in the test simulation
        /// </summary>
        /// <remarks>
        /// Base version of this method scans Sandbox.Common.dll to ensure all new blocks are automatically available for testing. This should only 
        /// be overridden for very specific purposes.
        /// </remarks>
        protected virtual void FindAvailableBlockTypes()
        {
            AvailableBlockTypes.AddRange(Assembly.LoadFrom("Sandbox.Common.dll").GetTypes().Where(
            t => t.IsInterface
                && typeof(IMyTerminalBlock).IsAssignableFrom(t)
                && t.Namespace.Contains("Ingame")
                && !t.Name.Equals("IMyTerminalBlock")
                && !t.Name.Equals("IMyFunctionalBlock")
                && !t.Name.Contains("Base")));
        }   

        /// <summary>
        /// Creates a new data grid 
        /// </summary>
        /// <param name="blocks">block data to populate</param>
        protected abstract void CreateGrid(List<IBlockData> blocks);
    }
}
