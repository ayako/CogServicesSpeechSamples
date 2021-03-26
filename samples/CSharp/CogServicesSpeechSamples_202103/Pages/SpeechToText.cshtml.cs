using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CogServicesSpeechSamples_202103.Pages
{
    public class SpeechToTextModel : PageModel
    {
        [BindProperty]
        public IFormFile VoiceFile { get; set; }
        public string Result { get; set; }
        public string RecognizedText { get; set; }

        private const string speechKey = "YOUR_SPEECHSERVICE_KEY";
        private const string speechLocation = "japaneast";
        //private const string speechEndpoint = "https://YOUR_LOCATION.api.cognitive.microsoft.com/";

        //public async Task<IActionResult> OnGetAsync()
        //{
        //    return Page();
        //}

        public async Task<IActionResult> OnPostAsync()
        {
            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechLocation);
            speechConfig.SpeechRecognitionLanguage = "ja-JP";

            byte[] readBytes;

            using var audioInputStream = AudioInputStream.CreatePushStream();
            using var reader = new BinaryReader(VoiceFile.OpenReadStream());
            do
            {
                readBytes = reader.ReadBytes(1024);
                audioInputStream.Write(readBytes, readBytes.Length);
            } while (readBytes.Length > 0);

            var audioConfig = AudioConfig.FromStreamInput(audioInputStream);
            using var speechRecognizer = new SpeechRecognizer(speechConfig,audioConfig);
            var result = await speechRecognizer.RecognizeOnceAsync();

            if (result.Reason == ResultReason.RecognizedSpeech)
            {
                Result = "Œ‹‰Ê:";
                RecognizedText = result.Text;
            }

            return Page();
        }
    }
}
