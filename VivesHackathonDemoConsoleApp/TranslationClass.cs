using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivesHackathonDemoConsoleApp
{
    public class TranslationClass
    {
        private readonly string _TranslationKey = "<YOUR TRANSLATION KEY>";

        public async Task<List<string>> TranslateEachItem(List<string> items)
        {
            var itemsTranslated = new List<string>();

            foreach (var item in items)
            {
                itemsTranslated.Add(await TranslateToDutchAsync(item));
            }

            return itemsTranslated;
        }

        public async Task<string> TranslateToDutchAsync(string input)
        {
            var body = new object[] { new { Text = input } };
            var requestBody = JsonConvert.SerializeObject(body);
            var result = new List<TranslateModel>();

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage())
            {
                // Build the request.
                request.Method = HttpMethod.Post;
                request.RequestUri = new Uri("https://api.cognitive.microsofttranslator.com/translate?api-version=3.0&to=nl");
                request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                request.Headers.Add("Ocp-Apim-Subscription-Key", _TranslationKey);

                // Send the request and get response.
                using (var response = await client.SendAsync(request))
                {
                    // Read response as a string.
                    var model = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<TranslateModel>>(model);
                }
            }

            // Handling nullpointer eerste FirstOrDefault
            var translation = result.FirstOrDefault();

            if (translation == null)
            {
                return null;
            }

            // Handling nullpointer tweede FirstOrDefault
            var output = translation.Translations.FirstOrDefault();

            if (string.IsNullOrEmpty(output.Text))
            {
                return null;
            }

            // Return de juiste vertaling
            return output.Text;
        }
    }
}
