using OpenTK.Audio.OpenAL;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;

class AudioManager
{
    ALDevice device;
    ALContext context;

    public void Init()
    {
        device = ALC.OpenDevice(null);

        if (device == ALDevice.Null)
        {
            Console.WriteLine("Audio device failed.");
            return;
        }

        context = ALC.CreateContext(device, new int[0]);

        if (context == ALContext.Null)
        {
            Console.WriteLine("Audio context failed.");
            return;
        }

        ALC.MakeContextCurrent(context);
    }

    public int LoadOgg(string path)
    {
        using var vorbis = new VorbisReader(path);

        int channels = vorbis.Channels;
        int sampleRate = vorbis.SampleRate;

        List<short> pcmData = new List<short>();

        float[] readBuffer = new float[4096];
        int samplesRead;

        while ((samplesRead = vorbis.ReadSamples(readBuffer, 0, readBuffer.Length)) > 0)
        {
            for (int i = 0; i < samplesRead; i++)
            {
                float sample = Math.Clamp(readBuffer[i], -1f, 1f);
                pcmData.Add((short)(sample * short.MaxValue));
            }
        }

        short[] pcmArray = pcmData.ToArray();

        ALFormat format;

        if (channels == 1)
            format = ALFormat.Mono16;
        else
            format = ALFormat.Stereo16;

        int buffer = AL.GenBuffer();

        AL.BufferData<short>(
    buffer,
    format,
    pcmArray,
    sampleRate
);

        return buffer;
    }

    public int PlaySound(int buffer)
    {
        int source = AL.GenSource();

        AL.Source(source, ALSourcei.Buffer, buffer);
        AL.SourcePlay(source);

        return source;
    }

    public void StopSound(int source)
    {
        AL.SourceStop(source);
        AL.DeleteSource(source);
    }

    public void Cleanup()
    {
        ALC.MakeContextCurrent(ALContext.Null);

        ALC.DestroyContext(context);
        ALC.CloseDevice(device);
    }
}