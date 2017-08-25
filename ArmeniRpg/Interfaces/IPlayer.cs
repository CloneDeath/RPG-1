﻿namespace ArmeniRpg.Interfaces
{
	public interface IPlayer : ICharacter, IMoveable, ICollect
	{
		string Name { get; }
		IRace Race { get; }
		int DefensiveBonus { get; }
		IGameEngine Engine { get; set; }
		void SelfHeal(IGameEngine engine);
		void DrinkHealthBonusPotion(IGameEngine engine);
	}
}