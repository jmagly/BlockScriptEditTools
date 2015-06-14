namespace BSET.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Collections;

    using Sandbox.ModAPI;

    /// <summary>
    /// Helper interface for running tests while allowing for code to be easily copied and pasted into SE
    /// </summary>
    public interface IBlockScript : IMyGridProgram
    {
        /// <summary>
        /// Container for any statements output using the Echo(string) command.
        /// </summary>
        Queue<string> EchoOutput { get; }

        /// <summary>
        /// Wrapper for calling void Main(string), used to make copy/paste and transformation easier.
        /// </summary>
        /// <param name="argument">argument data as would be used in-game</param>
        void MainMethod(string argument);
    }
}
