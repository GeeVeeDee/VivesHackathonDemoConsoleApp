using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using System;
using System.Net;
using System.Reflection;
using VivesHackathonDemoConsoleApp;
using System.Linq;

namespace MyApp // Note: actual namespace depends on the project name.
{
    class Program
    {
        static void printItems(List<string> items, string message)
        {
            Console.WriteLine(message);

            foreach (var item in items)
            {
                Console.WriteLine(item);
            }
        }

        static void Main(string[] args)
        {
            // Computer Vision
            var cv = new ComputerVisionClass("<LOCAL IMAGE FILE HERE>");
            var items = cv.ReadFile().GetAwaiter().GetResult();

            printItems(items, "Extracted items:");

            // Translator
            var tr = new TranslationClass();
            var itemsTranslated = tr.TranslateEachItem(items).GetAwaiter().GetResult();

            printItems(itemsTranslated, "Translated items:");

            // Speech
            var sp = new SpeechClass();
            sp.SynthesisToSpeakerAsync(string.Join(";", itemsTranslated)).GetAwaiter();

            Console.ReadLine();
        }
    }
}