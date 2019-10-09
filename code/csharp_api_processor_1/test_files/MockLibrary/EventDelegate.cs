using System;

namespace MockLibrary
{
    /// <summary>
    /// A delegate class
    /// </summary>
    /// <param name="sender">sender</param>
    /// <param name="e">e</param>
    /// <returns>a string</returns>
    [Serializable]
    public delegate string AlarmEventHandler(object sender, EventArgs e);

    /// <summary>
    /// A class with event.
    /// Demonstrates the xpaths formed for event and delegate types.
    /// </summary>
    [Serializable]
    public class WakeMeUp
    {
        /// <summary>
        /// AlarmRang has the same signature as AlarmEventHandler.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        /// <returns>a string</returns>
        public string AlarmRang(object sender, EventArgs e)
        {
            return e.ToString();
        }

        /// <summary>
        /// An Obsolete, NonSerialized field
        /// </summary>
        [Obsolete]
        [NonSerialized]
        public int[] field2;

        /// <summary>
        /// An event
        /// </summary>
        public event AlarmEventHandler Alarm;

        /// <summary>
        /// Constructor. Registers the <see cref="MockLibrary.AlarmEventHandler"/> handler to the
        /// <see cref="MockLibrary.WakeMeUp.Alarm"/> event
        /// </summary>
        WakeMeUp()
        {
            Alarm += new AlarmEventHandler(AlarmRang);
        }
    }

}
