using KantanMitsumori.Entity.ASESTSQL;
using Microsoft.EntityFrameworkCore;

namespace KantanMitsumori.DataAccess
{
    public partial class ASESTContext
    {
         partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TbRuibetsuEntity>().HasNoKey();
            modelBuilder.Entity<SerEstEntity>().HasNoKey();
        }
    }
}
