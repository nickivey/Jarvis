using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Timers;
namespace Jarvis.Utils
{
    class JarvisData
    {

        public static String isOff = "false";
        public static String lastTweet;
        public static void save()
        {
            /* XmlTextWriter writer = new XmlTextWriter("Data/data.xml", Encoding.UTF8);

             writer.Formatting = Formatting.Indented;
           
             writer.WriteStartElement("Jarvis");

             writer.WriteStartElement("isOff");
             writer.WriteString(isOff);
             writer.WriteEndElement();

             writer.WriteStartElement("lastTweet");
             writer.WriteString(lastTweet);
             writer.WriteEndElement();

             writer.WriteEndElement();
             writer.Close();
             */
        }

        public static void load()
        {
            /*XmlDocument reader = new XmlDocument();
            reader.Load("Data/data.xml");
            isOff = reader.SelectSingleNode("Jarvis/isOff").InnerText;
            lastTweet = reader.SelectSingleNode("Jarvis/lastTweet").InnerText;
             */
        }
    }
}
