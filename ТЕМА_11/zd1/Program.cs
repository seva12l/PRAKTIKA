using System;

MediaFactory audioFactory = new AudioFactory();
IMediaFile audio = audioFactory.CreateMedia("song.mp3");
audio.Play();

MediaFactory videoFactory = new VideoFactory();
IMediaFile video = videoFactory.CreateMedia("movie.mp4");
video.Play();

MediaFactory imageFactory = new ImageFactory();
IMediaFile image = imageFactory.CreateMedia("photo.jpg");
image.Play();


interface IMediaFile
{
    void Play();
}

class AudioFile : IMediaFile
{
    private string fileName;

    public AudioFile(string fileName)
    {
        this.fileName = fileName;
    }

    public void Play()
    {
        Console.WriteLine("Воспроизведение аудио: " + fileName);
    }
}

class VideoFile : IMediaFile
{
    private string fileName;

    public VideoFile(string fileName)
    {
        this.fileName = fileName;
    }

    public void Play()
    {
        Console.WriteLine("Воспроизведение видео: " + fileName);
    }
}

class ImageFile : IMediaFile
{
    private string fileName;

    public ImageFile(string fileName)
    {
        this.fileName = fileName;
    }

    public void Play()
    {
        Console.WriteLine("Открытие изображения: " + fileName);
    }
}

abstract class MediaFactory
{
    public abstract IMediaFile CreateMedia(string fileName);
}

class AudioFactory : MediaFactory
{
    public override IMediaFile CreateMedia(string fileName)
    {
        return new AudioFile(fileName);
    }
}

class VideoFactory : MediaFactory
{
    public override IMediaFile CreateMedia(string fileName)
    {
        return new VideoFile(fileName);
    }
}

class ImageFactory : MediaFactory
{
    public override IMediaFile CreateMedia(string fileName)
    {
        return new ImageFile(fileName);
    }
}
