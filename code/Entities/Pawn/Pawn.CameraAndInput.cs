using Sandbox;

namespace Zyglorb;

public partial class Pawn
{
	/// <summary> Player input - move direction (accounting for camera direction) </summary>
	[ClientInput]
	private Vector3 MoveInput { get; set; }

	/// <summary> Player input - look direction / angle </summary>
	[ClientInput]
	private Angles LookInput { get; set; }

	public override void BuildInput()
	{
		Keyboard.UpdateGameInputs();

		var lookInput = (LookInput + Input.AnalogLook).Normal;

		// Since we're a FPS game, let's clamp the player's pitch (between -89 and 89)
		LookInput = lookInput.WithPitch( lookInput.pitch.Clamp( -89f, 89f ) );

		var direction = Camera.Rotation.Angles().WithPitch( 0 ).ToRotation();

		// Create move input accounting for camera direction
		MoveInput = Input.AnalogMove.x * direction.Forward.Normal +
		            -(Input.AnalogMove.y * direction.Right.Normal);
	}

	/// <summary>
	/// FrameSimulate is called every frame on the client - we want to use it for our camera
	/// </summary>
	protected virtual void CameraFrameSimulate( IClient cl )
	{
		// Set camera position to our eye position
		Camera.Position = EyePosition;

		// Set camera rotation to the angle we're looking at
		Camera.Rotation = LookInput.ToRotation();

		// Set field of view to whatever the user chose in options
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView( Game.Preferences.FieldOfView );

		// Set the first person viewer to this, so it won't render our model
		Camera.FirstPersonViewer = this;
	}
}
