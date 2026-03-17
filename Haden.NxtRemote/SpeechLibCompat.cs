using System;
using System.IO;
using System.Media;
using AeonVoice;

namespace SpeechLib
{
    public enum SpeechVoiceSpeakFlags
    {
        SVSFDefault = 0
    }

    public sealed class SpVoice : IDisposable
    {
        private AeonVoiceEngine _engine;
        private bool _engineUnavailable;

        public void Speak(string text)
        {
            Speak(text, SpeechVoiceSpeakFlags.SVSFDefault);
        }

        public void Speak(string text, SpeechVoiceSpeakFlags flags)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            if (_engineUnavailable)
            {
                return;
            }

            try
            {
                _engine ??= new AeonVoiceEngine();

                var result = _engine.SynthesizeToPcm16(text, "Leena");
                if (result.Samples == null || result.Samples.Length == 0)
                {
                    return;
                }

                using var wav = new MemoryStream();
                WritePcm16Wav(wav, result.Samples, result.SampleRate);
                wav.Position = 0;
                using var player = new SoundPlayer(wav);
                player.PlaySync();
            }
            catch
            {
                _engineUnavailable = true;
            }
        }

        private static void WritePcm16Wav(Stream destination, short[] samples, int sampleRate)
        {
            var dataLength = samples.Length * sizeof(short);
            using var writer = new BinaryWriter(destination, System.Text.Encoding.ASCII, leaveOpen: true);
            writer.Write(new[] { 'R', 'I', 'F', 'F' });
            writer.Write(36 + dataLength);
            writer.Write(new[] { 'W', 'A', 'V', 'E' });
            writer.Write(new[] { 'f', 'm', 't', ' ' });
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)1);
            writer.Write(sampleRate);
            writer.Write(sampleRate * sizeof(short));
            writer.Write((short)sizeof(short));
            writer.Write((short)16);
            writer.Write(new[] { 'd', 'a', 't', 'a' });
            writer.Write(dataLength);
            foreach (var sample in samples)
            {
                writer.Write(sample);
            }
        }

        public void Dispose()
        {
            _engine?.Dispose();
            _engine = null;
        }
    }
}
