using Microsoft.CognitiveServices.Speech;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivesHackathonDemoConsoleApp
{
    public class SpeechClass
    {
        private readonly string _SpeechKey = "<YOUR SPEECH KEY>";

        private readonly SpeechConfig _speechConfig;

        public SpeechClass()
        {
            // To support Chinese Characters on Windows platform
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                Console.InputEncoding = System.Text.Encoding.Unicode;
                Console.OutputEncoding = System.Text.Encoding.Unicode;
            }

            var config = SpeechConfig.FromSubscription(_SpeechKey, "westeurope");
            config.SpeechSynthesisVoiceName = "nl-BE-ArnaudNeural";

            _speechConfig = config;
        }

        public async Task SynthesisToSpeakerAsync(string input)
        {
            using var synthesizer = new SpeechSynthesizer(_speechConfig);
            using var result = await synthesizer.SpeakTextAsync(input);

            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine($"Speech synthesized to speaker for text [{input}]");
            }
            else if (result.Reason == ResultReason.Canceled)
            {
                var cancellation = SpeechSynthesisCancellationDetails.FromResult(result);
                Console.WriteLine($"CANCELED: Reason={cancellation.Reason}");

                if (cancellation.Reason == CancellationReason.Error)
                {
                    Console.WriteLine($"CANCELED: ErrorCode={cancellation.ErrorCode}");
                    Console.WriteLine($"CANCELED: ErrorDetails=[{cancellation.ErrorDetails}]");
                    Console.WriteLine($"CANCELED: Did you update the subscription info?");
                }
            }

            // This is to give some time for the speaker to finish playing back the audio
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
