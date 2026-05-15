using System;

Television tv = new Television();
TVRemote remote = new TVRemote();

remote.SetCommand(new TVPowerOnCommand(tv));
remote.PressButton();

remote.SetCommand(new VolumeUpCommand(tv));
remote.PressButton();
remote.PressButton();
remote.PressButton();

remote.SetCommand(new VolumeDownCommand(tv));
remote.PressButton();

remote.SetCommand(new TVPowerOffCommand(tv));
remote.PressButton();


interface ICommand
{
    void Execute();
}

class Television
{
    private bool isOn = false;
    private int volume = 10;

    public void PowerOn()
    {
        isOn = true;
        Console.WriteLine("Телевизор включён");
    }

    public void PowerOff()
    {
        isOn = false;
        Console.WriteLine("Телевизор выключен");
    }

    public void IncreaseVolume()
    {
        if (!isOn)
        {
            Console.WriteLine("Телевизор выключен, громкость не изменить");
            return;
        }
        volume++;
        Console.WriteLine("Громкость увеличена: " + volume);
    }

    public void DecreaseVolume()
    {
        if (!isOn)
        {
            Console.WriteLine("Телевизор выключен, громкость не изменить");
            return;
        }
        volume--;
        Console.WriteLine("Громкость уменьшена: " + volume);
    }
}

class TVPowerOnCommand : ICommand
{
    private Television tv;

    public TVPowerOnCommand(Television tv)
    {
        this.tv = tv;
    }

    public void Execute()
    {
        tv.PowerOn();
    }
}

class TVPowerOffCommand : ICommand
{
    private Television tv;

    public TVPowerOffCommand(Television tv)
    {
        this.tv = tv;
    }

    public void Execute()
    {
        tv.PowerOff();
    }
}

class VolumeUpCommand : ICommand
{
    private Television tv;

    public VolumeUpCommand(Television tv)
    {
        this.tv = tv;
    }

    public void Execute()
    {
        tv.IncreaseVolume();
    }
}

class VolumeDownCommand : ICommand
{
    private Television tv;

    public VolumeDownCommand(Television tv)
    {
        this.tv = tv;
    }

    public void Execute()
    {
        tv.DecreaseVolume();
    }
}

class TVRemote
{
    private ICommand command;

    public void SetCommand(ICommand command)
    {
        this.command = command;
    }

    public void PressButton()
    {
        if (command == null)
        {
            Console.WriteLine("Команда не задана");
            return;
        }
        command.Execute();
    }
}
