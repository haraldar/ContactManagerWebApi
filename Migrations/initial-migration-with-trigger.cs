using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ContactManagerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrationwithtrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    Salution = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    CreationTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp"),
                    LastChangeTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "current_timestamp")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contacts", x => x.Id);
                });

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION func_update_last_modified_timestamp()
                RETURNS TRIGGER AS $$
                BEGIN
                    NEW.""LastChangeTimestamp"" = CURRENT_TIMESTAMP;
                    RETURN NEW;
                END;
                $$ LANGUAGE PLPGSQL;
            ");

            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_update_last_modified_timestamp
                BEFORE UPDATE ON contacts
                FOR EACH ROW
                EXECUTE FUNCTION func_update_last_modified_timestamp();
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "contacts");
        }
    }
}
