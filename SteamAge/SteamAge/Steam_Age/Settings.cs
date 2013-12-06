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
            GameSettings TmpSettings = (GameSettings)Serializer.Deserialize(new StreamReader(Path));

            this.DebugMode = TmpSettings.DebugMode;
            this.EnableLogging = TmpSettings.EnableLogging;
            this.FullScreen = TmpSettings.FullScreen;
            this.ParticlesMultipler = TmpSettings.ParticlesMultipler;
            this.ResX = TmpSettings.ResX;
            this.ResY = TmpSettings.ResY;
        }

        public void Save(string Path)
        {
            
            XmlSerializer Serializer = new XmlSerializer(typeof(GameSettings));
            Serializer.Serialize(new StreamWriter(Path), this);

        }
    }
}
