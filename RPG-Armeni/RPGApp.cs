﻿namespace RPGArmeni
{
    using Engine;
    using Interfaces;
    using UI;

    public class RpgApp
    {
        public static void Main()
        {
            IRenderer renderer = new ConsoleRenderer();
            IInputReader reader = new ConsoleInputReader();

            GameEngine engine = new GameEngine(reader, renderer);

            engine.Run();
        }
    }
}
