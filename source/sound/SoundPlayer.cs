using SCG = System.Collections.Generic;

using Godot;
using Godot.Collections;


public class SoundPlayer : Node
{
	public void Play(string key)
	{
		AudioStreamPlayer asp;

		if(audioPlayerMap.TryGetValue(key, out asp))
			asp.Play();
	}

	public void PlayIfStopped(string key)
	{
		AudioStreamPlayer asp;

		if(audioPlayerMap.TryGetValue(key, out asp))
		{
			if(!asp.Playing)
				asp.Play();
		}
	}

	public void PlayAndStop(string playKey, string stopKey)
	{
		Stop(stopKey);
		Play(playKey);
	}

	public void Stop(string key)
	{
		AudioStreamPlayer asp;

		if(audioPlayerMap.TryGetValue(key, out asp))
			asp.Stop();
	}

	private void Initialize()
	{
		audioPlayerMap = new Dictionary<string, AudioStreamPlayer>();

		if(audioPlayerNPMap != null)
		{		
			SCG.KeyValuePair<string, NodePath> pair;
			SCG.IEnumerator<SCG.KeyValuePair<string, NodePath>> it =
					audioPlayerNPMap.GetEnumerator();

			while(it.MoveNext())
			{
				pair = it.Current;
				audioPlayerMap.Add(pair.Key, GetNode<AudioStreamPlayer>(pair.Value));
			}
		}
	}

	public override void _EnterTree()
	{
		Initialize();
	}
	

	[Export]
	public Dictionary<string, NodePath> audioPlayerNPMap;


	private Dictionary<string, AudioStreamPlayer> audioPlayerMap;
}
