using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;

namespace VivesHackathonDemoConsoleApp
{
    public class ComputerVisionClass
    {
        private readonly string _CVKey = "YOUR COMPUTER VISION KEY";
        private readonly string _CVEndpoint = "YOUR COMPUTER VISION ENDPOINT";
        private readonly string _ImageName;

        private ComputerVisionClient _client;

        public ComputerVisionClass(string file)
        {
            _client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(_CVKey)) { Endpoint = _CVEndpoint };
            _ImageName = file;
        }

        public async Task<List<string>> ReadFile()
        {
            var returnList = new List<string>();

            using var imageData = File.OpenRead(_ImageName);
            var textHeaders = await _client.ReadInStreamAsync(imageData);

            string operationLocation = textHeaders.OperationLocation;
            string operationId = operationLocation.Substring(operationLocation.Length - 36);

            ReadOperationResult results;
            Console.WriteLine($"Extracting text from file {Path.GetFileName(_ImageName)}...");
            do
            {
                results = await _client.GetReadResultAsync(Guid.Parse(operationId));
            }
            while ((results.Status == OperationStatusCodes.Running || results.Status == OperationStatusCodes.NotStarted));

            Console.WriteLine("Text found:");

            foreach (ReadResult page in results.AnalyzeResult.ReadResults)
            {
                foreach (Line line in page.Lines)
                {
                    returnList.Add(line.Text);
                }
            }

            return returnList;
        }
    }
}
