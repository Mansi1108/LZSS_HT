namespace chessAPI.models.player;

public sealed class clsPlayer<TI>
    where TI : struct, IEquatable<TI>
{
    public clsPlayer(TI id, string email)
    {
        this.id = id;
        this.email = email;
    }

    public TI id { get; set; }
    public string email { get; set; }
}