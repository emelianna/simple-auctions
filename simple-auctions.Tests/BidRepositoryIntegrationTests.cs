using Microsoft.Extensions.Configuration;
using MySqlConnector;
using SimpleAuctions.Repositories;

namespace SimpleAuctions.Tests;

public class BidRepositoryIntegrationTests
{
        private static readonly string TestDb =
        Environment.GetEnvironmentVariable("TEST_DB_CONNECTION")
        ?? throw new InvalidOperationException("Set the TEST_DB_CONNECTION environment variable.");

    private readonly BidRepository repository;

    public BidRepositoryIntegrationTests()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:AuctionDatabase"] = TestDb
            })
            .Build();

        repository = new BidRepository(config);
    }

    private static async Task<long> Sql(string query)
    {
        await using var connection = new MySqlConnection(TestDb);
        await connection.OpenAsync();
        await using var command = new MySqlCommand(query, connection);
        await command.ExecuteNonQueryAsync();
        return command.LastInsertedId;
    }


    [Fact]
    public async Task TM_BID_02_GetByAuction_ReturnsBidsHighestFirst()
    {
        // Arrange
        await Sql("DELETE FROM bids; DELETE FROM auctions; DELETE FROM users;");
        var userId = await Sql("INSERT INTO users (email, password_hash) VALUES ('it@test.test', 'x');");
        var auctionId = await Sql($"INSERT INTO auctions (name, end_time, seller_id) VALUES ('Testspel', '2026-12-31', {userId});");
        await Sql($"INSERT INTO bids (amount, time, buyer_id, auction_id) VALUES (100, NOW(), {userId}, {auctionId}), (300, NOW(), {userId}, {auctionId}), (200, NOW(), {userId}, {auctionId});");

        // Act
        var bids = await repository.GetByAuction((int)auctionId);

        // Assert
        Assert.Equal(3, bids.Count);
        Assert.Equal(300m, bids[0].Amount);
        Assert.Equal(200m, bids[1].Amount);
        Assert.Equal(100m, bids[2].Amount);
    }

    [Fact]
    public async Task TM_BID_03_GetByAuction_NoBids_ReturnsEmptyList()
    {
        // Arrange
        await Sql("DELETE FROM bids; DELETE FROM auctions; DELETE FROM users;");
        var userId = await Sql("INSERT INTO users (email, password_hash) VALUES ('it@test.test', 'x');");
        var auctionId = await Sql($"INSERT INTO auctions (name, end_time, seller_id) VALUES ('Spel utan bud', '2026-12-31', {userId});");

        // Act
        var bids = await repository.GetByAuction((int)auctionId);

        // Assert
        Assert.Empty(bids);
    }
}
