using System;

IRobot robot = new BasicRobot();
Console.WriteLine(robot.GetStatus());
Console.WriteLine();

robot = new VoiceControlDecorator(robot);
Console.WriteLine(robot.GetStatus());
Console.WriteLine();

robot = new NavigationDecorator(robot);
Console.WriteLine(robot.GetStatus());
Console.WriteLine();

robot = new SensorDecorator(robot);
Console.WriteLine(robot.GetStatus());


interface IRobot
{
    string GetStatus();
}

class BasicRobot : IRobot
{
    public string GetStatus()
    {
        return "Базовый робот: готов к работе";
    }
}

abstract class RobotDecorator : IRobot
{
    protected IRobot robot;

    public RobotDecorator(IRobot robot)
    {
        this.robot = robot;
    }

    public virtual string GetStatus()
    {
        return robot.GetStatus();
    }
}

class VoiceControlDecorator : RobotDecorator
{
    public VoiceControlDecorator(IRobot robot) : base(robot)
    {
    }

    public override string GetStatus()
    {
        return base.GetStatus() + "\n + голосовое управление";
    }
}

class NavigationDecorator : RobotDecorator
{
    public NavigationDecorator(IRobot robot) : base(robot)
    {
    }

    public override string GetStatus()
    {
        return base.GetStatus() + "\n + улучшенная навигация";
    }
}

class SensorDecorator : RobotDecorator
{
    public SensorDecorator(IRobot robot) : base(robot)
    {
    }

    public override string GetStatus()
    {
        return base.GetStatus() + "\n + дополнительные датчики";
    }
}
