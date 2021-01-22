public interface Puzzle
{
	bool GoToNextPuzzleContentPage();
	bool GoToPreviousPuzzleContentPage();
	bool GoToNextPuzzleInputPage();
	PuzzleContent GetPuzzleInput(byte id);  

	PuzzleContent GetCurrentPuzzleContentPage();  
	PuzzleInputPage GetCurrentPuzzleInputPage();

	sbyte AddUserInput(PuzzleContent puzzleInput);
	sbyte RemoveLastUserInput();
	void ClearAnswer();
	void TrySolvePuzzle();

	bool Solved {get;}
	int ContentPageAmount {get;}
	int InputPageAmount {get;}
}
