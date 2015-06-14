namespace BSET.Mocks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Sandbox.ModAPI.Ingame;
    using Sandbox.ModAPI.Interfaces;
    using VRage.Collections;

    using Moq;
    using System.Reflection;

    /// <summary>
    /// Main static support methods for the framework. All methods are atomic and only used provided parameters as data for ouput.
    /// </summary>
    public static class SEMock
    {
        /// <summary>
        /// Creates and configures a new grid object
        /// </summary>
        /// <param name="type">type of object to create</param>
        /// <param name="name">name to use</param>
        /// <param name="getActionWithName">optional action to execute for calls to GetActionWithName</param>
        /// <param name="apply">optional action to execute for calls to Apply</param>
        /// <returns>newly created grid object</returns>
        public static Mock CreateGridObject(
            Type type,
            string name = "", 
            Action<ExecutedAction> getActionWithName = null,
            Action<ExecutedAction> apply = null)
        {
            var methodDefinition = typeof(SEMock).GetMethod("CreateGridObject", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.InvokeMethod);
            var method = methodDefinition.MakeGenericMethod(type);
            return (Mock)method.Invoke(null, new object[] { It.IsAny<List<IMyTerminalBlock>>(), It.IsAny<Func<IMyTerminalBlock, bool>>() });
        }

        /// <summary>
        /// Creates and configures a new grid object
        /// </summary>
        /// <typeparam name="T">type of object to create</typeparam>
        /// <param name="name">name to use</param>
        /// <param name="getActionWithName">optional action to execute for calls to GetActionWithName</param>
        /// <param name="apply">optional action to execute for calls to Apply</param>
        /// <returns>newly created grid object</returns>
        public static Mock<T> CreateGridObject<T>(  
            string name = "", 
            Action<ExecutedAction> getActionWithName = null,
            Action<ExecutedAction> apply = null) where T : class
        {
            return ConfigureGridObject<T>(name, getActionWithName, apply);
        }

        /// <summary>
        /// Configures the provided grid object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="getActionWithName"></param>
        /// <param name="apply"></param>
        /// <returns></returns>
        public static Mock<T> ConfigureGridObject<T>(
            string name = "", 
            Action<ExecutedAction> getActionWithName = null, 
            Action<ExecutedAction> apply = null) where T : class
        {
            var mock = new Mock<T>();

            SetName(name, mock);
            SetTerminalAction(mock, name, getActionWithName, apply);

            return mock;
        }

        /// <summary>
        /// Sets the name of a grid object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="moq"></param>
        /// <returns></returns>
        public static Mock<T> SetName<T>(string name, Mock<T> moq) where T : class
        {
            var mock = moq.As<IMyTerminalBlock>();
            mock.Setup(p => p.CustomName).Returns(name);
            mock.Setup(p => p.ToString()).Returns(name);

            return moq;
        }

        /// <summary>
        /// Sets up terminal action parameter processing
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="moq"></param>
        /// <param name="name"></param>
        /// <param name="getActionWithName"></param>
        /// <param name="apply"></param>
        /// <returns></returns>
        public static Mock<T> SetTerminalAction<T>(Mock<T> moq, string name, Action<ExecutedAction> getActionWithName = null, Action<ExecutedAction> apply = null) where T : class
        {
            var mock = moq.As<IMyTerminalBlock>();

            ExecutedAction executedAction = null;

            var mockAction = new Mock<ITerminalAction>();
            mockAction.Setup(a => a.Apply(It.IsAny<IMyCubeBlock>()))
                .Callback<IMyCubeBlock>((block) => 
                {
                    executedAction.Block = block;

                    if (apply != null)
                    {
                        apply(executedAction);
                    }
                });

            mockAction.Setup(a => a.Apply(It.IsAny<IMyCubeBlock>(), It.IsAny<ListReader<TerminalActionParameter>>()))
                .Callback<IMyCubeBlock, ListReader<TerminalActionParameter>>(
                (block, parameters) =>
                {
                    executedAction.Parameters = new List<TerminalActionParameter>();
                    executedAction.Block = block;

                    foreach (var param in parameters)
                    {
                        executedAction.Parameters.Add(param);
                    }

                    if (apply != null)
                    {
                        apply(executedAction);
                    }
                    
                });

            mock.Setup(b => b.GetActionWithName(It.IsAny<string>())).Returns<string>((an) =>
            {
                executedAction = new ExecutedAction() { Name = name, Action = an };

                if (getActionWithName != null)
                {
                    getActionWithName(executedAction);
                }

                return mockAction.Object;
            });

            return moq;
        }
    }
}
