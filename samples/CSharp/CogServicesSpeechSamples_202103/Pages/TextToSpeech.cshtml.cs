using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

namespace CogServicesSpeechSamples_202103.Pages
{
    public class TextToSpeechModel : PageModel
    {
        [BindProperty]
        public string TextString { get; set; }

        [BindProperty]
        public string LangVoice { get; set; }

        public string Result { get; set; }
        public string AudioUrl { get; set; }


        private const string speechKey = "YOUR_SPEECHSERVICE_KEY";
        private const string speechLocation = "japaneast";
        //private const string speechEndpoint = "https://YOUR_LOCATION.api.cognitive.microsoft.com/";


        // Setting for FileIO
        private readonly IWebHostEnvironment _host;
        public TextToSpeechModel(IWebHostEnvironment host)
        {
            _host = host;
        }


        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechLocation);
            speechConfig.SpeechSynthesisVoiceName = LangVoice;                  //ja-Jp-KeitaNeural
            speechConfig.SpeechSynthesisLanguage = LangVoice.Substring(0,5);    //ja-Jp
            var audioConfig = AudioConfig.FromWavFileOutput(Path.Combine(_host.WebRootPath, "audios\\voice.wav"));

            using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
            var result = await synthesizer.SpeakTextAsync(TextString);
            if (result.Reason == ResultReason.SynthesizingAudioCompleted)
            {
                Result = "Œ‹‰Ê:";
                AudioUrl = "audios/voice.wav";
            }

            return Page();
        }
    }
}
