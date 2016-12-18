using FreeBird.Infrastructure.DataInitializers;
using SimpleSSO.EFRepositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wechart.EFRepositories.Migrations
{
    public class SimpleDbContextInitializer : DropCreateDatabaseIfModelChanges<SimpleDbContext>
    {
        protected override void Seed(SimpleDbContext context)
        {
            var commands = SqlCommandProvider.GetCommands("create_init_data.sql");
            if (commands != null && commands.Length > 0)
            {
                foreach (var command in commands)
                    context.Database.ExecuteSqlCommand(command);
            }
        }
    }
}
