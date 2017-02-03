using System;
using System.Collections;

namespace TCompiler.Types
{
    /// <summary>
    /// The priority of an operation used for the execution order.
    /// </summary>
    public class OperationPriority : IComparer
    {
        /// <summary>
        /// Initializes a new OperationPriority
        /// </summary>
        /// <param name="operationSign">The sign of the operation</param>
        /// <param name="priority">The priority of the operation</param>
        /// <param name="leftRightParameterRequired">Indicates wether the left/right parameters are neccessary for this operation</param>
        public OperationPriority(string operationSign, int priority, Tuple<bool, bool> leftRightParameterRequired)
        {
            OperationSign = operationSign;
            Priority = priority;
            LeftRightParameterRequired = leftRightParameterRequired;
        }

        /// <summary>
        /// The sign of the operation
        /// </summary>
        public string OperationSign { get; }
        /// <summary>
        /// The priority of the operation
        /// </summary>
        private int Priority { get; }

        /// <summary>
        /// Indicates wether the left/right parameters are neccessary for this operation
        /// </summary>
        public Tuple<bool, bool> LeftRightParameterRequired { get; }

        /// <summary>
        /// Actually an item is bigger when it's less important.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(object x, object y)
        {
            if (x.GetType() != y.GetType() || x.GetType() != typeof(OperationPriority))
                throw new ArgumentException();
            var a = (OperationPriority) x;
            var b = (OperationPriority) y;
            return a.Priority > b.Priority ? -1 : (a.Priority < b.Priority ? 1 : 0);
        }
    }
}