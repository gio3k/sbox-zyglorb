using System;
using System.Linq;
using Sandbox;

namespace Zyglorb;

public abstract partial class Actor : AnimatedEntity
{
	protected virtual void AnimateTick()
	{
		SetAnimParameter( "b_grounded", GroundEntity != null );

		if ( MoveInput.Length != 0 )
		{
			// Rotation
			var idealRotation = Rotation.LookAt( MoveInput );

			Rotation = Rotation.Slerp( Rotation, idealRotation, Time.Delta * 13.0f );
		}

		{
			// Walking
			var forward = Rotation.Forward.Dot( Velocity );
			var side = Rotation.Right.Dot( Velocity );

			var angle = MathF.Atan2( side, forward ).RadianToDegree().NormalizeDegrees();

			SetAnimParameter( "move_direction", angle );
			SetAnimParameter( "move_speed", Velocity.Length );
			SetAnimParameter( "move_groundspeed", Velocity.WithZ( 0 ).Length );
			SetAnimParameter( "move_y", side );
			SetAnimParameter( "move_x", forward );
			SetAnimParameter( "move_z", Velocity.z );
		}

		SetAnimParameter( "holdtype", 0 );
		SetAnimParameter( "aim_body_weight", 0.5f );
	}
}
