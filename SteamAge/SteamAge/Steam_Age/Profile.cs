using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VAPI;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SteamAge
{
    [Serializable]
    public class Profile
    {

        public string PlayerName;

        public string GetDir
        {
            get
            {
                return "Profiles/" + PlayerName + "/";
            }
            set
            {
            }

        }

        public string[] GetSaves()
        {
            List<string> Saves = new List<string>();

            DirectoryInfo DInfo = new DirectoryInfo(GetDir + "Saves/");
            foreach (FileInfo F in DInfo.GetFiles())
            {
                Saves.Add(F.Name);
            }


            return Saves.ToArray();
        }

        public static string[] AvalibleProfiles()
        {
            List<string> Profiles = new List<string>();

            DirectoryInfo DInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + @"/Profiles/");
            if (DInfo.Exists)
            {
                foreach (DirectoryInfo F in DInfo.GetDirectories())
                {
                    Profiles.Add(F.Name);
                }
            }
            else
            {
                DInfo.Create();
            }

            return Profiles.ToArray();
        }

        public static Profile LoadProfile(string ProfileName)
        {
            Profile LoadedProfile = new Profile();

            XmlSerializer Serializer = new XmlSerializer(typeof(Profile));
            LoadedProfile = (Profile)Serializer.Deserialize(new StreamReader(Directory.GetCurrentDirectory()  + @"/Profiles/" + ProfileName + @"/Profile.xml"));

            return LoadedProfile;
        }

        public void SaveProfile()
        {
            Directory.CreateDirectory(this.GetDir);
            XmlSerializer Serializer = new XmlSerializer(typeof(Profile));
            Serializer.Serialize(new StreamWriter(this.GetDir + "Profile.xml"), this);
        }
    }
}
