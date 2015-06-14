namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    /// <summary>
    /// A single data object with information to see a simulated block instance
    /// </summary>
    /// <typeparam name="T">type of block</typeparam>
    public interface IBlockData<T> where T : class
    {
        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the property data for the block
        /// </summary>
        IDictionary<string, string> Properties { get; set; }
    }

    /// <summary>
    /// A single data object with information to see a simulated block instance
    /// </summary>
    public interface IBlockData
    {
        /// <summary>
        /// Gets or sets the name of the block
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the property data for the block
        /// </summary>
        IDictionary<string, string> Properties { get; set; }

        /// <summary>
        /// Gets or sets the type of the simulated block
        /// </summary>
        Type BlockType { get; set; }
    }
}
