using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using TechArt.DataClass.DataClassAccess;

namespace TechArt.DataClass.Database
{
    public class DataContext : DbContext
    {
        public DataContext()
            : base(@"data source=Acesli\SQL2014;initial catalog=TechArt;user id=sa;password=techart_2014")
        {
        }

        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<Identity> Identity { get; set; }

        public virtual DbSet<UserInfo> UserInfo { get; set; }

        public virtual DbSet<UserToken> UserToken { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Session
            modelBuilder.Entity<Session>()
                .HasMany(e => e.Tokens)
                .WithRequired(e => e.Session)
                .WillCascadeOnDelete(false);

            //Token
            modelBuilder.Entity<Token>()
                .HasMany(e => e.UserTokens)
                .WithRequired(e=>e.Token)
                .WillCascadeOnDelete(false);

            //User Info 
            modelBuilder.Entity<UserInfo>()
                .Property(e => e.UserName)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            modelBuilder.Entity<UserInfo>()
                .HasRequired(e => e.Identity)
                .WithRequiredDependent(r => r.UserInfo);
        }
    }
}