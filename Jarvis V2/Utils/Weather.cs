using System;
using System.Xml;
namespace Jarvis.Utils
{
    public class Weather
    {
        public string Temperature;
        public string Condition;
        public string Humidity;
        public string WindSpeed;
        public string Town;
        public string TFCloud;
        public string TFHigh;
        public string TFLow;


        public void getWeather()
        {
            string query = String.Format(""); //LINK
            XmlDocument wData = new XmlDocument();
            wData.Load(query);

            XmlNamespaceManager manager = new XmlNamespaceManager(wData.NameTable);
            manager.AddNamespace("yweather", "http://xml.weather.yahoo.com/ns/rss/1.0");

            XmlNode channel = wData.SelectSingleNode("rss").SelectSingleNode("channel");
            XmlNodeList nodes = wData.SelectNodes("/rss/channel/item/yweather:forcast", manager);

            Temperature = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["temp"].Value;
            Condition = channel.SelectSingleNode("item").SelectSingleNode("yweather:condition", manager).Attributes["text"].Value;
            Humidity = channel.SelectSingleNode("yweather:atmosphere", manager).Attributes["humidity"].Value;
            WindSpeed = channel.SelectSingleNode("yweather:wind", manager).Attributes["speed"].Value;
            Town = channel.SelectSingleNode("yweather:location", manager).Attributes["city"].Value;
            TFCloud = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["text"].Value;
            TFHigh = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["high"].Value;
            TFLow = channel.SelectSingleNode("item").SelectSingleNode("yweather:forecast", manager).Attributes["low"].Value;

        }
    }
}
