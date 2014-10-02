using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using CommandLine;
using CommandLine.Text;

namespace say
{
    class Options
    {
        [Option('m', "message", Required = false, HelpText = "Text to Speak")]
        public string Message { get; set; }

        [Option('v', "voice", Required = false, HelpText = "TTS voice to use")]
        public string TTSVoice { get; set; }

        [Option('s', "speed", Required = false, HelpText = "Speed for TTS voice")]
        public int TTSSpeed { get; set; }

        [Option('g', "gender", Required = false, HelpText = "Select TTS voice by gender")]
        public string TTSGender { get; set; }

        [Option('v', "volume", Required = false, HelpText = "Volume of TTS voice")]
        public int TTSVolume { get; set; }

        [Option('l', "lsvoices", Required = false, HelpText = "show list of installed TTS voices")]
        public bool ListVoices { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("TTS Say Command Line Utility", "0.1"),
                Copyright = new CopyrightInfo("Michael-John", 2014),
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true
            };
            help.AddPreOptionsLine("all rights reserved");
            help.AddPreOptionsLine("Usage: say -m [text] <other options>");
            help.AddOptions(this);
            return help;
        }
    }

    class Program
    {
        private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static InstalledVoice[] voiceList = synth.GetInstalledVoices().ToArray();

        public static Options options = new Options();
        public static void Main(String[] args)
        {
            #region oldparser
            //CommandLine.Parser.Default.
            //if (CommandLine.Parser.Default.ParseArguments(args, options))
            //{
            //    // consume Options instance properties
            //    if (options.TTSVoice != null)
            //    {
            //        synth.SelectVoice(parseVoice(options.TTSVoice));
            //    }
            //    else
            //    {
            //        Console.Out.WriteLine("Invalid Voice name, case sensitive with _ as space");
            //        synth.SelectVoice(voiceList[0].VoiceInfo.Name);
            //    }
            //}
            #endregion
            if (CommandLine.Parser.Default.ParseArguments(args, options)) 
            {
                if (options.ListVoices)
                {
                    listVoices();
                }
                if (options.TTSGender == "male" || options.TTSGender == "female")
                {
                    VoiceGender genderMale = VoiceGender.Male;
                    VoiceGender genderFemale = VoiceGender.Female;
                    if (options.TTSGender == "female"){
                        synth.SelectVoiceByHints(genderFemale);
                    }
                    else if(options.TTSGender == "male"){
                        synth.SelectVoiceByHints(genderMale);
                    }
                }
                if (options.TTSSpeed <= 10 && options.TTSSpeed >= -10)
                {
                    synth.Rate = options.TTSSpeed;
                }
                if (options.TTSVolume > 0 && options.TTSVolume <= 100)
                {
                    synth.Volume = options.TTSVolume;
                }
                if (options.Message != null)
                {
                    synth.Speak(options.Message);
                    //Console.Out.WriteLine("Unknown Command");
                }
            }
        }

        public static void listVoices()
        {
            int i = 0;
            for (i = 0; i < voiceList.Length; i++)
            {
                Console.Out.WriteLine(voiceList[i].VoiceInfo.Name);
            }
        }

        public static string parseVoice(string input)
        {
            char underscore = '_';
            char space = ' ';
            String properVoiceName = input.Replace(underscore, space);

            int i = 0;
            for (i = 0; i < voiceList.Length; i++)
            {
                if (properVoiceName == voiceList[i].VoiceInfo.Name)
                {
                    Console.Out.WriteLine(properVoiceName);
                    return properVoiceName;
                }
            }
            return null;
        }
    }
}