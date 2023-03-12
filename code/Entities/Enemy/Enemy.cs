using System;
using System.Linq;
using Sandbox;

namespace Zyglorb;

public partial class Enemy : Actor
{
	[Net] public string Word { get; set; }
	[Net] public int Index { get; set; }

	public Enemy()
	{
		if ( Game.IsServer ) Word = Words.Take();
	}

	protected override void AnimateTick()
	{
		base.AnimateTick();

		if ( Word.Length <= 4 )
			SetAnimParameter( "b_grounded", false );
	}

	protected override void CalculateVelocity()
	{
		var direction = MoveInput.Normal;
		direction *= float.Max( 150, 300 - Word.Length * 30 );
		Velocity = direction.WithZ( Velocity.z );

		// Apply gravity if we aren't grounded
		if ( !IsGrounded )
			Velocity -= new Vector3( 0, 0, Gravity * 0.6f ) * Time.Delta;
	}

	[Event.Tick]
	public void Tick()
	{
		if ( Game.IsClient )
			return;

		// Find the closest pawn
		var pawn = All.OfType<Player>().MinBy( v => v.Position.WithZ( Position.z ).Distance( Position ) );
		if ( pawn == null )
			return;

		// Find their direction
		MoveInput = (pawn.Position.WithZ( Position.z ) - Position).Normal;

		MovementTick();
	}

	public bool CheckNextCharacter( char c )
	{
		if ( Index >= Word.Length )
			return false;
		return Word[Index] == c;
	}

	public void BeAttacked( char c )
	{
		if ( !CheckNextCharacter( c ) ) return;
		Index++;
		if ( Index >= Word.Length )
			Delete();
	}
}
