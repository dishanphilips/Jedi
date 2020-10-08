using Jedi;
using System;
using System.Collections.Generic;

namespace Jedi.Tests
{
    public class TestData
    {
        public class Database
        {
            [Inject(id: "DB_CONNECTION")]
            private string _connection;
        }

        public class Player
        {
            private string _id;
            private string _name;
            private int _age;

            public static Player GetRandomPlayer()
            {
                return new Player()
                {
                    _id = Guid.NewGuid().ToString(),
                    _name = $"Player_{new Random().Next(0, 100)}",
                    _age = new Random().Next(0, 100)
                };
            }
        }

        public class Achivements
        {
            private Dictionary<string, int> _rewards = new Dictionary<string, int>()
        {
            { "ach_1", 10},{ "ach_2", 20},{ "ach_3", 30},{ "ach_4", 40},
        };
        }

        public class PlayerProfileSystem
        {
            [Inject]
            private Player _player;

            [InjectOptional]
            private Achivements _ach;

            private Database _db;

            [Inject]
            private void Inject(Database db)
            {
                _db = db;
            }
        }
    }
}