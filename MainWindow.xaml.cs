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
using Jarvis_V2.Utils;
using ActiveUp.Net.Mail;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Runtime.InteropServices;
using Emgu.CV.UI;


namespace Jarvis_V2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private HaarCascade haarCascade;
        Capture capture;
        Image<Bgr, Byte> currentFrame;
        Image<Gray, byte> gray = null;
       

        //private Timer autoSave = new Timer();
        private Timer clockTimer = new Timer();
        private Timer TwitterCheck = new Timer();
        private Timer TwitterCollectorTimer = new Timer();
        private Timer facialRecTimer = new Timer();
        Commands command = new Commands();
        Weather weather = new Weather();
        Twitter t1 = new Twitter();
        Grammar finalGrammar;
        GrammarBuilder build = new GrammarBuilder();
        bool alarm1 = false;


        JarvisData data = new JarvisData();

        SpeechRecognitionEngine _recognizer = new SpeechRecognitionEngine();

        //SpeechSynthesizer JARVIS = new SpeechSynthesizer();
   

        public MainWindow()
        {
            InitializeComponent();

            haarCascade = new HaarCascade(@"haarcascade_frontalface_alt_tree.xml");
            capture = new Capture();
            JarvisData.load();

            clockTimer.Tick += clockTimer_Tick;
            clockTimer.Interval = 1000;
            clockTimer.Start();

            TwitterCheck.Tick += TwitterCheck_Tick;
            TwitterCheck.Interval = 2000;
            TwitterCheck.Start();

            TwitterCollectorTimer.Tick += TwitterCollectorTimer_Tick;
            TwitterCollectorTimer.Interval = 1800000;
            TwitterCollectorTimer.Start();

            facialRecTimer.Tick += facialRecTimer_Tick;
            facialRecTimer.Interval = 1000;
            facialRecTimer.Start();
            //capture.QueryFrame();
            // autoSave.Tick += autoSave_Tick;
            //  autoSave.Start();
            // autoSave.Interval = 10000;

            if (JarvisData.isMiniMic.Contains("true"))
            {
                this.micLabel.Content = "Mic: Mini Mic";
            }
            else
            {
                this.micLabel.Content = "Mic: Kinect";
            }


            _recognizer.SetInputToDefaultAudioDevice();



            build.AppendDictation();
            finalGrammar = new Grammar(build);
            _recognizer.LoadGrammar(finalGrammar);
            _recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Commands);
            _recognizer.RecognizeAsync(RecognizeMode.Multiple);

           
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void clockTimer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.ToString("hh:mm:ss tt") == "06:25:00 AM")
            {
                if (alarm1 == true)
                {
                    //Play Sound turn on stereo etc

                }
            }
            this.Time.Content = DateTime.Now.ToString("hh:mm:ss tt");
            this.lastTweet.Content = "The last tweet I got was " + JarvisData.lastTweet;
            if (JarvisData.isHome == "false")
            {
                this.homeLabel.Content = "Nick is not home";
            }
            if (JarvisData.isMuted == "true")
            {
                this.muted.Content = "I am Muted";
            }
            if (JarvisData.webcam == "false")
            {
                this.webcam.Content = "I am not using the Webcam";
            }
        }

        private void facialRecTimer_Tick(object sender, EventArgs e)
        {
            if (JarvisData.webcam == "false") return;
            
            currentFrame = capture.QueryFrame();

            //Load the Image
            if (currentFrame != null)
            {
                gray = currentFrame.Convert<Gray, Byte>();
                var detectedFaces = haarCascade.Detect(gray);


                foreach (var face in detectedFaces)
                {
                    currentFrame.Draw(face.rect, new Bgr(0, double.MaxValue, 0), 3);
                    if (JarvisData.isHome == "false")
                    {
                        command.processCommand("nick home", this);
                        this.log.Items.Add("Nick Recognized");
                        currentFrame.Save(@"D:\xampp\htdocs\jarvis\imgs\img.jpeg");
                    }

                }
                imageBox.Source = ToBitmapSource(currentFrame);
            }
        }

        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);
        public static BitmapSource ToBitmapSource(IImage image)
        {

            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        private void TwitterCheck_Tick(object sender, EventArgs e)
        {

            /* MailReader rep = new MailReader("imap.gmail.com", 993, true, @"nick123ivey@gmail.com", "piczorocks101"); //Text to Command
              foreach (ActiveUp.Net.Mail.Message email in rep.GetUnreadMails("JarvisCommand"))
              {
                  System.IO.File.WriteAllText(@"C:\Users\nick\Documents\Visual Studio 2012\Projects\Jarvis V2\Jarvis V2\bin\Debug\Data\email.txt", email.BodyText.TextStripped);
                  // Response.Write(string.Format("<p>{0}: {1}</p><p>{2}</p>", email.From, email.Subject, email.BodyHtml.Text));
                  //command.processCommand(email.BodyHtml.Text, this);
              }
              */
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
            command.processCommand(e.Result.Text, this);
            JarvisData.load();


        }
        public void reloadMic()
        {


        }
        public void changeMic()
        {
            log.Items.Add(JarvisData.isMiniMic);
            if (JarvisData.isMiniMic.Contains("true"))
            {
                ExecuteCommand("C:/nircmd.exe setdefaultsounddevice \"Microphone\"");
            }
            else
            {
                ExecuteCommand("C:/nircmd.exe setdefaultsounddevice \"Microphone Array\"");
            }
            System.Windows.Forms.Application.Restart();
            this.Close();
        }
        public void restart()
        {
            System.Windows.Forms.Application.Restart();
            this.Close();
        }
        private void TwitterCollectorTimer_Tick(object sender, EventArgs e)
        {
            TwitterCollector.twitterLoad();
            this.log.Items.Add("Checked twitter ;)");


        }

        private void inputButton_Click(object sender, RoutedEventArgs e)
        {
            String input = this.Input.Text;
            command.processCommand(input, this);

        }

        private void Input_TextChanged(object sender, TextChangedEventArgs e)
        {

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
