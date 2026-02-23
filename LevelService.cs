using Newtonsoft.Json;
using MRXLevelBot.models;

namespace MRXLevelBot
{
    public class LevelService
    {
        private Dictionary<ulong, UserLevel> users;
        private string path = "data/levels.json";

        public LevelService()
        {
            if (!File.Exists(path))
                users = new Dictionary<ulong, UserLevel>();
            else
                users = JsonConvert.DeserializeObject<Dictionary<ulong, UserLevel>>(File.ReadAllText(path))
                        ?? new Dictionary<ulong, UserLevel>();
        }

        public UserLevel GetUser(ulong id)
        {
            if (!users.ContainsKey(id))
            {
                users[id] = new UserLevel
                {
                    UserId = id,
                    XP = 0,
                    Level = 1
                };
                Save();
            }

            return users[id];
        }

        public bool AddXP(ulong id, int amount)
        {
            var user = GetUser(id);

            user.XP += amount;

            int neededXP = user.Level * 100;

            if (user.XP >= neededXP)
            {
                user.Level++;
                user.XP = 0;
                Save();
                return true;
            }

            Save();
            return false;
        }

        public void Save()
        {
            File.WriteAllText(path,
                JsonConvert.SerializeObject(users, Formatting.Indented));
        }
    }
}