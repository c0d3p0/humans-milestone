using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class SoundPlayer3D : Node
{
	public void Play(string signal)
	{
		AudioStreamPlayer3D asp;

		if(audioPlayerMap.TryGetValue(signal, out asp))
			asp.Play();
	}

	public void PlayIfStopped(string signal)
	{
		AudioStreamPlayer3D asp;

		if(audioPlayerMap.TryGetValue(signal, out asp))
		{
			if(!asp.Playing)
				asp.Play();
		}
	}

	private void Initialize()
	{
		audioPlayerMap = new Dictionary<string, AudioStreamPlayer3D>();

		if(audioPlayerNPMap != null)
		{		
			SCG.KeyValuePair<string, NodePath> pair;
			SCG.IEnumerator<SCG.KeyValuePair<string, NodePath>> it =
					audioPlayerNPMap.GetEnumerator();

			while(it.MoveNext())
			{
				pair = it.Current;
				audioPlayerMap.Add(pair.Key, GetNode<AudioStreamPlayer3D>(pair.Value));
			}
		}
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	
	[Export]
	public Dictionary<string, NodePath> audioPlayerNPMap;


	private Dictionary<string, AudioStreamPlayer3D> audioPlayerMap;
}
