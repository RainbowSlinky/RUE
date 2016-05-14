using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
//using Windows.ApplicationModel;
//using Windows.Globalization;
//using Windows.Media.SpeechRecognition;
//using Windows.Storage;
//using Windows.UI.Core;

namespace SmartMirror.Auxileriers.Speech
{
    class SpeechComponent
    {
        SpeechRecognitionEngine speechRecognizer;                              
        public delegate void CommandsGeneratedHandler(string command, string parameter);

        public delegate void SessionFinishedHandler();

        public event CommandsGeneratedHandler commandsGenerated;
        public event SessionFinishedHandler sessionsExpired;

        /// <summary>
        /// Initialize Speech Recognizer and compile constraints.
        /// </summary>
        /// <param name="recognizerLanguage">Language to use for the speech recognizer</param>
        /// <returns>Awaitable task.</returns>
        public async Task initRecognizer(System.Globalization.CultureInfo culture)
        {
            
            if (speechRecognizer != null)
            {
                // cleanup prior to re-initializing this scenario.
                speechRecognizer.SpeechRecognized -= ContinuousRecognitionSession_ResultGenerated;
                speechRecognizer.RecognizeCompleted-= ContinuousRecognitionSession_Completed;
                this.speechRecognizer.Dispose();
                this.speechRecognizer = null;
            }

            try
            {
                // Initialize the SpeechRecognizer and add the grammar.
                speechRecognizer = new SpeechRecognitionEngine(culture);


                string languageTag = culture.TwoLetterISOLanguageName;
                string fileName = String.Format(System.Environment.CurrentDirectory+ "\\Common\\Speech\\Grammar\\{0}\\Grammar-{0}.xml", languageTag);
                Grammar g = new Grammar(fileName);
                try
                { speechRecognizer.LoadGrammar(g);}
                catch (Exception ex)
                {
System.Diagnostics.Debug.WriteLine("Unable to compile grammar, " +ex.Message);
                }
                
                {
                    speechRecognizer.SpeechRecognized += ContinuousRecognitionSession_ResultGenerated;
                    speechRecognizer.RecognizeCompleted += ContinuousRecognitionSession_Completed;
                }
                speechRecognizer.SetInputToDefaultAudioDevice();

                speechRecognizer.RecognizeAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("EXCEPTION: " + ex.Message);
            }
        }
        

        private void ContinuousRecognitionSession_ResultGenerated(object sender, SpeechRecognizedEventArgs args)
        {
            if (args.Result.Semantics != null)
            {
                string command = "", param = "";
                if (args.Result.Semantics.ContainsKey("command"))
                { command = args.Result.Semantics["command"].Value.ToString(); }
                if (args.Result.Semantics.ContainsKey("param"))
                { param = args.Result.Semantics["param"].Value.ToString(); }

                commandsGenerated(command, param);
            }
            //commandsGenerated(extractCommands(args.Result.Text));
        }
        //endTBD
        private void ContinuousRecognitionSession_Completed(object sender, RecognizeCompletedEventArgs args)
        {
            sessionsExpired();
        }

        public async void startSession()
        {
            await initRecognizer(System.Globalization.CultureInfo.CurrentCulture);
            //// The recognizer can only start listening in a continuous fashion if the recognizer is currently idle.
            //// This prevents an exception from occurring.
            //if (speechRecognizer.State == SpeechRecognizerState.Idle)
            //{
            //    // Reset the text to prompt the user.
            //    try
            //    {
            //        speechRecognizer.RecognizeAsync();
            //    }
            //    catch (Exception ex)
            //    {
            //        var messageDialog = new Windows.UI.Popups.MessageDialog(ex.Message, "StartAsync Exception");
            //        await messageDialog.ShowAsync();
            //    }
            //}
            //else
            //{
            //    throw new IllegaleStateException("not idle");
            //}
        }

        public async void endSession()
        {
            speechRecognizer.RecognizeAsyncCancel();
        }
    }
}
