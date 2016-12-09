using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using GMBuddy.Games.Micro20.Models;
using Microsoft.Extensions.Logging;

namespace GMBuddy.Games.Micro20.Data
{
    public static class DatabaseInitializer
    {
        private sealed class CsvItemMap : CsvClassMap<Item>
        {
            public CsvItemMap()
            {
                Map(m => m.Name);
                Map(m => m.Cost);
                Map(m => m.Description);
                Map(m => m.ItemType);
                Map(m => m.WeaponDamage).Name("Weapon Damage");
                Map(m => m.WeaponRange).Name("Weapon Range");
                Map(m => m.ArmorBonus).Name("Armor Bonus");
            }
        }

        private sealed class CsvSpellMap : CsvClassMap<Spell>
        {
            public CsvSpellMap()
            {
                Map(m => m.Name);
                Map(m => m.Level);
                Map(m => m.Description);
                Map(m => m.School);
            }
        }

        public static void Init(ILoggerFactory loggerFactory, string dir = null)
        {
            var logger = loggerFactory.CreateLogger(nameof(DatabaseInitializer));

            if (dir == null)
            {
                dir = Path.Combine(Utils.GetDataDirectory(), "Static");
            }

            string equipmentPath = Path.Combine(dir, "Equipment.csv");
            string spellsPath = Path.Combine(dir, "Spells.csv");

            if (!File.Exists(equipmentPath) || !File.Exists(spellsPath))
            {
                logger.LogError("Could not initialize database with equipment and spells");
                logger.LogError($"Directory {dir} did not contain an Equipment.csv and Spells.csv");
                return;
            }

            logger.LogInformation($"Using {equipmentPath} for items and {spellsPath} for spells");

            using (var reader = new StreamReader(new FileStream(equipmentPath, FileMode.Open)))
            {
                var config = new CsvConfiguration();
                config.RegisterClassMap(typeof(CsvItemMap));
                var equipment = new CsvReader(reader, config).GetRecords<Item>();

                using (var db = new DatabaseContext())
                {
                    if (!db.Items.Any())
                    {
                        logger.LogInformation("Adding default items to database");
                        db.Items.AddRange(equipment);

                        int changes = db.SaveChangesAsync().GetAwaiter().GetResult();
                        logger.LogInformation($"Added {changes} items to the database");
                    }
                }
            }

            using (var reader = new StreamReader(new FileStream(spellsPath, FileMode.Open)))
            {
                var config = new CsvConfiguration();
                config.RegisterClassMap(typeof(CsvSpellMap));
                var spells = new CsvReader(reader, config).GetRecords<Spell>();

                using (var db = new DatabaseContext())
                {
                    if (!db.Spells.Any())
                    {
                        logger.LogInformation("Adding default spells to database");
                        db.Spells.AddRange(spells);

                        int changes = db.SaveChangesAsync().GetAwaiter().GetResult();
                        logger.LogInformation($"Added {changes} spells to the database");
                    }
                }
            }
        }
    }
}
