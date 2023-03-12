using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;

namespace Zyglorb;

public static class Keyboard
{
	#region Focus Panel

	private class FocusPanel : Panel
	{
		public FocusPanel()
		{
			AcceptsFocus = true;
			InputFocus.Set( this );
		}

		public override void OnButtonEvent( ButtonEvent e ) => KeyChanged?.Invoke( this, e );
	}

	#endregion

	public static event EventHandler<ButtonEvent> KeyChanged;

	internal static void Init()
	{
		Game.AssertClient();

		Game.RootPanel ??= new RootPanel();
		Game.RootPanel.AddChild<FocusPanel>();

		KeyChanged += UpdateGameInputCache;
	}

	#region Game Inputs

	private class GameInput
	{
		public InputButton Button { get; }
		public bool Pressed { get; set; }

		public GameInput( InputButton button ) => Button = button;
	}

	private static readonly List<GameInput> GameInputCache = new()
	{
		new GameInput( InputButton.Forward ),
		new GameInput( InputButton.Back ),
		new GameInput( InputButton.Left ),
		new GameInput( InputButton.Right ),
		new GameInput( InputButton.Run ),
		new GameInput( InputButton.Jump )
	};

	private static string ConvertInputToGame( string key )
	{
		var result = key.ToLowerInvariant();
		if ( result is "lshift" or "rshift" )
			result = "shift";
		if ( result is "lalt" or "ralt" )
			result = "alt"; // not sure
		if ( result is "lcontrol" or "rcontrol" )
			result = "control"; // not sure

		return result;
	}

	private static void UpdateGameInputCache( object sender, ButtonEvent e )
	{
		foreach ( var gameInput in from gameInput in GameInputCache
		         let key = Input.GetButtonOrigin( gameInput.Button )
		         where string.Compare( key, ConvertInputToGame( e.Button ),
			         StringComparison.InvariantCultureIgnoreCase ) == 0
		         select gameInput )
		{
			gameInput.Pressed = e.Pressed;
		}
	}

	public static void UpdateGameInputs()
	{
		foreach ( var gameInput in GameInputCache ) Input.SetButton( gameInput.Button, gameInput.Pressed );

		// Update AnalogMove
		var forward = 0;
		var side = 0;
		if ( Input.Down( InputButton.Forward ) )
			forward += 1;
		if ( Input.Down( InputButton.Back ) )
			forward -= 1;
		if ( Input.Down( InputButton.Left ) )
			side += 1;
		if ( Input.Down( InputButton.Right ) )
			side -= 1;
		Input.AnalogMove = new Vector3( forward, side );
	}

	#endregion
}
