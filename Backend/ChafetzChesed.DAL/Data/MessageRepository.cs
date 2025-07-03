using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using ChafetzChesed.DAL.Entities;



namespace ChafetzChesed.DAL.Data
{
    public class MessageRepository : IMessageRepository
    {
        private readonly string _connectionString;

        public MessageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Message> GetMessagesByClientId(string clientId)
        {
            var messages = new List<Message>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT * FROM Messages WHERE ClientID = @ClientID ORDER BY DateSent DESC", conn);
            cmd.Parameters.AddWithValue("@ClientID", clientId);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                messages.Add(new Message
                {
                    ID = (int)reader["ID"],
                    ClientID = (string)reader["ClientID"],
                    MessageType = (string)reader["MessageType"],
                    MessageText = (string)reader["MessageText"],
                    DateSent = (DateTime)reader["DateSent"],
                    IsRead = (bool)reader["IsRead"]
                });
            }

            return messages;
        }

        public void AddMessage(Message message)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Messages (ClientID, MessageType, MessageText, DateSent, IsRead)
                VALUES (@ClientID, @MessageType, @MessageText, @DateSent, @IsRead)", conn);

            cmd.Parameters.AddWithValue("@ClientID", message.ClientID);
            cmd.Parameters.AddWithValue("@MessageType", message.MessageType);
            cmd.Parameters.AddWithValue("@MessageText", message.MessageText);
            cmd.Parameters.AddWithValue("@DateSent", message.DateSent);
            cmd.Parameters.AddWithValue("@IsRead", message.IsRead);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
