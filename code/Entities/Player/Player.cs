using System.Collections.Generic;
using System.Linq;
using Sandbox;
using Sandbox.UI;

namespace Zyglorb;

public partial class Player : Actor
{
	private const float MaxAttackAngle = 20.0f;
	private List<Enemy> Targets { get; } = new();
	private Enemy BestTarget { get; set; }

	[ConCmd.Server( "zyglorb_attack" )]
	private static void AttackToServer( string str )
	{
		var c = str[0];
		if ( ConsoleSystem.Caller.Pawn is not Player pawn ) return;
		pawn.BestTarget?.BeAttacked( c );
	}

	protected virtual void HandleKeyboardInput( object sender, ButtonEvent e )
	{
		if ( BestTarget == null )
			return;
		if ( !e.Pressed )
			return;
		if ( !BestTarget.CheckNextCharacter( e.Button[0] ) )
			return;
		// i swear this wasn't always necessary...
		ConsoleSystem.Run( "zyglorb_attack", e.Button );
	}

	public override void ClientSpawn()
	{
		base.ClientSpawn();

		Keyboard.KeyChanged += HandleKeyboardInput;
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		MovementTick();

		CameraFrameSimulate( cl );

		foreach ( var enemy in All.OfType<Enemy>() )
		{
			if ( BestTarget == enemy )
			{
				DebugOverlay.Text( $"*{enemy.Word}*", enemy.Position );

				DebugOverlay.Text(
					$"*{new string( '*', enemy.Index )}{new string( ' ', enemy.Word.Length - enemy.Index )}*",
					enemy.Position, 1, Color.Cyan );
			}
			else
				DebugOverlay.Text( enemy.Word, enemy.Position );
		}
	}

	private void PopulateTargets()
	{
		Targets.Clear();

		foreach ( var enemy in All.OfType<Enemy>() )
		{
			// Get angle to enemy
			var direction = enemy.Position.WithZ( Position.z ) - Position;
			var angle = Vector3.GetAngle( LookInput.WithPitch( 0 ).Forward, direction );
			if ( angle < MaxAttackAngle )
				Targets.Add( enemy );
		}

		BestTarget = Targets.MinBy( v => v.Position.WithZ( Position.z ).Distance( Position ) );
	}

	/// <summary>
	/// Called every tick, clientside and serverside.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		MovementTick();

		PopulateTargets();
	}
}
