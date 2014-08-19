/**
 * 
 * Fair use to anyone who wants to use it. 
 * 
 * Nick Ivey
 * Nickivey Productions
 * nick123ivey@gmail.com
 * http://nickivey.com
 * 
 * 
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using Jarvis.Utils;
using System.Speech.AudioFormat;
using System.Speech.Synthesis;
using System.Diagnostics; 
using System.Runtime.InteropServices;
using System.Threading;
  

namespace Jarvis.Utils
{
    class Commands
    {
        Thread t;
        Random random = new Random();
        Logger Log = new Logger();
        Weather w = new Weather();
        SpeechSynthesizer tts = new SpeechSynthesizer();
        string[] greetingAutoResponses = { "Yes Sir?", "How can i Help you?", "What's up sir?", "hows it going?" };
        string[] thankYouResponses = { "No problem sir", "It is my pleasure sir" };
        string[] DemandAutoResponses = { "Yes Sir", "OK", "No problem" };
        String response;

      public void processCommand(String c, MainWindow main)
        {

            c = c.ToLower();
            main.log.Items.Add(DateTime.Now + ": " + c);


            if (!c.Contains("xbox")) //Because I have a xbox one (Prevents jarvis from picking up xbox commands)
  
      if (c.Contains("sleep"))
          {
              ExecuteCommand("C:/nircmd.exe monitor off");
          }
      else
      {
          if (JarvisData.isOff != "true")
          {
              t = new Thread(ProcessCommand2);
              t.Start(c);
          }
         
      }
          
          
        }

      public void ProcessCommand2(object c1) {
          string c = c1.ToString();
          if (c.Contains("hello"))
          {
              justSpeak(greetingAutoResponses[random.Next(greetingAutoResponses.Length)]);
          }
         
          else if (c.StartsWith("google"))
          {
              String google = c.Replace("google", "");
              google = google.Replace(" ", "+");
              System.Diagnostics.Process.Start("http://google.com/search?q=" + google);
          }

          else if (c.StartsWith("text"))
          { //TEXT test
              String text = c;
              text = text.Replace("text", " ");
              Logger.SendSMS("", text);


          }
       
         
        
          else if (c.Contains("weather"))
          {
              w.getWeather();
              speak("The current temperature is " + w.Temperature + " degrees. The weather condition today will be " + w.TFCloud + " With a high of " + w.TFHigh + " and a low of " + w.TFLow + " and humidity at " + w.Humidity + ".", c);

          }

          else if (c.Contains("I'm up") || (c.Contains("i'm up")) || (c.Contains("im up")) || (c.Contains("awake")))
          {
              w.getWeather();
              speak("Good morning,it is " + DateTime.Now.ToShortTimeString() + ". today is " + DateTime.Now.ToLongDateString() + ". The current temperature is " + w.Temperature + " degrees. The weather condition today will be " + w.TFCloud + " With a high of " + w.TFHigh + " and a low of " + w.TFLow + " and humidity at " + w.Humidity + ".", c);
          }
          else if (c.Contains("date"))
          {
              speak(DateTime.Today.ToString("dd-MM-yyyy"), c);
          }

         /* else if (c.Contains("volume up") | c.Contains("volume of"))
          {
              ExecuteCommand("C:/nircmd.exe changesysvolume 10000"); //TODO add normal volume based on when im listening to music and when im not INTELLIGENCE class
          }
          else if (c.Contains("volume normal") | c.Contains("vol normal"))
          {
              ExecuteCommand("C:/nircmd.exe setsysvolume 30000"); //TODO add normal volume based on when im listening to music and when im not INTELLIGENCE class
          }
          else if (c.Contains("volume down"))
          {
              ExecuteCommand("C:/nircmd.exe changesysvolume -10000"); //TODO add normal volume based on when im listening to music and when im not INTELLIGENCE class
          }
          */
          else if (c.Contains("right"))
          {
              justSpeak("yes sir");
          }
          else if (c.Contains("thank you"))
          {
              justSpeak(thankYouResponses[random.Next(thankYouResponses.Length)]);
          }
         
          else if (c.Contains("leaving") | c.Contains("be back"))
          {
            
              JarvisData.isOff = "true";
              JarvisData.save();
          }
        
          else if (c.EndsWith("jarvis"))
          {    
                  justSpeak(greetingAutoResponses[random.Next(greetingAutoResponses.Length)]);
          }
          t.Abort();
      }

      private void justSpeak(string text)
      {
          tts.Speak(text);  
      }
      public void speak(String response, String c)
      {
          //LOGGER
          Log.log(c);
           tts.Speak(response);       
          }
      

    

      public static void ExecuteCommand(string Command)
      {
          System.Diagnostics.ProcessStartInfo procStartInfo =
           new System.Diagnostics.ProcessStartInfo("cmd", "/c " + Command);

          // The following commands are needed to redirect the standard output.
          // This means that it will be redirected to the Process.StandardOutput StreamReader.
          procStartInfo.RedirectStandardOutput = true;
          procStartInfo.UseShellExecute = false;
          // Do not create the black window  .
          procStartInfo.CreateNoWindow = true;
          // Now we create a process, assign its ProcessStartInfo and start it
          System.Diagnostics.Process proc = new System.Diagnostics.Process();
          proc.StartInfo = procStartInfo;
          proc.Start();
          proc.Close();
      }

    }
}
