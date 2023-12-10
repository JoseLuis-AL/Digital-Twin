using System;

namespace DigitalTwin
{
    [Serializable]
    public class Data
    {
        /// <summary>
        /// Number of revolutions made by the engine since it started, 
        /// taking the initial position as a reference.
        /// </summary>
        public int Counts;
    }
}