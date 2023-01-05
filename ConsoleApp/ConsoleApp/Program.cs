using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;

class Program
{
    static string speechKey = "ADD_KEY_HERE";
    static string speechRegion = "ADD_REGION_HERE";

    static async Task SynthesizeAudioAsync()
    {
        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        speechConfig.SpeechSynthesisLanguage = "en-US";
        speechConfig.SpeechSynthesisVoiceName = "en-US-JennyNeural";
        speechConfig.SetProperty(speechKey, speechRegion);
        using var audioConfig = AudioConfig.FromWavFileOutput($"C:/Users/Chandan Y S/Desktop/chandan_git/azure_text_to_speech/{timeStamp}.wav");
        using var synthesizer = new SpeechSynthesizer(speechConfig, audioConfig);
        await synthesizer.SpeakTextAsync("I'm excited to try text-to-speech"); // Add text here.
        Console.WriteLine($"Audio file created successfully. File name - {timeStamp}.wav");
        Console.ReadKey();
    }

    static async Task SynthesizeAudioAsyncFromSSML()
    {
        string workingDirectory = Environment.CurrentDirectory;
        string? ssmlProjectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.FullName;
        string? rootProjectDirectory = Directory.GetParent(workingDirectory)?.Parent?.Parent?.Parent?.Parent?.FullName;
        if (string.IsNullOrWhiteSpace(ssmlProjectDirectory) || string.IsNullOrWhiteSpace(rootProjectDirectory)) throw new Exception("Invalid project directory.");

        string timeStamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
        using var synthesizer = new SpeechSynthesizer(speechConfig, null);
        var ssml = File.ReadAllText($"{ssmlProjectDirectory}/ssml.xml"); // Add text in ssml.xml file.
        var result = await synthesizer.SpeakSsmlAsync(ssml);
        using var stream = AudioDataStream.FromResult(result);
        await stream.SaveToWaveFileAsync($"{rootProjectDirectory}/{timeStamp}.wav");
        Console.WriteLine($"Audio file created successfully. File name - {timeStamp}.wav");
        Console.ReadKey();
    }

    async static Task Main(string[] args)
    {
        try
        {
            await SynthesizeAudioAsyncFromSSML();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR - {ex.Message}");
            Console.ReadKey();
        }
    }
}
