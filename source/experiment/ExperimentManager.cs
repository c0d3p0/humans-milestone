using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class ExperimentManager : Node
{
	public void RestartCharacters()
	{
		experimentCharacterManger.RestartCharacters();
	}

	private void StartExperiment()
	{
		EmitSignal(SignalKey.CONNECT_TO_EXPERIMENT_MANAGER, this);
		EmitSignal(SignalKey.START_EXPERIMENT);
	}

	public void OnExperimentLevelChanged(sbyte experimentLevel)
	{
		experimentCharacterManger.OnExperimentLevelChanged(experimentLevel);
	}

	public void OnClaimAward()
	{
		experimentCharacterManger.OnClaimAward();
	}

	public void OnCheatingConsequences()
	{
		experimentCharacterManger.ActivateExtraMonsters();
	}

	private void Initialize()
	{
		playerCharacter = GetNode<Spatial>(playerCharacterNP);
		monsterCharacterList = new Array<Spatial>();
		secretMonsterCharacterList = new Array<Spatial>();
		monsterStartSpatialList = new Array<Spatial>();
		monsterRespawnSpatialList = new Array<Spatial>();
		playerRespawnSpatialList = new Array<Spatial>();
		worldEnvironment = GetNode<WorldEnvironment>(worldEnvironmentNP);
		experimentCharacterManger = GetNode<ExperimentCharacterManager>(
				experimentCharacterMangerNP);
	}

	private void InitializeSpatialList(Array<Spatial> spatialList, NodePath nodePath)
	{
		Godot.Collections.Array c = GetNode<Spatial>(nodePath).GetChildren();
		System.Collections.IEnumerator it = c.GetEnumerator();

		while(it.MoveNext())
			spatialList.Add((Spatial) it.Current);
	}

	private void InitializeMonsterList(
			Array<Spatial> monsterList, Array<NodePath> monsterNPList)
	{
		SCG.IEnumerator<NodePath> it = monsterNPList.GetEnumerator();

		while(it.MoveNext())
			monsterList.Add(GetNode<Spatial>(it.Current));
	}

	public override void _EnterTree()
	{
		Initialize();
		InitializeSpatialList(monsterStartSpatialList, monsterStartSpotNP);
		InitializeSpatialList(monsterRespawnSpatialList, monsterRespawnSpotNP);
		InitializeSpatialList(playerRespawnSpatialList, playerRespawnSpotNP);
		InitializeMonsterList(monsterCharacterList, monsterCharacterNPList);
		InitializeMonsterList(secretMonsterCharacterList,
				secretMonsterCharacterNPList);
	}

	public override void _Ready()
	{
		StartExperiment();
	}

	public Spatial PlayerCharacter
	{
		get
		{
			return playerCharacter;
		}
	}

	public Array<Spatial> MonsterCharacterList
	{
		get
		{
			return monsterCharacterList;
		}
	}

	public Array<Spatial> SecretMonsterCharacterList
	{
		get
		{
			return secretMonsterCharacterList;
		}
	}

	public Array<Spatial> PlayerRespawnSpatialList
	{
		get
		{
			return playerRespawnSpatialList;
		}
	}

	public Array<Spatial> MonsterStartSpatialList
	{
		get
		{
			return monsterStartSpatialList;
		}
	}

	public Array<Spatial> MonsterRespawnSpatialList
	{
		get
		{
			return monsterRespawnSpatialList;
		}
	}
	
	public WorldEnvironment WorldEnvironment
	{
		get
		{
			return worldEnvironment;
		}
	}
	

	[Export]
	public NodePath playerCharacterNP;

	[Export]
	public Array<NodePath> monsterCharacterNPList;

	[Export]
	public Array<NodePath> secretMonsterCharacterNPList;

	[Export]
	public NodePath playerRespawnSpotNP;

	[Export]
	public NodePath monsterStartSpotNP;

	[Export]
	public NodePath monsterRespawnSpotNP;

	[Export]
	public NodePath worldEnvironmentNP;
	
	[Export]
	public NodePath experimentCharacterMangerNP;


	private Spatial playerCharacter;
	private Array<Spatial> monsterCharacterList;
	private Array<Spatial> secretMonsterCharacterList;
	private Array<Spatial> playerRespawnSpatialList;
	private Array<Spatial> monsterStartSpatialList;
	private Array<Spatial> monsterRespawnSpatialList;
	private WorldEnvironment worldEnvironment;
	private ExperimentCharacterManager experimentCharacterManger;
}
