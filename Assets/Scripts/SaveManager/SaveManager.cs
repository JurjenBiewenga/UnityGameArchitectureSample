using System;
using System.Collections.Generic;
using System.Linq;
using SaveManager.IO;
using UnityEngine;

namespace SaveManager
{
    public static class SaveManager
    {
        private static Dictionary<string, object> data = new Dictionary<string, object>();
        private static SaveManagerIO IO;

        public static string saveGameName = "Savegame.json";

        static SaveManager()
        {
            IO = new SaveAsFile();
        }

        public static void SaveData(string saveName = "")
        {
            if (IO != null)
                IO.Save(data, (string.IsNullOrEmpty(saveName)) ? saveGameName : saveName);
        }

        public static void LoadData(string saveName = "")
        {
            if (IO != null)
                data = IO.Load((string.IsNullOrEmpty(saveName)) ? saveGameName : saveName);
            
            if(data.Count == 0)
                data.Add("version", 1);
        }

        /// <summary>
        /// Get a value from the config
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="defaultValue">Value to return if there is no match found</param>
        /// <param name="keys">The keys to get the value from</param>
        /// <returns>Returns the object cast to <paramref name="T"/></returns>
        /// <exception cref="InvalidCastException">When it failed to cast the value to <paramref name="T"/></exception>
        public static T GetValue<T>(T defaultValue, params string[] keys)
        {
            return GetValue<T>(data, 0, defaultValue, keys);
        }

        private static T GetValue<T>(Dictionary<string, object> dict, int currIndex, T defaultValue,
                                     params string[] keys)
        {
            if(data.Count == 0)
                LoadData();
            if (currIndex == keys.Length - 1)
                return GetValue<T>(dict, keys[currIndex], defaultValue);
            else
            {
                if (dict.ContainsKey(keys[currIndex]))
                {
                    if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                    {
                        return GetValue<T>(
                                           (Dictionary<string, object>)dict[keys[currIndex]],
                                           ++currIndex,
                                           defaultValue,
                                           keys);
                    }
                    else
                    {
                        throw new
                            InvalidCastException(string
                                                     .Format("The value of '{0}' is not a dictionary, the type is '{1}'",
                                                             string.Join(".", keys.Take(currIndex + 1).ToArray()),
                                                             dict[keys[currIndex]].GetType().FullName));
                    }
                }
                else
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// Get a value from a given dictionary
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="dict">The dictionary to search in</param>
        /// <param name="key">The key to get the value from</param>
        /// <returns>Returns the object cast to <paramref name="T"/></returns>
        /// <exception cref="InvalidCastException">When it failed to cast the value to <paramref name="T"/></exception>
        private static T GetValue<T>(Dictionary<string, object> dict, string key, T defaultValue)
        {
            if(data.Count == 0)
                LoadData();
            // Check if the key is in the dictionary
            if (dict.ContainsKey(key))
            {
                // If the object is of type T, we can just cast it
                if (dict[key] is T)
                    return (T)dict[key];
                else
                {
                    try
                    {
                        return (T)Convert.ChangeType(dict[key], typeof(T));
                    }
                    catch (InvalidCastException)
                    {
                        throw new InvalidCastException(string.Format("Failed to cast the value of '{0}' to {1}", key,
                                                                     typeof(T)));
                    }
                }
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Check if the config has a certain key chain
        /// </summary>
        /// <param name="keys">Name to look for</param>
        public static bool ContainsKey(params string[] keys)
        {
            return ContainsKey(data, keys, 0);
        }

        private static bool ContainsKey(Dictionary<string, object> dict, string[] keys, int currIndex)
        {
            if(data.Count == 0)
                LoadData();
            if (dict != null)
            {
                if (dict.ContainsKey(keys[currIndex]))
                {
                    if (currIndex == keys.Length - 1)
                    {
                        return true;
                    }
                    else
                    {
                        if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                        {
                            return ContainsKey(
                                               dict[keys[currIndex]] as Dictionary<string, object>,
                                               keys,
                                               ++currIndex);
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Set a value in the config
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="keys">The key chain of which to set the value of</param>
        public static void SetValue(object value, params string[] keys)
        {
            SetValue(value, false, keys);
        }

        /// <summary>
        /// Set a value in the config
        /// </summary>
        /// <param name="value">The value to set</param>
        /// <param name="write">Should the config be written to disk after it successfully set the value</param>
        /// <param name="keys">The key chain of which to set the value of</param>
        public static void SetValue(object value, bool write, params string[] keys)
        {
            SetValue(data, keys, value, 0);
            //Debug.Log("Set the value of '" + string.Join(".", keys) + "' in the config");

            if (write)
                SaveData();
        }

        /// <summary>
        /// Recursively go through the chain of keys and finally set the value
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="keys"></param>
        /// <param name="value"></param>
        /// <param name="currIndex"></param>
        /// <exception cref="InvalidCastException">When the value of one of the keys (not the last one) is not a dictionary</exception>
        private static void SetValue(Dictionary<string, object> dict, string[] keys, object value, int currIndex)
        {
            if(data.Count == 0)
                LoadData();
            if (dict == null)
            {
                Debug.LogError("Failed to set a value in the config");
                return;
            }

            if (currIndex == keys.Length - 1)
            {
                dict[keys[currIndex]] = value;
            }
            else
            {
                if (!dict.ContainsKey(keys[currIndex]))
                    dict[keys[currIndex]] = new Dictionary<string, object>();

                if (dict[keys[currIndex]].GetType() == typeof(Dictionary<string, object>))
                {
                    SetValue(
                             dict[keys[currIndex]] as Dictionary<string, object>,
                             keys,
                             value,
                             ++currIndex);
                }
                else
                {
                    throw new InvalidCastException(string.Format("The value of '{0}' is not a dictionary",
                                                                 string.Join(".", keys.Take(currIndex + 1).ToArray())));
                }
            }
        }
    }
}