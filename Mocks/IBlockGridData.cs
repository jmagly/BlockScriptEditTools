namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    /// <summary>
    /// A data object containing all the information necessary to create a simulated grid for testing
    /// </summary>
    public interface IBlockGridData
    {
        /// <summary>
        /// Gets or sets the block types that can be created
        /// </summary>
        List<Type> AvailableBlockTypes { get; set; }

        /// <summary>
        /// Gets or sets the block data to be used in the simulation
        /// </summary>
        List<IBlockData> Blocks { get; set; }
    }
}
