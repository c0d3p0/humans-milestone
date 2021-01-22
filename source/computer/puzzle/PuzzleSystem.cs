using System;
using Godot;


public class PuzzleSystem : Node
{
	public void ChangePuzzleContentPage(bool next)
	{
		if(next ? puzzle.GoToNextPuzzleContentPage() : 
				puzzle.GoToPreviousPuzzleContentPage())
		{			
			UpdatePuzzleContentPanel(puzzle.GetCurrentPuzzleContentPage());
		}
	}
	
	public void AddPasswordCharacter(byte buttonId)
	{
		if(!puzzle.Solved)
		{
			PuzzleContent pc = puzzle.GetPuzzleInput(buttonId);
			sbyte id = puzzle.AddUserInput(pc);

			if(id > -1)
			{
				Label l =  this.EmitSignal<Label>(this, SignalKey.GET_LABEL, id);
				TextureRect tr = this.EmitSignal<TextureRect>(this,
						SignalKey.GET_TEXTURE_RECT, id);
				UpdateSingleContentControl(pc, tr, l);
			}
		}
	}

	public void TrySolvePuzzle()
	{
		if(!puzzle.Solved)
		{
			puzzle.TrySolvePuzzle();
			UpdatePuzzleContentPanel(puzzle.GetCurrentPuzzleContentPage());
			EmitSignal(SignalKey.ON_SOLVING_PUZZLE, puzzle.Solved);
		}		
	}

	public void RemoveLastPasswordCharacter()
	{
		if(!puzzle.Solved)
		{
			sbyte id = puzzle.RemoveLastUserInput();

			if(id > -1)
				ClearPasswordCharacterPanel(id);
		}
	}

	public void ClearAllPasswordCharacters()
	{
		if(!puzzle.Solved)
		{
			puzzle.ClearAnswer();

			for(sbyte i = (sbyte) SystemGUIID.LBL_PWD_C0;
					i <= SystemGUIID.LBL_PWD_C11; i++)
			{
				ClearPasswordCharacterPanel(i);
			}
		}
	}

	public void ChangeKeyboardPage()
	{
		if(puzzle.GoToNextPuzzleInputPage())
			UpdateKeyboardPage(puzzle.GetCurrentPuzzleInputPage());
	}

	private void UpdateKeyboardPage(PuzzleInputPage puzzleInputPage)
	{
		PuzzleContent pc;
		Button keyButton;

		for(byte i = SystemGUIID.BTN_KB_KEY0; i <= SystemGUIID.BTN_KB_KEY11; i++)
		{
			pc = puzzleInputPage.GetPuzzleInput(i);
			keyButton = this.EmitSignal<Button>(this,
					SignalKey.GET_BUTTON, i);

			if(keyButton != null)
			{
				keyButton.Disabled = pc == null;
				keyButton.Visible = pc != null;

				if(pc != null)
				{
					TextureRect tr = this.EmitSignal<TextureRect>(this,
							SignalKey.GET_TEXTURE_RECT, i);
					UpdateSingleContentControl(pc, tr, null, keyButton);
				}
			}
		}

		keyButton = this.EmitSignal<Button>(this,
					SignalKey.GET_BUTTON, SystemGUIID.BTN_KB_KEY_ALT);
		keyButton.Disabled = puzzle.InputPageAmount < 2;
		keyButton.Visible = puzzle.InputPageAmount > 1;
	}

	private void UpdatePuzzleContentPanel(PuzzleContent puzzleContent)
	{
		TextureRect tr = this.EmitSignal<TextureRect>(this,
				SignalKey.GET_TEXTURE_RECT, SystemGUIID.LBL_PAGE_CONTENT);
		Label l = this.EmitSignal<Label>(this,
				SignalKey.GET_LABEL, SystemGUIID.LBL_PAGE_CONTENT);
		UpdateContentArea(puzzleContent, tr, l);
	}

	private void UpdateSingleContentControl(PuzzleContent puzzleContent,
			TextureRect textureRect = null, Label label = null, Button button = null)
	{
		if(textureRect == null || puzzleContent.texture == null)
		{
			string content = GetPuzzleContentText(puzzleContent);

			if(label != null)
				label.Text = content;
			
			if(button != null)
				button.Text = content;
		}
		else
			textureRect.Texture = puzzleContent.texture;
	}

	private void UpdateContentArea(PuzzleContent puzzleContent,
			TextureRect textureRect = null, Label label = null, Button button = null)
	{
		string content = GetPuzzleContentText(puzzleContent);

		if(label != null)
			label.Text = content;
		
		if(button != null)
			button.Text = content;

		if(textureRect != null)
			textureRect.Texture = puzzleContent.texture;
	}

	private void ClearPasswordCharacterPanel(sbyte id)
	{
		Label l = this.EmitSignal<Label>(this, SignalKey.GET_LABEL, id);
		TextureRect tr = this.EmitSignal<TextureRect>(this,
				SignalKey.GET_TEXTURE_RECT, id);

		if(l != null)
			l.Text = null;

		if(tr != null)
			tr.Texture = null;
	}

	public string GetPuzzleContentText(PuzzleContent puzzleContent)
	{
		if(puzzleContent.text == null && puzzleContent.character != Char.MinValue)
			return puzzleContent.character.ToString().Trim();
		
		return puzzleContent.text;
	}

	// DEBUG: Comment/Uncomment the puzzle new instance line for testing.
	public override void _Ready()
	{
		this.Active = true;
		// Puzzle = new LeadByTheEvidenceV1Puzzle();
	}

	public Puzzle Puzzle
	{
		set
		{
			puzzle = value;

			if(puzzle != null)
			{
				UpdatePuzzleContentPanel(puzzle.GetCurrentPuzzleContentPage());
				UpdateKeyboardPage(puzzle.GetCurrentPuzzleInputPage());
			}
		}
	}

	public bool Solved
	{
		get
		{
			return puzzle.Solved;
		}
	}

	public bool Active
	{
		set
		{
			active = value;
			EmitSignal(SignalKey.UPDATE_GUI_STATE, active);
		}
	}


	private Puzzle puzzle;
	private bool active;
}
