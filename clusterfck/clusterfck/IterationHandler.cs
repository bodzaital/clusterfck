using System;
using System.Collections.Generic;
using System.Text;

namespace clusterfck
{
    /// <summary>
    /// Holds information about an iteration.
    /// </summary>
    public class IterationHandler
    {
        /// <summary>
        /// The number of remaining repeats.
        /// </summary>
        public int RemainingCount { get; set; }

        /// <summary>
        /// The start of the iteration.
        /// </summary>
        public int Head { get; set; }

        /// <summary>
        /// Creates a new IterationHandler.
        /// </summary>
        /// <param name="remainingCount">The current value of dataPtr.</param>
        /// <param name="head">The current value of programCtr</param>
        public IterationHandler(int remainingCount, int head)
        {
            RemainingCount = remainingCount;
            Head = head;
        }
    }
}
