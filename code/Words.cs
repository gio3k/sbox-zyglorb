using System;
using System.Collections.Generic;
using Sandbox;

namespace Zyglorb;

public static class Words
{
	private class WordList
	{
		public List<string> Collections { get; }
		public List<string> Objects { get; }
		public List<string> Predicates { get; }
		public List<string> Teams { get; }
		public List<string> All { get; } = new();

		/// <summary>
		/// Words without WASD / input buttons
		/// </summary>
		public List<string> Usable { get; } = new();

		public WordList( List<string> collections, List<string> objects, List<string> predicates, List<string> teams )
		{
			Collections = collections;
			Objects = objects;
			Predicates = predicates;
			Teams = teams;
			foreach ( var w in Collections ) All.Add( w );
			foreach ( var w in Objects ) All.Add( w );
			foreach ( var w in Predicates ) All.Add( w );
			foreach ( var w in Teams ) All.Add( w );
			foreach ( var w in All )
			{
				if ( w.Contains( 'w' ) || w.Contains( 'a' ) || w.Contains( 's' ) || w.Contains( 'd' ) )
					continue;
				Usable.Add( w );
			}
		}
	}

	private static WordList _wordList;
	private static readonly Queue<string> WordCache = new();
	private const int RequeueAmount = 30;

	internal static void Init()
	{
		Game.AssertServer();
		_wordList = FileSystem.Mounted.ReadJson<WordList>( "words/words.json" );
		Cache( RequeueAmount );
	}

	public static void Cache( int amount )
	{
		var count = _wordList.Usable.Count;

		for ( var i = 0; i < amount; i++ )
		{
			var num = Random.Shared.Next( 0, count - 1 );
			WordCache.Enqueue( _wordList.Usable[num] );
		}
	}

	public static string Take()
	{
		if ( WordCache.TryDequeue( out var result ) )
			return result;
		Log.Info( $"Word cache out of words! Adding {RequeueAmount} more" );
		Cache( RequeueAmount );
		if ( WordCache.TryDequeue( out result ) )
			return result;
		throw new Exception( "Word queue failure" );
	}
}
