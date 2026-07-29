using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows.Forms;

namespace PhoneServiceSystem.ClassFolder
{
    public static class Repository
    {
        private static readonly string _databaseFile =
            Path.Combine(Application.StartupPath, "database.db");
        private static readonly string _connString =
            "Data Source=" + _databaseFile + ";Version=3;";
        static Repository()
        {
            if (!File.Exists(_databaseFile))
                SQLiteConnection.CreateFile(_databaseFile);

            using (var conn = new SQLiteConnection(_connString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Client (
                        ClientId    INTEGER PRIMARY KEY AUTOINCREMENT,
                        FirstName   TEXT    NOT NULL,
                        LastName    TEXT    NOT NULL,
                        PhoneNumber TEXT    NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS ExtraOption (
                        ExtraOptionId INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name          TEXT    NOT NULL,
                        MonthlyCost   REAL    NOT NULL
                    );
                    CREATE TABLE IF NOT EXISTS Subscription (
                        SubscriptionId INTEGER PRIMARY KEY AUTOINCREMENT,
                        ClientId       INTEGER NOT NULL,
                        ExtraOptionId  INTEGER NOT NULL,
                        StartDate      TEXT    NOT NULL,
                        EndDate        TEXT,
                        FOREIGN KEY(ClientId)      REFERENCES Client(ClientId),
                        FOREIGN KEY(ExtraOptionId) REFERENCES ExtraOption(ExtraOptionId)
                    );";
                cmd.ExecuteNonQuery();
            }
        }
        public static List<Client> GetAllClients()
        {
            var list = new List<Client>();
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT ClientId, FirstName, LastName, PhoneNumber
                          FROM Client
                      ORDER BY LastName, FirstName;";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new Client
                            {
                                ClientId = rdr.GetInt32(0),
                                FirstName = rdr.GetString(1),
                                LastName = rdr.GetString(2),
                                PhoneNumber = rdr.GetString(3)
                            });
                        }
                    }
                }
            }
            return list;
        }
        public static void InsertClient(Client c)
        {
            if (string.IsNullOrWhiteSpace(c.FirstName))
                throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(c.LastName))
                throw new ArgumentException("Last name is required");
            if (string.IsNullOrWhiteSpace(c.PhoneNumber))
                throw new ArgumentException("Phone number is required");
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Client (FirstName,LastName,PhoneNumber)
                             VALUES (@fn,@ln,@pn);
                        SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("@fn", c.FirstName);
                    cmd.Parameters.AddWithValue("@ln", c.LastName);
                    cmd.Parameters.AddWithValue("@pn", c.PhoneNumber);
                    c.ClientId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public static void UpdateClient(Client c)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Client
                           SET FirstName   = @fn,
                               LastName    = @ln,
                               PhoneNumber = @pn
                         WHERE ClientId   = @id;";
                    cmd.Parameters.AddWithValue("@fn", c.FirstName);
                    cmd.Parameters.AddWithValue("@ln", c.LastName);
                    cmd.Parameters.AddWithValue("@pn", c.PhoneNumber);
                    cmd.Parameters.AddWithValue("@id", c.ClientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteClient(long clientId)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Client WHERE ClientId = @id;";
                    cmd.Parameters.AddWithValue("@id", clientId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void SaveClients(IEnumerable<Client> clients)
        {
            foreach (var c in clients)
            {
                if (c.ClientId == 0) InsertClient(c);
                else UpdateClient(c);
            }
        }
        public static List<ExtraOption> GetAllExtraOptions()
        {
            var list = new List<ExtraOption>();
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT ExtraOptionId, Name, MonthlyCost
                          FROM ExtraOption
                      ORDER BY Name;";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            float cost = (float)rdr.GetDouble(2);
                            list.Add(new ExtraOption
                            {
                                ExtraOptionId = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                MonthlyCost = cost
                            });
                        }
                    }
                }
            }
            return list;
        }

        public static void InsertExtraOption(ExtraOption o)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO ExtraOption (Name,MonthlyCost)
                             VALUES (@nm,@mc);
                        SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("@nm", o.Name);
                    cmd.Parameters.AddWithValue("@mc", (double)o.MonthlyCost);
                    o.ExtraOptionId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
        public static void UpdateExtraOption(ExtraOption o)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE ExtraOption
                           SET Name        = @nm,
                               MonthlyCost = @mc
                         WHERE ExtraOptionId = @id;";
                    cmd.Parameters.AddWithValue("@nm", o.Name);
                    cmd.Parameters.AddWithValue("@mc", (double)o.MonthlyCost);
                    cmd.Parameters.AddWithValue("@id", o.ExtraOptionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void DeleteExtraOption(long extraOptionId)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM ExtraOption WHERE ExtraOptionId = @id;";
                    cmd.Parameters.AddWithValue("@id", extraOptionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void SaveExtraOptions(IEnumerable<ExtraOption> options)
        {
            foreach (var o in options)
            {
                if (o.ExtraOptionId == 0) InsertExtraOption(o);
                else UpdateExtraOption(o);
            }
        }
        public static List<Subscription> GetAllSubscriptions()
        {
            var list = new List<Subscription>();
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT SubscriptionId, ClientId, ExtraOptionId, StartDate, EndDate
                          FROM Subscription
                      ORDER BY StartDate;";
                    using (var rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            list.Add(new Subscription
                            {
                                SubscriptionId = rdr.GetInt32(0),
                                ClientId = rdr.GetInt32(1),
                                ExtraOptionId = rdr.GetInt32(2),
                                StartDate = DateTime.Parse(rdr.GetString(3)),
                                EndDate = rdr.IsDBNull(4)
                                                    ? (DateTime?)null
                                                    : DateTime.Parse(rdr.GetString(4))
                            });
                        }
                    }
                }
            }
            return list;
        }
        public static void InsertSubscription(Subscription s)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Subscription
                            (ClientId,ExtraOptionId,StartDate,EndDate)
                             VALUES (@cid,@eid,@sd,@ed);
                        SELECT last_insert_rowid();";
                    cmd.Parameters.AddWithValue("@cid", s.ClientId);
                    cmd.Parameters.AddWithValue("@eid", s.ExtraOptionId);
                    cmd.Parameters.AddWithValue("@sd", s.StartDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ed",
                        s.EndDate.HasValue
                          ? (object)s.EndDate.Value.ToString("yyyy-MM-dd")
                          : DBNull.Value);
                    s.SubscriptionId = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public static void UpdateSubscription(Subscription s)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Subscription
                           SET ClientId      = @cid,
                               ExtraOptionId = @eid,
                               StartDate     = @sd,
                               EndDate       = @ed
                         WHERE SubscriptionId = @id;";
                    cmd.Parameters.AddWithValue("@cid", s.ClientId);
                    cmd.Parameters.AddWithValue("@eid", s.ExtraOptionId);
                    cmd.Parameters.AddWithValue("@sd", s.StartDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@ed",
                        s.EndDate.HasValue
                          ? (object)s.EndDate.Value.ToString("yyyy-MM-dd")
                          : DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", s.SubscriptionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteSubscription(long subscriptionId)
        {
            using (var conn = new SQLiteConnection(_connString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Subscription WHERE SubscriptionId = @id;";
                    cmd.Parameters.AddWithValue("@id", subscriptionId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static void SaveSubscriptions(IEnumerable<Subscription> subs)
        {
            foreach (var s in subs)
            {
                if (s.SubscriptionId == 0) InsertSubscription(s);
                else UpdateSubscription(s);
            }
        }
    }
}
