namespace BSET.Testing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using VRage.Collections;

    using Sandbox.ModAPI.Ingame;
    using Sandbox.ModAPI.Interfaces;

    using Moq;

    using Mocks;
    using System.Reflection;

    /// <summary>
    /// Base class for block script testing, handles creation and maintenance of basic grid context
    /// </summary>
    public abstract class BlockScriptTest : IDisposable
    {
        /// <summary>
        /// Represents a grid object running in the test simulation using test provided data
        /// </summary>
        protected class SimulatedGridObject
        {
            /// <summary>
            /// Actual block instance used in the test
            /// </summary>
            public IMyTerminalBlock Block { get; set; }

            /// <summary>
            /// Data used to seed the object instance
            /// </summary>
            public IBlockData TestData { get; set; }

            public override string ToString()
            {
                return TestData.ToString();
            }
        }

        protected const string DefaultExecutionBlockName = "CPU -=DEFAULT=- TEST001";

        /// <summary>
        /// data instance used to create a simulated test grid
        /// </summary>
        private IBlockGridData gridData;
        
        /// <summary>
        /// Output for all LCDs in the grid
        /// </summary>
        private Dictionary<string, string> displayOutputs = new Dictionary<string, string>();

        /// <summary>
        /// Log of all calls go Apply to execute a block action
        /// </summary>
        private Queue<ExecutedAction> executionLog = new Queue<ExecutedAction>();

        /// <summary>
        /// Collection of all blocks in the grid simulation
        /// </summary>
        private List<SimulatedGridObject> gridBlocks = new List<SimulatedGridObject>();

        /// <summary>
        /// Mock of the actual grid used during testing
        /// </summary>
        private Mock<IMyGridTerminalSystem> currentMockGts;

        private IBlockData executionBlockData;
        private IMyProgrammableBlock executionBlock;

        /// <summary>
        /// Creates a new instance <see cref="BlockScriptTest"/>
        /// </summary>
        public BlockScriptTest()
        {
            ResetGrid();
        }

        ~BlockScriptTest()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the current test data used to create the grid
        /// </summary>
        protected IBlockGridData GridData
        {
            get
            {
                return gridData;
            }
        }

        /// <summary>
        /// Gets the collection of grid blocks currently running in the test simulation
        /// </summary>
        protected List<SimulatedGridObject> GridBlocks
        {
            get
            {
                return gridBlocks;
            }
        }

        /// <summary>
        /// Gets the mock of the GridTerminalSystem used in the test
        /// </summary>
        protected Mock<IMyGridTerminalSystem> CurrentMockGts
        {
            get
            {
                return currentMockGts;
            }
            set
            {
                currentMockGts = value;
            }
        }

        /// <summary>
        /// Gets the current GridTerminalSystem used in the test
        /// </summary>
        protected IMyGridTerminalSystem CurrentGts
        {
            get
            {
                return CurrentMockGts.Object;
            }
        }

        protected virtual string ExecutionBlockName
        {
            get
            {
                return DefaultExecutionBlockName;
            }
        }

        /// <summary>
        /// Gets or sets the block to act as the executor of the block script being tested
        /// </summary>
        protected IMyProgrammableBlock ExecutionBlock
        {
            get
            {
                return executionBlock;
            }
            set
            {
                executionBlock = value;
            }
        }

        /// <summary>
        /// Gets the display output data from the IMyTextPanel blocks running within the grid
        /// </summary>
        protected Dictionary<string, string> DisplayOutputs
        {
            get
            {
                return displayOutputs;
            }
        }

        /// <summary>
        /// Gets the log of all Apply actions currently captured
        /// </summary>
        protected Queue<ExecutedAction> ExecutionLog
        {
            get
            {
                return executionLog;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Creates a new grid instance and stores it as the current simulation grid
        /// </summary>
        /// <returns>grid instance created</returns>
        protected IMyGridTerminalSystem CreateGrid()
        {
            var grid = CreateGridMock().Object;

            InitializeGrid(grid);
            SetExecutionBlockContext(executionBlockData);

            return grid;
        }

        /// <summary>
        /// Initializes the grid provided with basic mocks
        /// </summary>
        /// <param name="grid">grid to configure</param>
        protected void InitializeGrid(IMyGridTerminalSystem grid)
        {
            var blocks = new List<IMyTerminalBlock>();

            foreach (var type in GridData.AvailableBlockTypes)
            {
                GetBlocksOfType(type, blocks);
            }
        }

        /// <summary>
        /// Resets the stored grid as if the test has just freshly been constructed
        /// </summary>
        /// <returns>newly created grid instance</returns>
        protected IMyGridTerminalSystem ResetGrid()
        {
            ClearGrid();
            return CreateGrid();
        }

        /// <summary>
        /// Creates a mock wrapped grid instance
        /// </summary>
        /// <returns>newly created mock instance</returns>
        protected virtual Mock<IMyGridTerminalSystem> CreateGridMock()
        {
            if (currentMockGts != null)
            {
                return currentMockGts;
            }

            var mockGrid = new Mock<IMyGridTerminalSystem>();

            gridData = AcquireTestData();

            // Add Execution Block Context Data
            executionBlockData = GetExecutionBlockData();
            gridData.Blocks.Add(executionBlockData);

            foreach (var blockType in gridData.AvailableBlockTypes)
            {
                ExecuteGenericMethod<Mock>(this, "SetupGridForBlockType", blockType, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, gridData, mockGrid);
            }

            mockGrid.Setup(g => g.GetBlockWithName(It.IsAny<string>())).Returns<string>(
                (name) => 
                    {
                        var simBlock = GridBlocks.FirstOrDefault(b => b.Block.CustomName.Equals(name));
                        return simBlock != null ? simBlock.Block : null;
                    });

            currentMockGts = mockGrid;

            return currentMockGts;
        }

        /// <summary>
        /// Method for determining data for testing
        /// </summary>
        /// <returns>data instance used for testing.</returns>
        protected virtual IBlockGridData AcquireTestData()
        {
            return new SimpleGridData();
        }

        protected virtual IBlockData GetExecutionBlockData()
        {
            return new MockBlockData<IMyProgrammableBlock>(ExecutionBlockName);
        }

        protected virtual void SetExecutionBlockContext(IBlockData executionBlockData)
        {           
            ExecutionBlock = (IMyProgrammableBlock)CurrentGts.GetBlockWithName(ExecutionBlockName);
        }

        /// <summary>
        /// Create a new simulated test block 
        /// </summary>
        /// <typeparam name="T">type to create</typeparam>
        /// <param name="name">name to create</param>
        /// <param name="getActionWithName">optional action for calls to GetActionWithName</param>
        /// <param name="apply">optional action for calls to Apply</param>
        /// <returns>newly created simulated block</returns>
        protected virtual T Create<T>(string name,
            Action<ExecutedAction> getActionWithName = null,
            Action<ExecutedAction> apply = null) where T : class
        {
            return CreateMock<T>(name).Object;
        }

        /// <summary>
        /// Create a new mocked test block
        /// </summary>
        /// <typeparam name="T">type to create</typeparam>
        /// <param name="name">name to create</param>
        /// <param name="getActionWithName">optional action for calls to GetActionWithName</param>
        /// <param name="apply">optional action for calls to Apply</param>
        /// <returns>newly created simulated block</returns>
        protected virtual Mock<T> CreateMock<T>(string name,
            Action<ExecutedAction> getActionWithName = null,
            Action<ExecutedAction> apply = null) where T : class
        {
            var mock = SEMock.CreateGridObject<T>(name, getActionWithName, (executedAction) =>
            {
                ExecutionLog.Enqueue(executedAction);

                if (apply != null)
                {
                    apply(executedAction);
                }
            });

            if (typeof(T).Equals(typeof(IMyTextPanel)))
            {
                ConfigurePanel(name, mock.As<IMyTextPanel>());
            }

            return mock;
        }

        /// <summary>
        /// Configure a <see cref="IMyTextPanel" /> instance for use with the test
        /// </summary>
        /// <typeparam name="T">panel type</typeparam>
        /// <param name="name">panel name</param>
        /// <param name="mock">mock instance for pre-test configuration</param>
        protected virtual void ConfigurePanel<T>(string name, Mock<T> mock) where T : class, IMyTextPanel
        {
            displayOutputs[name] = string.Empty;

            mock.Setup(p => p.WritePublicText(It.IsAny<string>(), It.IsAny<bool>())).Callback(new Action<string, bool>(
                (message, append) => 
                    {
                        displayOutputs[name] = append ? displayOutputs[name] + message : message;
                    }));

            mock.Setup(p => p.GetPublicText()).Returns(
                () =>
                {
                    return displayOutputs[name];
                });
        }

        /// <summary>
        /// Configure the grid to handle lookups for a specific blocktype. This is required for proper operation of methods like GetBlocksOfType`1
        /// </summary>
        /// <typeparam name="T">blocktype to configure for</typeparam>
        /// <param name="gridData">test grid data to use</param>
        /// <param name="mockGrid">mock grid instance to configure</param>
        protected void SetupGridForBlockType<T>(IBlockGridData gridData, Mock<IMyGridTerminalSystem> mockGrid)
        {
            var blockType = typeof(T);

            mockGrid.Setup(s => s.GetBlocksOfType<T>(It.IsAny<List<IMyTerminalBlock>>(), It.IsAny<Func<IMyTerminalBlock, bool>>()))
                    .Callback<List<IMyTerminalBlock>, Func<IMyTerminalBlock, bool>>(
                    (blocks, collect) =>
                    {
                        var currentBlocks = GridBlocks.Where(b => b.TestData.BlockType.Equals(blockType));

                        if (currentBlocks != null && currentBlocks.Count() > 0)
                        {
                            blocks.AddRange(currentBlocks.Select(b => b.Block));
                            return;
                        }

                        foreach (var dataObject in gridData.Blocks.Where(b => b.BlockType.Equals(blockType)))
                        {
                            var mockGridObject = ExecuteGenericMethod<Mock>(this, "CreateMock", dataObject.BlockType, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod, dataObject.Name, null, null);
                            blocks.Add((IMyTerminalBlock)mockGridObject.Object);
                            gridBlocks.Add(new SimulatedGridObject() { Block = (IMyTerminalBlock)mockGridObject.Object, TestData = dataObject });
                        }
                    });
        }

        /// <summary>
        /// Wrapper for normal GetBlocksOfType to allow looping through collections of types 
        /// </summary>
        /// <param name="typeParameter">type to lookup</param>
        /// <param name="blocks">block list to populate with results</param>
        /// <param name="collect">collection lookup</param>
        protected void GetBlocksOfType(Type typeParameter, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            ExecuteGenericMethod<IMyTerminalBlock>(CurrentGts, "GetBlocksOfType", typeParameter, blocks, collect);
        }

        /// <summary>
        /// Simple helper for using refection to execute a generic method when no non-generic version is available and the type parameter 
        /// is an instance rather than a compile time value.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="instance">instance to execute on</param>
        /// <param name="methodName">method name to invoke</param>
        /// <param name="typeParameter">input type parameter</param>
        /// <param name="parameters">input method parameters</param>
        /// <returns>result</returns>
        protected static T ExecuteGenericMethod<T>(object instance, string methodName, Type typeParameter, params object[] parameters) where T : class
        {
            var methodDefinition = instance.GetType().GetMethods().Where(m => m.Name.Equals(methodName)).FirstOrDefault();
            var method = methodDefinition.MakeGenericMethod(typeParameter);
            var result = method.Invoke(instance, parameters);

            return result != null ? (T)result : null;
        }

        /// <summary>
        /// Simple helper for using refection to execute a generic method when no non-generic version is available and the type parameter 
        /// is an instance rather than a compile time value.
        /// </summary>
        /// <typeparam name="T">return type</typeparam>
        /// <param name="instance">instance to execute on</param>
        /// <param name="methodName">method name to invoke</param>
        /// <param name="typeParameter">input type parameter</param>
        /// <param name="flags">reflection binding flags to use for method lookup</param>
        /// <param name="parameters">input method parameters</param>
        /// <returns>result</returns>
        protected static T ExecuteGenericMethod<T>(object instance, string methodName, Type typeParameter, BindingFlags flags, params object[] parameters) where T : class
        {
            var methodDefinition =
                instance.GetType().GetMethods(flags).Where(m => m.Name.Equals(methodName)).FirstOrDefault();

            var method = methodDefinition.MakeGenericMethod(typeParameter);
            
            var result = method.Invoke(instance, parameters);

            return result != null ? (T)result : null;
        }

        /// <summary>
        /// Places a new expected action on the provided queue using the provided strings and the Current Grid Terminal System
        /// </summary>
        /// <param name="queue">queue to add item too</param>
        /// <param name="name">name of block</param>
        /// <param name="action">name of action</param>
        protected void EnqueueAction(Queue<ExecutedAction> queue, string name, string action = "")
        {
            queue.Enqueue(new ExecutedAction() { Name = name, Action = action, Block = CurrentGts.GetBlockWithName(name) });
        }

        protected virtual void Dispose(bool disposing)
        {
            ClearGrid();
        }

        /// <summary>
        /// Clears the currently stored grid instance
        /// </summary>
        private void ClearGrid()
        {
            currentMockGts = null;

            DisplayOutputs.Clear();
            ExecutionLog.Clear();
            GridBlocks.Clear();
        }
    }
}
