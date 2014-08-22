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

     
      public void ProcessCommand2(object c1)
      {
          string c = c1.ToString();
          if (c.Contains("hello"))
          {
              justSpeak("Yes sir?");
          }


          t.Abort();
      }


      private void justSpeak(string text)
      {
          tts.Speak(text);  
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
