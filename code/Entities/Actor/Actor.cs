using System;
using System.Linq;
using Sandbox;

namespace Zyglorb;

public abstract partial class Actor : AnimatedEntity
{
	/// <summary> Player input - move direction (accounting for camera direction) </summary>
	[ClientInput]
	protected Vector3 MoveInput { get; set; }

	/// <summary> Player input - look direction / angle </summary>
	[ClientInput]
	protected Angles LookInput { get; set; }

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
}
