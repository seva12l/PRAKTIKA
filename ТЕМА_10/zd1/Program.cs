using System;

AudioPlayer player1 = AudioPlayer.GetInstance();
player1.Play("song1.mp3");

AudioPlayer player2 = AudioPlayer.GetInstance();
player2.Play("song2.mp3");

Console.WriteLine();
Console.WriteLine("player1 и player2 это один объект: " + (player1 == player2));
Console.WriteLine();

player2.Stop();


class AudioPlayer
{
    private static AudioPlayer instance;
    private string currentTrack;

    private AudioPlayer()
    {
    }

    public static AudioPlayer GetInstance()
    {
        if (instance == null)
            instance = new AudioPlayer();
        return instance;
    }

    public void Play(string track)
    {
        currentTrack = track;
        Console.WriteLine("Воспроизведение: " + track);
    }

    public void Stop()
    {
        if (currentTrack == null)
        {
            Console.WriteLine("Ничего не играет");
            return;
        }
        Console.WriteLine("Остановлено: " + currentTrack);
        currentTrack = null;
    }
}
