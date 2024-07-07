namespace Goodtocode.SemanticKernel.Core.Domain.ChatCompletion.Enums;

public struct OpenAiModels
{
    public struct ChatComplation
    {
        public const string ChatGpt4 = "gpt-4";
        public const string ChatGpt4Turbo = "gpt-4-turbo";
        public const string ChatGpt35Turbo = "gpt-3.5-turbo";
    }
    public struct TextGeneration
    {
        public const string ChatGpt35TurboInstruct = "gpt-3.5-turbo-instruct";
    }
    public struct TextEmbedding
    {
        public const string TextEmbedding3Large = "text-embedding-3-large";
        public const string TextEmbedding3Small = "text-embedding-3-small";
    }
    public struct TextModeration
    {
        public const string TextModerationLatest = "text-moderation-latest";
    }
    public struct Image
    {
        public const string Dalle3 = "dall-e-3";
        public const string Dalle2 = "dall-e-2";
    }
    public struct Audio
    {
        public const string TextToSpeech1 = "tts-1";
        public const string Whisper1 = "whisper-1";
        public const string Whisper2 = "whisper-2";
    }
}
