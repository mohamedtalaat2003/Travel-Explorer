using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Travel_Explorer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFlightScheduleConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 'xmin' is a PostgreSQL system column already present on every table. It is used here as the
            // optimistic-concurrency token for flight_schedules (see UseXminAsConcurrencyToken). No DDL is
            // required - attempting to ADD COLUMN "xmin" would fail - so this migration only records the
            // model change; the concurrency token is enforced by EF at runtime.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
