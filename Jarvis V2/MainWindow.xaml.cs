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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Speech;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using Jarvis.Utils;


namespace Jarvis
{

    public partial class MainWindow : Window
    {

  

        //private Timer autoSave = new Timer();
        private System.Windows.Forms.Timer clockTimer = new System.Windows.Forms.Timer();
      //  private System.Windows.Forms.Timer TwitterCheck = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer sleepTimer = new System.Windows.Forms.Timer();
        Commands command = new Commands();
        Twitter t1 = new Twitter();

        Grammar finalGrammar;
        GrammarBuilder build = new GrammarBuilder();


        JarvisData data = new JarvisData();
        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();

        public MainWindow()
        {
            InitializeComponent();

            
            JarvisData.load();

            clockTimer.Tick += clockTimer_Tick;
            clockTimer.Interval = 1000;
            clockTimer.Start();

      //      TwitterCheck.Tick += TwitterCheck_Tick;
      //      TwitterCheck.Interval = 2000;
      //      TwitterCheck.Start();

            sleepTimer.Tick += sleepTimer_Tick;
            sleepTimer.Interval = 300000;
            sleepTimer.Start();




            _recognizer.SetInputToDefaultAudioDevice();



            build.AppendDictation();
            finalGrammar = new Grammar(build);
            _recognizer.LoadGrammar(finalGrammar);
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Commands);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

           
        }

        private void sleepTimer_Tick(object sender, EventArgs e)
        {
            JarvisData.isOff = "true";
            JarvisData.save();
            this.log.Items.Add("Going to sleep");
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            /*if (DateTime.Now.ToString("hh:mm:ss tt") == "06:25:00 AM")
            {
                if (alarm1 == true)
                {
                    //Play Sound turn on stereo etc

                }
            }
            */
            this.Time.Content = DateTime.Now.ToString("hh:mm:ss tt");
      
          
        }

        private void TwitterCheck_Tick(object sender, EventArgs e)
        {

            t1.TwitterLoad(); //Tweet to command
            JarvisData.load();
            if (JarvisData.lastTweet != Twitter.LatestTweet)
            {
                command.processCommand(Twitter.LatestTweet, this);
            }
            JarvisData.lastTweet = Twitter.LatestTweet;
            JarvisData.save();
            this.log.Items.Add(Twitter.LatestTweet + "- Twitter Feed");

            if (log.Items.Count > 50)
            {
                log.Items.Clear();
            }
        }
        void Commands(object sender, SpeechRecognizedEventArgs e) //Voice to command
        {
            if (e.Result.Text.StartsWith("jarvis"))
            {
                JarvisData.isOff = "false";
                JarvisData.save();
            }

            command.processCommand(e.Result.Text, this);
            JarvisData.load();
        }
       
        public void restart()
        {
            System.Windows.Forms.Application.Restart();
            this.Close();
        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            String input = this.Input.Text;
            command.processCommand(input, this);

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
