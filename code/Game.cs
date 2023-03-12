using Sandbox;
using System;
using System.Linq;

namespace Zyglorb;

public class ZyglorbGame : GameManager
{
	public ZyglorbGame()
	{
		if ( Game.IsClient )
			Keyboard.Init();

		if ( Game.IsServer )
			Words.Init();
	}

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new Player();
		client.Pawn = pawn;

		if ( All.OfType<SpawnPoint>().MinBy( x => Guid.NewGuid() ) is not { } spawn )
			return;

		var transform = spawn.Transform;
		transform.Position += Vector3.Up * 50.0f;
		pawn.Transform = transform;
	}

	private TimeUntil _timeUntilEnemy = 3;

	[Event.Tick]
	private void SpawnEnemy()
	{
		if ( Game.IsClient ) return;
		if ( All.OfType<EnemySpawner>().MinBy( x => Guid.NewGuid() ) is not { } spawn )
			return;
		if ( _timeUntilEnemy )
		{
			_timeUntilEnemy = 2;
			var enemy = new Enemy();
			var transform = spawn.Transform;
			transform.Position += Vector3.Up * 50.0f;
			enemy.Transform = transform;
		}
	}
}
