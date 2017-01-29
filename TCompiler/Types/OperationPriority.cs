using System;
using System.Collections;

namespace TCompiler.Types
{
    public class OperationPriority : IComparer
    {
        public OperationPriority(string operationSign, int priority, Tuple<bool, bool> leftRightParameterRequired)
        {
            OperationSign = operationSign;
            Priority = priority;
            LeftRightParameterRequired = leftRightParameterRequired;
        }

        public string OperationSign { get; }
        private int Priority { get; }

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