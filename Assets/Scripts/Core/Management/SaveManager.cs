//Made by Galactspace Studios

using System;
using System.IO;
using UnityEngine;
using System.Linq;
using System.Xml.Linq;
using Scriptable.Save;
using Scriptable.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Management
{
    public class SaveManager : MonoBehaviour
    {
        public string SaveFolderPath => $"{Application.persistentDataPath}";

        private XElement _curDoc;

        private string _curSave;
        public string CurrentSave
        {
            get => _curSave;
            set => _curSave = value;
        }

        public string SettingsPath => $"{SaveFolderPath}/Settings.gsave";
        public string CurrentPath => $"{SaveFolderPath}/{CurrentSave}.gsave";

        private bool HasKey(string key) => HasKey(key, _curDoc);
        private bool HasKey(string key, XElement doc) => doc.Elements().Any(x => x.Name == key);

        private bool CurrentExists => File.Exists(CurrentPath);

        [Space]
        [SerializeField] private SettingsSo settingsSo;
        [SerializeField] private SaveCallerSo saveChannel;

        private void Awake()
        {
            SetSlot(0);
        }

        public void SetSlot(int slot)
        {
            SetSave($"Slot_{slot}");
        }

        public void SetSave(string save)
        {
            _curSave = save;
            Load();
        }

        public void Set(string key, object value)
        {
            if (_curDoc.IsNull()) throw new NullReferenceException("Current Doc Is Null");

            if (HasKey(key)) _curDoc.Element(key).Value = value.ToString();
            else _curDoc.Add(new XElement(key, value));

            Save();
        }

        public void Set<T>(string filename, T data) where T : class
        {
            BinaryFormatter bf = new();
            FileStream fs = File.Create($"{SaveFolderPath}/{filename}.gsave");
            var json = JsonUtility.ToJson(data);
            bf.Serialize(fs, json);
            fs.Close();
        }

        public T Get<T>(string key, T defaultValue)
        {
            if (_curDoc.IsNull()) throw new NullReferenceException("Current Doc Is Null");

            if (HasKey(key)) return (T)Convert.ChangeType(_curDoc.Element(key).Value, typeof(T));
            else return defaultValue;
        }

        public T Get<T>(string filename, Func<T> initializer) where T : class
        {
            T data = initializer();

            if (!File.Exists($"{SaveFolderPath}/{filename}.gsave")) return data;

            BinaryFormatter bf = new();
            FileStream fs = File.Open($"{SaveFolderPath}/{filename}.gsave", FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(fs), data);
            fs.Close();

            return data;
        }

        public void Load()
        {
            if (!string.IsNullOrEmpty(_curSave) && CurrentExists)
                _curDoc = XElement.Load(CurrentPath);
            else if (!string.IsNullOrEmpty(_curSave))
                _curDoc = new XElement("Save");
            else
                throw new NullReferenceException("No Current Save Specified");
        }

        public void Save()
        {
            if (_curDoc != null) _curDoc.Save(CurrentPath);
            else throw new NullReferenceException("No Save Loaded");
        }
    }
}