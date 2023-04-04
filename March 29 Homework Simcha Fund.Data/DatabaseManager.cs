using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace March_29_Homework_Simcha_Fund.Data
{
    public class DatabaseManager
    {
        private string _connectionString = "Data Source=.\\sqlexpress;Initial Catalog=SimchaFund;Integrated Security=True";

        public List<Simcha> GetSimchas()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT s.Id, s.SimchaName, s.SimchaDate, SUM(co.ContributionAmount) as 'ContributionAmount', " +
               "COUNT(co.ContributionAmount) AS 'ContributorCount' FROM Simchas s " +
               "LEFT JOIN Contributions co " +
               "ON co.SimchaId = s.Id " +
               "GROUP BY s.Id, s.SimchaName, s.SimchaDate";
            connection.Open();
            List<Simcha> simchas = new List<Simcha>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                simchas.Add(new Simcha
                {
                    Id = (int)reader["Id"],
                    SimchaName = (string)reader["SimchaName"],
                    SimchaDate = (DateTime)reader["SimchaDate"],
                    ContributorCount = (int)reader["ContributorCount"],
                    TotalContributed = reader.GetOrNull<decimal>("ContributionAmount")
                });
            }
            return simchas;
        }

        public string GetSimchaName(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT SimchaName From Simchas WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name = (string)reader["SimchaName"];
            }
            return name;
        }
        public List<Contributor> GetContributors()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Contributors";
            connection.Open();
            List<Contributor> contributors = new List<Contributor>();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                contributors.Add(new Contributor
                {
                    Id = (int)reader["Id"],
                    FirstName = (string)reader["FirstName"],
                    LastName = (string)reader["LastName"],
                    AlwaysInclude = (bool)reader["AlwaysInclude"],
                    CellNumber = (string)reader["CellNumber"],
                    DateCreated = (DateTime)reader["DateCreated"],
                    Balance = GetBalance((int)reader["Id"]),
                });
            }

            return contributors;
        }

       

        public void AddSimcha(Simcha simcha)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Simchas (SimchaName, SimchaDate) " +
                                  "VALUES (@simchaname, @simchadate)";
            command.Parameters.AddWithValue("@simchaname", simcha.SimchaName);
            command.Parameters.AddWithValue("@simchadate", simcha.SimchaDate.ToShortDateString());
            connection.Open();
            command.ExecuteNonQuery();
        }

        public int AddContributor(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Contributors (FirstName, LastName, DateCreated, CellNumber, AlwaysInclude) " +
                                  "VALUES (@FirstName, @LastName, @DateCreated, @CellNumber, @AlwaysInclude); SELECT SCOPE_IDENTITY();";
            command.Parameters.AddWithValue("@FirstName", c.FirstName);
            command.Parameters.AddWithValue("@LastName", c.LastName);
            command.Parameters.AddWithValue("@DateCreated", c.DateCreated);
            command.Parameters.AddWithValue("@CellNumber", c.CellNumber);
            command.Parameters.AddWithValue("@AlwaysInclude", c.AlwaysInclude);
            connection.Open();

            return (int)(decimal)command.ExecuteScalar();
        }

        public void AddDeposit(Deposit deposit)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT INTO Deposits (ContributorId, DepositDate, DepositAmount) " +
                                  "VALUES (@ContributorId, @DepositDate, @DepositAmount)";
            command.Parameters.AddWithValue("@ContributorId", deposit.ContributorId);
            command.Parameters.AddWithValue("@DepositDate", deposit.DepositDate);
            command.Parameters.AddWithValue("@DepositAmount", deposit.DepositAmount);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public void EditContributor(Contributor c)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "UPDATE Contributors " +
                "Set FirstName = @firstname, LastName = @lastname, DateCreated = @datecreated, CellNumber = @cellnumber, AlwaysInclude = @alwaysinclude " +
                "WHERE Id = @id";
            command.Parameters.AddWithValue("@firstname", c.FirstName);
            command.Parameters.AddWithValue("@lastname", c.LastName);
            command.Parameters.AddWithValue("@datecreated", c.DateCreated);
            command.Parameters.AddWithValue("@cellnumber", c.CellNumber);
            command.Parameters.AddWithValue("@alwaysinclude", c.AlwaysInclude);
            command.Parameters.AddWithValue("@id", c.Id);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<Contributions> GetContributionsBySimcha(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Select co.*, s.SimchaName From Contributions co " +
                "JOIN Contributors c ON co.ContributorId = c.Id " +
                "JOIN simchas s ON s.Id = co.SimchaId " +
                "WHERE s.Id = @id";
            command.Parameters.AddWithValue("@id", simchaId);
            connection.Open();
            var contributions = new List<Contributions>();
            SqlDataReader reader = command.ExecuteReader();
            //if(!reader.Read())
            //{
            //    return null;
            //}
            while (reader.Read())
            {
                contributions.Add(new Contributions
                {
                    ContributorId = (int)reader["ContributorId"],
                    //SimchaId = (int)reader["SimchaId"],
                    ContributionAmount = (decimal)reader["ContributionAmount"],
                });
            }
            return contributions;
        }

        

        public decimal GetBalance(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Select SUM(DepositAmount) AS 'Total' from deposits " +
                "Where ContributorId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            decimal totalDeposits = 0;
            while(reader.Read())
            {
                totalDeposits = (decimal)reader["Total"];
            }

            decimal totalContributions = GetTotalContributionsForId(id);

            return totalDeposits - totalContributions;
        }

        public decimal GetTotalContributionsForId(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Select SUM(ContributionAmount) AS 'Total' from Contributions " +
                "Where ContributorId = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            decimal totalContributions = 0;
            while (reader.Read())
            {
                totalContributions = reader.GetOrNull<decimal>("Total");
            }

            return totalContributions;
        }

        public decimal GetTotalContributionsForAll()
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Select SUM(ContributionAmount) AS 'Total' from Contributions";
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            decimal totalContributions = 0;
            while (reader.Read())
            {
                totalContributions = reader.GetOrNull<decimal>("Total");
            }

            return totalContributions;
        }

        public void UpdateContributions(List<Contributions> contributions, int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "INSERT into Contributions (SimchaId, ContributorId, ContributionAmount) " +
                "VALUES (@simchaid, @contributorid, @contributionamount)";
            connection.Open();

            foreach (Contributions c in contributions)
            {
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@simchaid", simchaId);
                command.Parameters.AddWithValue("@contributorid", c.ContributorId);
                command.Parameters.AddWithValue("@contributionamount", c.ContributionAmount);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteContributions(int simchaId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "Delete From Contributions " +
                "WHERE SimchaId = @simchaid";
            command.Parameters.AddWithValue("@simchaid", simchaId);
            connection.Open();
            command.ExecuteNonQuery();
        }

        public List<Transactions> GetContributionsByContributor(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT co.ContributionAmount, s.SimchaName, s.SimchaDate From Contributions co " +
                "JOIN Simchas s " +
                "ON s.Id = co.SimchaId " +
                "WHERE co.ContributorId = @ContributorId";
            command.Parameters.AddWithValue("@ContributorId", contributorId);
            List<Transactions> transactions = new List<Transactions>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                transactions.Add(new Transactions
                {
                    Amount = -(decimal)reader["ContributionAmount"],
                    Date = (DateTime)reader["SimchaDate"],
                    Action = "Contribution for the " + (string)reader["SimchaName"] + " Simcha"
                });
            }
            
            return transactions;
        }

        public List<Transactions> GetDepositsByContributor(int contributorId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * From Deposits " +
                "WHERE ContributorId = @id";
            command.Parameters.AddWithValue("@id", contributorId);
            List<Transactions> depositTransactions = new List<Transactions>();
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                depositTransactions.Add(new Transactions
                {
                    Amount = (decimal)reader["DepositAmount"],
                    Date = (DateTime)reader["DepositDate"],
                    Action = "Deposit",
                });
            }

            return depositTransactions;
        }

        public string GetNameById(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT FirstName, LastName From Contributors " +
                "WHERE Id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            string name = "";
            while (reader.Read())
            {
                name += (string)reader["FirstName"] + " " + (string)reader["LastName"];
            };

            return name;
        }
    
    }
}
