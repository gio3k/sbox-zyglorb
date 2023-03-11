using System;
using Sandbox;

namespace Zyglorb;

public partial class Pawn : AnimatedEntity
{
	/// <summary>
	/// Called when the entity is first created 
	/// </summary>
	public override void Spawn()
	{
		base.Spawn();

		SetModel( "models/citizen/citizen.vmdl" );

		SetupPhysicsFromAABB( PhysicsMotionType.Keyframed, Mins, Maxs );

		Tags.Add( "player" );

		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = false;
		EnableShadowCasting = false;
		EnableAllCollisions = true;
		EnableHitboxes = true;
	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	public override void FrameSimulate( IClient cl )
	{
		base.FrameSimulate( cl );

		MovementSimulate( cl );

		CameraFrameSimulate( cl );
	}

	/// <summary>
	/// Called every tick, clientside and serverside.
	/// </summary>
	public override void Simulate( IClient cl )
	{
		base.Simulate( cl );

		MovementSimulate( cl );

		if ( Game.IsServer && Input.Pressed( InputButton.PrimaryAttack ) )
		{
			// The player pressed the primary attack button, let's shoot a watermelon!
			var watermelon = new ModelEntity();
			var direction = LookInput.ToRotation();

			watermelon.SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
			watermelon.Position = EyePosition + direction.Forward * 40;
			watermelon.SetupPhysicsFromModel( PhysicsMotionType.Dynamic );
			watermelon.PhysicsGroup.Velocity = direction.Forward * 1000;
		}
	}
}
