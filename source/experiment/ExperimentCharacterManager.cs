using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;



public class ExperimentCharacterManager : Node
{
	public void RestartCharacters()
	{
		Spatial s = this.GetRandomItem<Spatial>(playerRespawnSpatialList, rng);
		playerCharacter.EmitSignal(SignalKey.UPDATE_ACTIVE, s.Translation);
		Array<Spatial> usedSpotList = new Array<Spatial>();
		SCG.IEnumerator<Spatial> it = monsterRespawnSpatialList.GetEnumerator();
		// Vector3 translation;
		Spatial monster;
		
		while(it.MoveNext())
			usedSpotList.Add(it.Current);

		for(byte i = 0; i < mainMonsterAmount; i++)
		{
			monster = monsterCharacterList[i];
			s = this.GetRandomItem<Spatial>(usedSpotList, rng);
			// translation = s.Translation;
			// translation.y = monster.GlobalTransform.origin.y;
			monster.Translation = s.Translation;
			usedSpotList.Remove(s);
			monster.EmitSignal(SignalKey.UPDATE_ACTIVE, true);
		}
	}

	public void ActivateExtraMonsters()
	{
		mainMonsterAmount = (byte) monsterCharacterList.Count;
		Spatial spot = monsterStartSpatialList[2]; // Best spot to spawn the extra monsters.
		Vector3 t = IsPlayerDistant(spot) ? spot.Translation :
				monsterRespawnSpatialList[5].Translation;
		Spatial m;

		for(byte i = 1; i < mainMonsterAmount; i++)
		{
			m = monsterCharacterList[i];
			m.Translation = t + new Vector3((i - 1) * 2.0f, 0f, 0f);
			m.EmitSignal(SignalKey.UPDATE_ACTIVE, true);
		}
	}

	public void OnExperimentLevelChanged(sbyte experimentLevel)
	{
		if(experimentLevel == 0)
		{
			monsterCharacterList[0].EmitSignal(SignalKey.UPDATE_ACTIVE,
					true, new Vector3(-3.5f, 0f, 3f));
		}
		else if(experimentLevel > 11)
		{
			for(byte i = 0; i < mainMonsterAmount; i++)
				monsterCharacterList[i].EmitSignal(SignalKey.ACTIVATE_KILLER_DEVICE);
		}
	}

	public void OnClaimAward()
	{
		SCG.IEnumerator<Spatial> it  = secretMonsterCharacterList.GetEnumerator();

		while(it.MoveNext())
			it.Current.EmitSignal(SignalKey.UPDATE_ACTIVE, true);
	}

	private void StartCharacters()
	{
		playerCharacter.EmitSignal(SignalKey.UPDATE_ACTIVE);
		Spatial sp = this.GetRandomItem<Spatial>(monsterStartSpatialList, rng);
		monsterCharacterList[0].Translation = sp.Translation;
	}

	private bool IsPlayerDistant(Spatial spot)
	{
		return spot.GlobalTransform.origin.DistanceTo(
				playerCharacter.GlobalTransform.origin) > 25;
	}

	private void Initialize()
	{
		rng = new RandomNumberGenerator();
		mainMonsterAmount = 1;
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		StartCharacters();
	}

	public Spatial PlayerCharacter
	{
		set
		{
			playerCharacter = value;
		}
	}

	public Array<Spatial> MonsterCharacterList
	{
		set
		{
			monsterCharacterList = value;
		}
	}

	public Array<Spatial> PlayerRespawnSpatialList
	{
		set
		{
			playerRespawnSpatialList = value;
		}
	}

	public Array<Spatial> MonsterStartSpatialList
	{
		set
		{
			monsterStartSpatialList = value;
		}
	}

	public Array<Spatial> MonsterRespawnSpatialList
	{
		set
		{
			monsterRespawnSpatialList = value;
		}
	}

	public Array<Spatial> SecretMonsterCharacterList
	{
		set
		{
			secretMonsterCharacterList = value;
		}
	}


	private Spatial playerCharacter;
	private Array<Spatial> monsterCharacterList;
	private Array<Spatial> secretMonsterCharacterList;
	private Array<Spatial> monsterStartSpatialList;
	private Array<Spatial> playerRespawnSpatialList;
	private Array<Spatial> monsterRespawnSpatialList;


	private RandomNumberGenerator rng;
	private byte mainMonsterAmount;
}
