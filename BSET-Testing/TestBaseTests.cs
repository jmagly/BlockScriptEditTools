namespace BSET_Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;

    using Xunit;

    using BSET.Mocks;    
    using BSET.Testing;

    /// <summary>
    /// Xunit test to validate basic test usage using simple control/block scripts.
    /// </summary>
    public class TestBaseTests : BlockScriptTest
    {
        public TestBaseTests() : base()
        {

        }

        [Fact]
        public void Initialize_Test()
        {
            var blocks = new List<IMyTerminalBlock>();

            foreach (var type in GridData.AvailableBlockTypes)
            {
                GetBlocksOfType(type, blocks);
            }

            // One block for each type + 1 for the default block that runs the block script
            Assert.True(blocks.Count == 57);
            Assert.All(
                GridData.Blocks, 
                new Action<IBlockData>((block) => Assert.True(blocks.Any(b => b.CustomName.Equals(block.Name) 
                    && b.GetType().GetInterfaces().Any(i => block.BlockType.IsAssignableFrom(i))))));
        }

        protected override IBlockGridData AcquireTestData()
        {
            return new SimpleGridData(true);
        }
    }
}
