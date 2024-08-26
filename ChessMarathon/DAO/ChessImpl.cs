using ChessMarathon.Models;
using Npgsql;
using System.Data;
using System.Numerics;

namespace ChessMarathon.DAO
{
    public class ChessImpl : IChess
    {
        NpgsqlConnection _conn;

        public ChessImpl(NpgsqlConnection conn)
        {
            this._conn = conn;
        }
        public async Task<Matches> GetMatchesById(int id)
        {
            Matches match = new Matches();
            string errorMsg= string.Empty;
            string query = @"select * from chess.matches where match_id=@mid";
            try
            {
                using (_conn)
                {
                    await _conn.OpenAsync();
                    NpgsqlCommand selectCommand= new NpgsqlCommand(query, _conn);
                    selectCommand.CommandType = CommandType.Text;
                    selectCommand.Parameters.AddWithValue("mid", id);
                    NpgsqlDataReader reader = await selectCommand.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            match.MatchId = reader.GetInt32(0);
                            match.Player1Id = reader.GetInt32(1);
                            match.Player2Id = reader.GetInt32(2);
                            match.MatchDate = reader.GetDateTime(3);
                            match.MatchLevel = reader.GetString(4);
                            match.WinnerId = reader.GetInt32(5);
                        }
                    }

                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return match;

        }

        //problem 1
        public async Task<int> InsertMatches(Matches m)
        {
            int rowInserted = 0;
            string insertQuery = $@"insert into chess.matches(match_id, player1_id, player2_id, match_date, match_level, winner_id) values({m.MatchId},{m.Player1Id},{m.Player2Id},'{m.MatchDate}','{m.MatchLevel}',{m.WinnerId})";
            try
            {
                using (_conn)
                {
                    await _conn.OpenAsync();
                    NpgsqlCommand insertCommand = new NpgsqlCommand(insertQuery, _conn);
                    insertCommand.CommandType = CommandType.Text;

                    rowInserted = await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine("Exception" + ex.Message);
            }


            return rowInserted;
        }

        //problem 2
        public async Task<List<Players>> GetPlayersByCountry(string country)
        {
            List<Players?> playerList = new List<Players>();
            string query = @"select player_id,concat(first_name,' ',last_name), country, current_world_ranking, total_matches_played  from chess.players where country=@c order by current_world_ranking";

            try
            {
                using (_conn)
                {
                    await _conn.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _conn);
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@c", country);
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Players player = new Players();
                            player.PlayerId = reader.GetInt32(0);
                            player.PlayerName = reader.GetString(1);
                            player.Country = reader.GetString(2);
                            player.CurrentWorldRanking = reader.GetInt32(3);
                            player.TotalMatchesPlayed = reader.GetInt32(4);
                            playerList.Add(player);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }


            return playerList;
        }

        public async Task<List<PlayerPerformance>> GetPlayerPerformance()
        {
            List<PlayerPerformance?> playerPerformancesList = new List<PlayerPerformance>();
            string query = @"select concat(p.first_name,' ',p.last_name) as Name,p.total_matches_played, count(m.winner_id) as TotalWinCount,
	ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS win_percentage
	from chess.players p
join chess.matches m on p.player_id=m.winner_id
group by Name,p.total_matches_played
order by Name";
            try
            {
                using (_conn)
                {
                    await _conn.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _conn);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PlayerPerformance playerPerformance = new PlayerPerformance();
                            playerPerformance.Name = reader.GetString(0);
                            playerPerformance.TotalMatchesPlayed = reader.GetInt32(1);  
                            playerPerformance.TotalWinCount = reader.GetInt32(2);
                            playerPerformance.WinPercentage = reader.GetDouble(3);
                            playerPerformancesList.Add(playerPerformance);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return playerPerformancesList;
        }

        public async Task<List<PlayerPerformance>> GetPlayerWithHighestWon()
        {
            List<PlayerPerformance?> playerPerformancesList = new List<PlayerPerformance>();
            string query = @"select concat(p.first_name,' ',p.last_name) as Name,p.total_matches_played, count(m.winner_id) as TotalWinCount,
	ROUND(COUNT(m.winner_id) * 100.0 / p.total_matches_played, 2) AS win_percentage
	from chess.players p
join chess.matches m on p.player_id=m.winner_id
group by Name,p.total_matches_played
having count(m.winner_id) > (select avg(win_count) from(
select count(winner_id) as win_count from chess.Matches group by winner_id) as win_counts )
order by Name";
            try
            {
                using (_conn)
                {
                    await _conn.OpenAsync();
                    NpgsqlCommand command = new NpgsqlCommand(query, _conn);
                    command.CommandType = CommandType.Text;
                    NpgsqlDataReader reader = await command.ExecuteReaderAsync();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PlayerPerformance playerPerformance = new PlayerPerformance();
                            playerPerformance.Name = reader.GetString(0);
                            playerPerformance.TotalMatchesPlayed = reader.GetInt32(1);
                            playerPerformance.TotalWinCount = reader.GetInt32(2);
                            playerPerformance.WinPercentage = reader.GetDouble(3);
                            playerPerformancesList.Add(playerPerformance);
                        }
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return playerPerformancesList;
        }
    }
}
