using System.Text;

using Godot;


public class ExperimentResultSystem : Node
{
	public void UpdateExperimentDataPage(ExperimentResultData experimentResultData)
	{
		this.experimentResultData = experimentResultData;

		if(experimentResultData.allPuzzlesSolved)
			UpdateResultDataPage();

		EmitSignal(SignalKey.UPDATE_GUI_STATE, experimentResultData.allPuzzlesSolved);
	}

	public void OnClaimAward()
	{
		UpdateAwardPage(false, new StringBuilder("We hope you enjoy your award."));
		EmitSignal(SignalKey.ON_CLAIM_AWARD, experimentResultData.score);
	}

	private void UpdateResultDataPage()
	{
		string[] baseDataTexts = new string[]{"SubjectID: ",
				"Time: ", "Puzzle Solved: ", "Score: ", "Hits Taken: "};
		object[] data = new object[]{experimentResultData.subjectID,
				GetExperimentTimeText(), experimentResultData.puzzleSolved,
				experimentResultData.score, experimentResultData.hitsTaken};
		StringBuilder sb = new StringBuilder();
		Label label = this.EmitSignal<Label>(this, SignalKey.GET_LABEL,
				SystemGUIID.LBL_EXPERIMENT_RESULT_DATA);

		for(byte i = 0; i < baseDataTexts.Length; i++)
			sb.Append(baseDataTexts[i]).Append(data[i]).Append('\n');

		sb.Remove(sb.Length - 1, 1);
		label.Text = sb.ToString();
	}

	private void UpdateAwardPage(bool buttonActive, StringBuilder labelText)
	{
		Label label = this.EmitSignal<Label>(this, SignalKey.GET_LABEL,
				SystemGUIID.LBL_AWARD_INFO);
		Button button = this.EmitSignal<Button>(this, SignalKey.GET_BUTTON,
				SystemGUIID.BTN_KB_KEY_OK);
		button.Disabled = !buttonActive;
		button.Visible = buttonActive;
		label.Text = labelText.ToString().Trim();
	}

	private string GetExperimentTimeText()
	{
		long allSeconds = experimentResultData.experimentTime / 1000;
		long allMinutes = allSeconds / 60;
		long allHours = allMinutes / 60;

		if(allHours < 24)
		{
			return string.Format("{0:D2}:{1:D2}:{2:D2}.{3:D3}",
					allHours, allMinutes % 60, allSeconds % 60,
					experimentResultData.experimentTime % 1000);
		}
		else
			return "1+ day";
	}


	private ExperimentResultData experimentResultData;
}
