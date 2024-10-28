using Microsoft.EntityFrameworkCore;

namespace CloudFolder.Data;

public class ApplicationDbContext :DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options){

    }
}