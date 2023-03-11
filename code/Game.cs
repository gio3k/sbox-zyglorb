using Sandbox;
using System;
using System.Linq;

namespace Zyglorb;

public class ZyglorbGame : GameManager
{
	public ZyglorbGame() => Keyboard.Init();

	public override void ClientJoined( IClient client )
	{
		base.ClientJoined( client );

		var pawn = new Pawn();
		client.Pawn = pawn;

		if ( All.OfType<SpawnPoint>().MinBy( x => Guid.NewGuid() ) is not { } spawn )
			return;

		var transform = spawn.Transform;
		transform.Position += Vector3.Up * 50.0f;
		pawn.Transform = transform;
	}
}
