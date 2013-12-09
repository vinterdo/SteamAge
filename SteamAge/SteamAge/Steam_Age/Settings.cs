using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using VAPI;
using System.Xml.Serialization;

namespace VAPI
{
    [Serializable]
    public class GameSettings 
    {

        public bool DebugMode = true;
        public int ResX = 1024;
        public int ResY = 768;
        public float ParticlesMultipler = 1f;
        public bool FullScreen = false;
        public bool EnableLogging = true;


        public GameSettings()
        {
        }

        public void Load(string Path)
        {
            XmlSerializer Serializer = new XmlSerializer(typeof(GameSettings));
            GameSettings GameSettings;
            try
            {
                GameSettings = (GameSettings)Serializer.Deserialize(new StreamReader(Path));
            }
            catch (Exception e)
            {
                Logger.Write("Deserialization of settings file failed, make sure its proper XML format");
                Logger.Write(e.StackTrace.ToString());
                GameSettings = new GameSettings();
            }

            this.DebugMode = GameSettings.DebugMode;
            this.EnableLogging = GameSettings.EnableLogging;
            this.FullScreen = GameSettings.FullScreen;
            this.ParticlesMultipler = GameSettings.ParticlesMultipler;
            this.ResX = GameSettings.ResX;
            this.ResY = GameSettings.ResY;
        }

        public void Save(string Path)
        {
            
            XmlSerializer Serializer = new XmlSerializer(typeof(GameSettings));
            Serializer.Serialize(new StreamWriter(Path), this);

        }
    }
}
