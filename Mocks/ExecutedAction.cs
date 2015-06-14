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
    
    /// <summary>
    /// Represents an action that has been executed through the Apply pattern
    /// </summary>
    public sealed class ExecutedAction
    {
        /// <summary>
        /// Gets or Sets Name of the block
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Action performed
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Gets or Sets Action parameters
        /// </summary>
        public List<TerminalActionParameter> Parameters { get; set; }

        /// <summary>
        /// Gets or Sets Block instance used during the action
        /// </summary>
        /// <remarks>Warning: depending on when the grid is cleared when manually managing persistence this may be null during Asserts</remarks>
        public IMyCubeBlock Block { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ExecutedAction))
            {
                return false;
            }

            var action = (ExecutedAction)obj;

            return this.Action.Equals(action.Action)
                && this.Name.Equals(action.Name)
                && action.Block.GetType().IsAssignableFrom(this.Block.GetType());
        }

        public override string ToString()
        {
            return Name + " : " + Action;
        }
    }
}