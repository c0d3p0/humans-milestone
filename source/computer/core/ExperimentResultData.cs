public class ExperimentResultData : Godot.Object
{
	public void ClearAll()
	{
		subjectID = -1;
		experimentTime = 0;
		puzzleSolved = 0;
		score = 0;
	}

	public void CalculateScore()
	{
		int maximumScoreTime = MINUTE_MILLIS * 50; // 50 minutes
		long zeroScoreTime = maximumScoreTime + (75 * MINUTE_MILLIS); // max + 75 minutes.
		int onePointCostTime = MINUTE_MILLIS - 15000; // Each 45 costs 1 point.
		long resultScore = 100;

		if((experimentTime < zeroScoreTime && experimentTime > 0) && hitsTaken < 10)
		{
			if(experimentTime > maximumScoreTime)
				resultScore -= (experimentTime - maximumScoreTime) / onePointCostTime;

			if(hitsTaken > 0)
			{
				resultScore -= 10 * hitsTaken;
				resultScore = resultScore > 79 ? 79 : resultScore;
			}

			if(resultScore > 100)
				score = 100;
			else if(resultScore < 0)
				score = 0;
			else
				score = (byte) resultScore;
		}
		else
			resultScore = 0;
	}


	public short subjectID;
	public long experimentTime;
	public byte puzzleSolved;
	public ushort hitsTaken;
	public byte score;
	public bool allPuzzlesSolved;


	private const int MINUTE_MILLIS = 60000;
}
