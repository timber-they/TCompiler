using System;
using System.Collections;

namespace TCompiler.Types
{
    public class OperationPriority : IComparer
    {
        public OperationPriority(string operationSign, Type operation, int priority)
        {
            OperationSign = operationSign;
            Operation = operation;
            Priority = priority;
        }

        public string OperationSign { get; }
        public Type Operation { get; }
        public int Priority { get; }

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