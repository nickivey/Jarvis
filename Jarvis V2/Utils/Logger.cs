using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Net.Mail;
using SharpVoice; //Google voice not twillo

namespace Jarvis.Utils
{
    class Logger
    {

        public void log(String command)
        {
            try
            {
                XmlDocument xlog = new XmlDocument();
                xlog.Load("Data/logs.xml");
                XmlNode log = xlog.CreateElement("Log");

                XmlNode date = xlog.CreateElement("Date");
                date.InnerText = DateTime.Today.ToString("D");
                log.AppendChild(date);

                XmlNode time = xlog.CreateElement("Time");
                time.InnerText = DateTime.Now.ToShortTimeString();
                log.AppendChild(time);

                XmlNode com = xlog.CreateElement("Command");
                com.InnerText = command;
                log.AppendChild(com);


                xlog.DocumentElement.AppendChild(log);
                xlog.Save("Data/Logs.xml");
            }
            catch (FileNotFoundException e)
            {
                //TODO
            }
            catch (IOException e)
            {
                if (e.Source != null)
                    Console.WriteLine("IOException source: {0}", e.Source);
                throw;
            }
        }
        
    
        public void getLog()
        {
            string query = String.Format("/Data/Log.xml");
            XmlDocument wData = new XmlDocument();
            wData.Load(query);

            XmlNode channel = wData.SelectSingleNode("Log");
            string logData = channel.SelectNodes("Log").ToString();

        }
        

        public static void SendSMS(String topic, String msg)
        {
          Voice v = new Voice("USER", "PASS");
          v.SendSMS("PHONENUMBER", topic+" \n "+msg);

        }



    }
}

