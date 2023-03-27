namespace chessAPI.dataAccess.models;

using chessAPI.models.game;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public sealed class clsGameEntityModel
{
    public clsGameEntityModel()
    {
    }

    public clsGameEntityModel(clsNewGame newGame, long id)
    {
        this.whites = newGame.whites;
        this.blacks = newGame.blacks;
        this.turn = newGame.turn;
        this.id = id;
        this.started = DateTimeOffset.Now;
    }

    public ObjectId Id { get; set; }

    [BsonElement("id_game")]
    public long id { get; set; }
    public DateTimeOffset started { get; set; }
    public int whites { get; set; }
    public int blacks { get; set; }
    public bool turn { get; set; }
    public int winner { get; set; }

    public static explicit operator clsGame(clsGameEntityModel x)
    {
        return new clsGame()
        {
            id = x.id,
            started = x.started,
            whites = x.whites,
            blacks = x.blacks,
            turn = x.turn,
            winner = x.winner
        };
    }
}