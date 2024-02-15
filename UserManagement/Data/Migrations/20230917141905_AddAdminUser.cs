using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagement.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount], [FirstName], [LastName], [ProfilePicture]) VALUES (N'6a0e57f0-5f27-434a-b402-f252c43ddfa7', N'admin', N'ADMIN', N'admin@admin.com', N'ADMIN@ADMIN.COM', 0, N'AQAAAAIAAYagAAAAEEiNz3UzT2P+/cvnuPSg6lyKxOSuks7c5ZcVEY4qWvHziQ7zXXYYEtxmnduetaMyhA==', N'YQWW6EKN4TPHOWU2E2TP7QST7DLN3MHH', N'0483b01e-3502-4abb-a1a2-3891f5107fad', NULL, 0, 0, NULL, 1, 0, N'sally', N'karam', NULL)\r\n");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[AspNetUsers] WHERE Id = '6a0e57f0-5f27-434a-b402-f252c43ddfa7' ");
        }
    }
}
