public class SignalKey
{
	// SignalData
	public const string SET = "Set";
	//

	// GlobalValue
	public const string GET = "GET";
	public const string PUT = "PUT";
	public const string REMOVE = "DEL";
	//

	// LoadScreen
	public const string SET_SCENE_PATH = "s_s_p";
	//

	// MainComputer
	public const string GET_ROOM_INFORMATION = "g_r_i";
	public const string ON_SOLVING_PUZZLE = "o_s_p";
	public const string ADD_DOOR = "a_d";
	public const string ADD_PUZZLE_COMPUTER = "a_p_c";
	public const string ADD_INFORMATION_COMPUTER = "a_i_c";
	public const string ADD_EXPERIMENT_RESULT_COMPUTER = "a_e_r_c";
	public const string START_EXPERIMENT = "s_e";
	public const string IS_EXPERIMENT_FINISHED = "i_e_f";
	public const string ON_CLAIM_AWARD = "c_a";
	public const string CONNECT_TO_EXPERIMENT_MANAGER = "c_t_e_m";
	public const string ON_CHEATING_ATTEMPT = "o_c_at";
	public const string ON_CHEATING_CONSEQUENCES = "o_c_c";
	//

	// ExperimentDoorSystem
	public const string LOCK_RANDOM_DOORS = "l_r_d";
	public const string SET_DOORS_UNLOCKED = "s_ds_u";
	public const string SET_DOOR_UNLOCKED = "s_d_u";
	//

	// ExperimentComputerSystem
	public const string SET_PUZZLE_COMPUTERS_RANDOM_PUZZLES = "s_p_c_r_p";
	public const string SET_PUZZLE_COMPUTER_PUZZLE = "s_p_c_p";
	public const string SET_ROOM_PUZZLE_COMPUTERS_ACTIVE = "s_r_p_c_a";
	public const string UPDATE_ALL_INFORMATION_COMPUTERS = "u_a_i_c";
	public const string UPDATE_ALL_EXPERIMENT_RESULT_COMPUTERS = "u_a_e_r_c";
	//

	// PuzzleComputer, InformationComputer
	public const string GET_BUTTON_BY_TOUCH = "g_b_b_t";
	public const string GET_LABEL = "g_l";
	//

	// PuzzleComputer, ExperimentResultComputer
	public const string GET_BUTTON = "g_b";
	//

	// PuzzleComputer
	public const string GET_TEXTURE_RECT = "g_t_r";
	public const string SET_SYSTEM_ACTIVE = "s_s_a";
	public const string SET_PUZZLE = "s_p";
	public const string IS_PUZZLE_SOLVED = "i_p_s";
	public const string UPDATE_GUI_STATE = "u_g_s";
	//

	// InformationComputer
	public const string UPDATE_INFORMATION_PAGE = "u_i_p";
	// 

	// ExperimentResultComputer
	public const string UPDATE_EXPERIMENT_DATA_PAGE = "u_e_d_p";
	// 

	// ComputerTouchScreen
	public const string ON_INTERACTION_RECEIVED = "o_i_r";
	//

	// Door
	public const string IS_UNLOCKED = "i_u";
	public const string EXECUTE_HUMAN_COMMAND = "e_h_c";
	public const string EXECUTE_SYSTEM_COMMAND = "e_s_c";
	public const string ON_DOOR_LOCKED = "o_d_l";
	public const string ON_DOOR_UNLOCKED = "o_d_u";
	//

	// PlayerCharacter, MonsterCharacter
	public const string GET_DIRECTION = "g_t";
	public const string SHOULD_WALK = "s_w";
	public const string CANNOT_MOVE = "c_m";
	public const string SET_CHARACTER_MOVE_ENABLED = "s_c_m_e";
	//

	// PlayerCharacter
	public const string SHOULD_INTERACT = "s_i";
	//

	// PlayerMainAction
	public const string UPDATE_ACTIVE = "u_a";
	//

	// MonsterCharacter
	public const string ON_CHARACTER_INACTIVE = "o_c_i";
	public const string ACTIVATE_KILLER_DEVICE = "a_k_l";
	//

	// CharacterLook
	public const string EXECUTE_LOOK = "e_l";
	//

	// MonsterAI
	public const string CREATE_PATH_TO_SEARCH_POINT = "c_p_t_s_p";
	public const string CLEAR_TARGET_AND_ALL_PATHS = "c_t_a_a_p";
	//

	// MonsterMainAction
	public const string LOOK_AT = "l_a";
	//

	// ExperimentManager
	public const string ON_EXPERIMENT_STARTED = "o_e_s";
	public const string RESTART_CHARACTERS = "r_c";
	public const string ON_EXPERIMENT_LEVEL_CHANGED = "o_e_l_c";
	//

	// DeadScreen
	public const string SHOW_DEAD_SCREEN = "s_d_s";
	//

	// Others
	public const string ON_RUNNING = "o_r";
	public const string ON_WALKING = "o_w";
	public const string ON_IDLING = "o_i";
	public const string ON_INTERACTING = "o_it";
	public const string ON_PANICKED = "o_p";
	public const string ON_CHARACTER_ACTIVE = "o_c_a";
	public const string ON_CHARACTER_DEATH = "o_c_d";
	public const string ON_ATTACK = "o_a";
	public const string ON_CAUGHT = "o_c";
	public const string ON_HIT = "o_h";

	public const string HELLO_SUBJECT_1054 = "h_s_1";
	public const string HELLO_SUBJECT_2948 = "h_s_2";
	public const string HELLO_SUBJECT_3682 = "h_s_3";
	public const string HELLO_SUBJECT_4706 = "h_s_4";

	public const string ON_EXPERIMENT_UPDATE = "o_e_u";
	public const string ON_CORRECT_PASSWORD = "o_c_p";
	public const string ON_WRONG_PASSWORD = "o_w_p";
	public const string ON_ALL_PUZZLE_SOLVED = "o_a_p_s";
	public const string HIGH_SCORE = "h_s";
	public const string AVERAGE_SCORE = "a_s";
	public const string LOW_SCORE = "l_s";
	public const string SPEECH_END = "sp_e";
	public const string ANTI_CHEAT_WARNING = "a_c_w";
	public const string CHEATING_CONSEQUENCES = "c_c";

	public const string ON_BUTTON_TOUCHED = "o_b_t";

	public const string ON_DOOR_MOVE = "o_d_m";
	//
}
