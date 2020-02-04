using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;
using WanzyeeStudio;

namespace SaveManager.IO
{
    public class SaveAsFile : SaveManagerIO
    {
        public SaveAsFile()
        {
             JsonConvert.DefaultSettings = () => new JsonSerializerSettings(){
             	Converters = JsonNetUtility.defaultSettings.Converters,
             	DefaultValueHandling = DefaultValueHandling.Populate
             };
        }

        public void Save(Dictionary<string, object> dictionary, string saveGameName)
        {
            string json = JsonConvert.SerializeObject(dictionary, Formatting.Indented,
                                                      new JsonSerializerSettings
                                                      {
                                                          PreserveReferencesHandling = PreserveReferencesHandling.All,
                                                          TypeNameHandling = TypeNameHandling.All,
                                                          TraceWriter = new UnityTraceWriter(),
                                                      });
            File.WriteAllText(Path.Combine(Application.dataPath, saveGameName), json);

        }

        public Dictionary<string, object> Load(string saveGameName)
        {
            string path = Path.Combine(Application.dataPath, saveGameName);
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(json,
                                                                                 new JsonSerializerSettings
                                                                                 {
                                                                                     PreserveReferencesHandling =
                                                                                         PreserveReferencesHandling.All,
                                                                                     TypeNameHandling =
                                                                                         TypeNameHandling.All,
                                                                                     TraceWriter = new UnityTraceWriter()
                                                                                 });
            }

            return new Dictionary<string, object>();
        }
    }

    public class UnityTraceWriter : ITraceWriter
    {
        public void Trace(TraceLevel level, string message, Exception ex)
        {
            switch (level)
            {
                case TraceLevel.Off:
                    UnityEngine.Debug.Log(message);
                    break;
                case TraceLevel.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                case TraceLevel.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case TraceLevel.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case TraceLevel.Verbose:
                    UnityEngine.Debug.Log(message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("level", level, null);
            }
        }

        public TraceLevel LevelFilter
        {
            get { return TraceLevel.Verbose; }
        }
    }
    
    public class GeneralJsonEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }

        public override void WriteJson(JsonWriter writer, object
                                           value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type
                                            objectType, object existingValue, JsonSerializer serializer)
        {
            return Enum.Parse(objectType, reader.Value.ToString(), true);
        }
    }
}