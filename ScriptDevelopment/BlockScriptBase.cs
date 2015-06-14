namespace BSET.ScriptDevelopment
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Text;

    using Sandbox.ModAPI.Ingame;
    using Sandbox.ModAPI.Interfaces;
    using Sandbox.Common;
    using VRage;
    using VRageMath;

    using Domain;

    /// <summary>
    /// Base helper that allows for test fixtures to easily plug in and start/stop scripts in a similar way the game would. 
    /// </summary>
    public abstract class BlockScriptBase : IBlockScript
    {
        /// <summary>
        /// Support for IMyGridProgram
        /// </summary>
        private bool hasMainMethod = true;

        /// <summary>
        /// Storage for echo output queue
        /// </summary>
        private Queue<string> echoOutput = new Queue<string>();

        private DateTime lastExecution;

        /// <summary>
        /// Creates a new instance of BlockScriptBase
        /// </summary>
        /// <remarks>Consumers should wrap and ensure the <see cref="IMyGridTerminalSystem" /> is passed into the script context correctly</remarks>
        /// <param name="gts">GridTerminalSystem instance used for script context</param>
        /// <param name="executingBlock">programmable block that is "running" the block script</param>
        public BlockScriptBase(IMyGridTerminalSystem gts, IMyProgrammableBlock executingBlock)
        {
            GridTerminalSystem = gts;
            Me = executingBlock;

            // Inject default echo action for queuing output
            Echo = new Action<string>((message) => EchoOutput.Enqueue(message));
        }

        /// <summary>
        /// Main method wrapper to allow for compilation in IDE without modifying Main(string) signature in implementers
        /// </summary>
        /// <param name="arg"></param>
        public void Main(string arg) { MainMethod(arg); }

        /// <summary>
        /// Forced impl requirement on inheritors to allow for correct test wire-up
        /// </summary>
        /// <param name="argument"></param>
        public virtual void MainMethod(string argument)
        {
            if (lastExecution != null)
            {
                ElapsedTime = DateTime.UtcNow - lastExecution;
                return;
            }

            lastExecution = DateTime.UtcNow;
            ElapsedTime = new TimeSpan(0);
        }

        /// <summary>
        /// Gets or sets the echo output queue
        /// </summary>
        public virtual Queue<string> EchoOutput
        {
            get { return echoOutput; }
            set { echoOutput = value; }
        }

        #region IMyGridProgram Members
        /// <summary>
        /// Gets or sets the <see cref="IMyGridTerminalSystem" /> instance used by this script
        /// </summary>
        public virtual IMyGridTerminalSystem GridTerminalSystem
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the echo handler used by this script
        /// </summary>
        public virtual Action<string> Echo
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the elapsed time since last execution of the script
        /// </summary>
        public virtual TimeSpan ElapsedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the HasMainMethod flag for SE framework support
        /// </summary>
        public virtual bool HasMainMethod
        {
            get
            {
                return hasMainMethod;
            }
            set
            {
                hasMainMethod = value;
            }
        }

        /// <summary>
        /// Gets or sets the programmable block instance that is "running" the script
        /// </summary>
        public virtual IMyProgrammableBlock Me
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the storage string for the script
        /// </summary>
        public virtual string Storage
        {
            get;
            set;
        }
        #endregion
    }
}
