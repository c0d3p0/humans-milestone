using Godot.Collections;


public class RoomInformation : Godot.Object
{
	public RoomInformation()
	{
		unlockedDoors = new Array<bool>();
		solvedPuzzles = new Array<bool>();
	}

	public void ClearAll()
	{
		roomTitle = null;
		unlockedDoors.Clear();
		solvedPuzzles.Clear();
		validInformation = false;
	}


	public string roomTitle;
	public Array<bool> unlockedDoors;
	public Array<bool> solvedPuzzles;
	public bool validInformation;
}
