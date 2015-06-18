namespace $safeprojectname$
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    $if$ ($targetframeworkversion$ >= 3.5)using System.Linq;
    $endif$using System.Text;

    using VRage;
    using VRageMath;

    using Sandbox.ModAPI.Ingame;
    using Sandbox.ModAPI.Interfaces;
    using Sandbox.Common.ObjectBuilders;

    using BSET.ScriptDevelopment;

    /// <summary>
    /// Description of Block Script
    /// </summary>
    public class HelloWorldControl : BlockScriptBase
    {
        // Required constructor wrapper to ensure injection from tests
        public HelloWorldControl(IMyGridTerminalSystem gts, IMyProgrammableBlock executingBlock) : base(gts, executingBlock) { }

        // Required execution wrapper to make copy-paste of code in region require no extra steps
        public override void MainMethod(string argument)
        {
            Main(argument);
        }

        #region Game Code - Copy/Paste Code from this region into Block Script Window in Game

        // Make the message template public in this case to make it easier to validate during testing
        public const string MessageTemplate = "Hello World. My args are {0} and my block name is {1}";

        void Main(string args)
        {
            Echo(string.Format(MessageTemplate, args, base.Me.CustomName));
        }
        #endregion
    }
}