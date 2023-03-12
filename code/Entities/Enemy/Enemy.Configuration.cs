namespace Zyglorb;

public partial class Enemy
{
	/// <summary> Enemy body girth </summary>
	protected virtual float Girth => 32.0f;

	/// <summary> Enemy body height </summary>
	protected virtual float Height => 72.0f;
	protected virtual float EyeHeight => Height - 6.0f;

	/// <summary> Enemy eye position </summary>
	public Vector3 EyePosition => Position + Vector3.Up * EyeHeight;

	/// <summary> Minimum corner of the pawn AABB relative to the pawn's origin. </summary>
	protected virtual Vector3 Mins => new Vector3( -(Girth * 0.5f), -(Girth * 0.5f), 0 ) * Scale;

	/// <summary> Maximum corner of the pawn AABB relative to the pawn's origin. </summary>
	protected virtual Vector3 Maxs => new Vector3( Girth * 0.5f, Girth * 0.5f, Height ) * Scale;
}
