using System.Collections.Generic;

//////////////////////////////////////////////////////////////////
// Script Purpose: This script is used to dynamically track data about the world.
//////////////////////////////////////////////////////////////////

namespace Hellcrown.AI
{
    public class DataStorage
    {
        // Holds all data, that can dynamically change.
        Dictionary<string, object> _data = new Dictionary<string, object>();

        /// <summary>
        /// Adds new data to track.
        /// </summary>
        /// <param name="key">Data name</param>
        /// <param name="val">Data value (can be anything including arrays)</param>
        public void AddData(string key, object val)
        {
            if (!_data.ContainsKey(key))
                _data.Add(key, val);
            else
                _data[key] = val;
        }

        /// <summary>
        /// Returns data, that is being tracked.
        /// </summary>
        /// <param name="key">Data name</param>
        public object GetData(string key)
        {
            if (_data.ContainsKey(key))
                return _data[key];
            return null;
        }
    }
}
