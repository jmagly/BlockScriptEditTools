﻿namespace $safeprojectname$
{
    using System;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    $if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
    $endif$using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;
    using Sandbox.ModAPI.Interfaces;

    using Moq;
    //HELLO WORLD
    using Xunit;

    using BSET.Mocks;
    using BSET.Testing;
	
    /// <summary>
    /// Example test using the Hello World control. 
    /// </summary>
    public class ExampleTest : BlockScriptTest
    {
        /// <summary>
        /// When in game a programmable block provides the context of the running script. Set the name of that context by overriding this property if needed.
        /// </summary>
        protected override string ExecutionBlockName
        {
            get
            {
                return "MY CUSTOM EXECUTION NAME";
                //return base.ExecutionBlockName;
            }
        }

        /// <summary>
        /// Pass Hello World Test a single argument and ensure the echo is as expected.
        /// </summary>
        [Fact]
        public void My_Test_1()
        {
            // Get the current GridTerminalSystem instance created on test start by default
            var gridTerminalSystem = CurrentGts;

            // Get the default programmable block that will "run" the script
            var executionBlock = ExecutionBlock;

            // Create the control wrapper for the block script to be tested, be sure to provide the 
            //GridTerminalSystem and Executing programmable block instances
            /*var control = new HelloWorldControl(gridTerminalSystem, executionBlock);

            // Store test data in variables so that it can be used in validation later
            var testArg = "I-Am a Test Argument";

            // Execute the control
            control.MainMethod(testArg);

            // Using Xuint API in this case, validate the results
            // There should only be one echo message
            Assert.True(control.EchoOutput.Count == 1);

            // Ensure the message matches as expected
            Assert.True(control.EchoOutput.First().Equals(string.Format(HelloWorldControl.MessageTemplate, testArg, ExecutionBlockName)));
			*/
        }

        /* OPTIONAL - Useful overrides for customizing testing - see other test classes for examples
        /// <summary>
        /// Override this to modify the test data used to populate the grid prior to it's creation
        /// </summary>
        /// <returns>new grid data instance</returns>
        protected override IBlockGridData AcquireTestData()
        {
            return base.AcquireTestData();
        }

        /// <summary>
        /// Override this to modify the grid during it's creation
        /// </summary>
        /// <returns>new mock grid instance</returns>
        protected override Mock<IMyGridTerminalSystem> CreateGridMock()
        {
            return base.CreateGridMock();
        }

        /// <summary>
        /// Override this to modify the creation for all mock objects
        /// </summary>
        /// <typeparam name="T">type of mock to create</typeparam>
        /// <param name="name">block name for the mock</param>
        /// <param name="getActionWithName">optional action to execute when GetActionWithName is called</param>
        /// <param name="apply">optional action  to execute when Apply is called</param>
        /// <returns>newly created mock object</returns>
        protected override Moq.Mock<T> CreateMock<T>(string name, Action<ExecutedAction> getActionWithName = null, Action<ExecutedAction> apply = null)
        {
            return base.CreateMock<T>(name, getActionWithName, apply);
        }

        /// <summary>
        /// Override this to modify the creation of a new test object (which may or may not be mocked)
        /// </summary>
        /// <typeparam name="T">type of object to create</typeparam>
        /// <param name="name">block name for the object</param>
        /// <param name="getActionWithName">optional action to execute when GetActionWithName is called</param>
        /// <param name="apply">optional action  to execute when Apply is called</param>
        /// <returns>newly created object</returns>
        protected override T Create<T>(string name, Action<ExecutedAction> getActionWithName = null, Action<ExecutedAction> apply = null)
        {
            return base.Create<T>(name, getActionWithName, apply);
        }

        /// <summary>
        /// Override this to modify the special creation of a panel and its tracking values for display text. 
        /// </summary>
        /// <typeparam name="T">type of panel to configure</typeparam>
        /// <param name="name">name of panel block</param>
        /// <param name="mock">mock to configure</param>
        protected override void ConfigurePanel<T>(string name, Mock<T> mock)
        {
            base.ConfigurePanel<T>(name, mock);
        }

        /// <summary>
        /// Override this when using additional resources that need clean up after testing or when taking control of grid persistence between tests
        /// </summary>
        /// <param name="disposing">true=dispose called;false=finalize called</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
         */
    }
}
