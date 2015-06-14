namespace BSET.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;    
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    using Mocks;

    /// <summary>
    /// Grid data populated by creating one data entry for each block type as noted in <see cref="AvailableBlockTypes"/>
    /// </summary>
    public class SimpleGridData : MockBlockGridData
    {
        public SimpleGridData(bool autoCreate = false)
            : base(autoCreate) 
        { }

        /// <summary>
        /// Creates a new grid data and stores it for usage
        /// </summary>
        /// <param name="blocks">collection to use for storage</param>
        protected override void CreateGrid(List<IBlockData> blocks)
        {
            if (!AutoCreate)
            {
                return;
            }

            foreach (var blockType in AvailableBlockTypes)
            {
                blocks.Add(new MockBlockData(string.Format("{0} - Test 1", blockType.Name), blockType));
            }
        }
    }
}
