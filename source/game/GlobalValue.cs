using Godot;
using Godot.Collections;


public class GlobalValue : Node
{
	public void Get(string key, Godot.Object signalData)
	{
		object value;

		if(globaValueMap.TryGetValue(key, out value))
			signalData.EmitSignal(SignalKey.SET, value);
	}

	public void Put(string key, object value)
	{
		if(globaValueMap.ContainsKey(key))
			globaValueMap[key] = value;
		else
			globaValueMap.Add(key, value);
	}

	public void Remove(string key)
	{
		if(globaValueMap.ContainsKey(key))
			globaValueMap.Remove(key);
	}

	private void Initialize()
	{
		globaValueMap = new Dictionary<string, object>();

		AddUserSignal(SignalKey.GET);
		AddUserSignal(SignalKey.PUT);
		AddUserSignal(SignalKey.REMOVE);

		Connect(SignalKey.GET, this, SignalMethod.GET);
		Connect(SignalKey.PUT, this, SignalMethod.PUT);
		Connect(SignalKey.REMOVE, this, SignalMethod.REMOVE);
	}

	public override void _EnterTree()
	{
		Initialize();
	}


	protected Dictionary<string, object> globaValueMap;
}
