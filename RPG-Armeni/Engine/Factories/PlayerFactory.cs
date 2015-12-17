﻿namespace RPGArmeni.Engine.Factories
{
    using RPGArmeni.Interfaces;
    using System;
    using RPGArmeni.UI;
    using System.Text.RegularExpressions;
    using RPGArmeni.Models.Characters;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using RPGArmeni.Attributes;

    public class PlayerFactory
    {
        private static PlayerFactory instance;
        private static readonly Regex PlayerNamePattern = new Regex("([a-zA-Z]+){2,10}");
        private List<Type> availableRaces = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.CustomAttributes.Any(y => y.AttributeType == typeof(RaceAttribute)))
            .ToList();

        private PlayerFactory()
        {
        }

        public static PlayerFactory Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlayerFactory();
                }

                return instance;
            }
        }

        public IGameEngine Engine { get; set; }

        public IPlayer CreatePlayer()
        {
            string name = this.GetPlayerName();
            IRace playerRace = this.GetPlayerRace();
            IPosition startingPosition = new Position(0, 0);
            char playerSymbol = 'P';
            this.Engine.Map.Matrix[startingPosition.X, startingPosition.Y] = playerSymbol;

            return new Player(startingPosition, playerSymbol, name, playerRace);

        }

        private string GetPlayerName()
        {
            string name;
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleRenderer.Write("Type player's name : ");
            Console.ResetColor();
            ConsoleRenderer.WriteLine("(including only small and capital letters and between 2 and 10 characters)");
            while (true)
            {
                try
                {
                    name = ConsoleInputReader.ReadLine();

                    if (!PlayerNamePattern.IsMatch(name))
                    {
                        throw new ArgumentException("Invalid name. Try again.");
                    }

                    break;
                }
                catch (ArgumentException ex)
                {
                    ConsoleRenderer.WriteLine(ex.Message);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleRenderer.WriteLine("Player name set to: {0}", name);
            Console.ResetColor();

            return name;
        }

        private IRace GetPlayerRace()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleRenderer.WriteLine("Choose a race : ");
            Console.ResetColor();

            for (int i = 0; i < this.availableRaces.Count; i++)
            {
                IRace currentRace = Activator.CreateInstance(this.availableRaces[i]) as IRace;
                Console.WriteLine("{0}: {1} - (Health: {2}, Damage: {3})",
                        i + 1, this.availableRaces[i].Name, currentRace.Health, currentRace.Damage);
            }

            int index;
            while (true)
            {
                try
                {
                    string raceNumber = ConsoleInputReader.ReadLine();

                    if (!int.TryParse(raceNumber, out index))
                    {
                        throw new ArgumentException("Please enter a valid race number.");
                    }

                    index = int.Parse(raceNumber);

                    if (index < 1 || index > this.availableRaces.Count)
                    {
                        throw new ArgumentOutOfRangeException("Please enter a valid race number.");
                    }

                    break;
                }
                
                catch (ArgumentOutOfRangeException ex)
                {
                    ConsoleRenderer.WriteLine(ex.Message);
                }
                catch (ArgumentException ex)
                {
                    ConsoleRenderer.WriteLine(ex.Message);
                }
            }

            Console.ForegroundColor = ConsoleColor.Green;
            ConsoleRenderer.WriteLine("Race chosen: {0}", this.availableRaces[index - 1].Name);
            Console.ResetColor();

            return Activator.CreateInstance(this.availableRaces[index - 1]) as IRace;
        }
    }
}
