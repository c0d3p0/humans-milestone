using System.Text;

using Godot;
using Godot.Collections;


public class InformationSystem : Node
{
	public void UpdateInformationPage(int amount)
	{
		sbyte id = (sbyte) (roomId + amount);
		id = id < 0 ? (sbyte) 0 : id > 2 ? (sbyte) 2 : id;
		EmitSignal(SignalKey.GET_ROOM_INFORMATION, id, roomInformation);

		if(roomInformation.validInformation)
		{	
			roomId = id;
			roomNameLabel.Text = roomInformation.roomTitle;
			UpdateLabel(doorsStatusLabel, doorStatusInfos,
					roomInformation.unlockedDoors);
			UpdateLabel(puzzlesStatusLabel, puzzleStatusInfos,
					roomInformation.solvedPuzzles);
		}
	}

	private void UpdateLabel(Label label, string[] texts, Array<bool> statusList)
	{
		bool active;
		StringBuilder sb = new StringBuilder();

		for(int i = 0; i < statusList.Count; i++)
		{
			active = statusList[i];
			sb.Append(i + 1).Append(": ").Append(active ? texts[0] : texts[1]);
			sb.Append("\n");
		}

		label.Text = sb.ToString().Trim();
	}

	private void Initialize()
	{
		doorStatusInfos = new string[]{"Unlocked", "Locked"};
		puzzleStatusInfos = new string[]{"Solved", "Unsolved"};
		roomInformation = new RoomInformation();
	}

	private void InitializeLabels()
	{
		roomNameLabel = this.EmitSignal<Label>(this,
				SignalKey.GET_LABEL, SystemGUIID.LBL_ROOM_TITLE);
		doorsStatusLabel = this.EmitSignal<Label>(this,
				SignalKey.GET_LABEL, SystemGUIID.LBL_DOORS_STATUS);
		puzzlesStatusLabel = this.EmitSignal<Label>(this,
				SignalKey.GET_LABEL, SystemGUIID.LBL_PUZZLES_STATUS);
	}

	public override void _EnterTree()
	{
		Initialize();
	}

	public override void _Ready()
	{
		InitializeLabels();
	}


	private sbyte roomId;

	private RoomInformation roomInformation;

	private Label roomNameLabel;
	private Label doorsStatusLabel;
	private Label puzzlesStatusLabel;

	private string[] doorStatusInfos;
	private string[] puzzleStatusInfos;
}
