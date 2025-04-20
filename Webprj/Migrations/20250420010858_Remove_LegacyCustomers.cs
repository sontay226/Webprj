using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webprj.Migrations
{
    public partial class RemoveLegacyCustomers : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            // 1) Drop old FK from Orders → Customers
            migrationBuilder.DropForeignKey(
                name: "FK__Orders__Customer__46E78A0C" ,
                table: "Orders");

            // 2) Drop legacy Customers table
            migrationBuilder.DropTable(
                name: "Customers");

            // 3) Add new FK Orders.CustomerID → AspNetUsers.Id
            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerID" ,
                table: "Orders" ,
                column: "CustomerID" ,
                principalTable: "AspNetUsers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            // 1) Remove new FK
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerID" ,
                table: "Orders");

            // 2) Recreate legacy Customers table
            migrationBuilder.CreateTable(
                name: "Customers" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    CustomerName = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: false) ,
                    Email = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    PasswordHash = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    PhoneNumber = table.Column<string>(type: "varchar(20)" , unicode: false , maxLength: 20 , nullable: true) ,
                    ShippingAddress = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    BillingAddress = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    CreatedAt = table.Column<DateTime>(type: "datetime" , nullable: false , defaultValueSql: "(getdate())")
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers" , x => x.Id);
                });

            // 3) Recreate unique index on Email
            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D1053485F331D6" ,
                table: "Customers" ,
                column: "Email" ,
                unique: true ,
                filter: "[Email] IS NOT NULL");

            // 4) Restore old FK Orders.CustomerID → Customers.Id
            migrationBuilder.AddForeignKey(
                name: "FK__Orders__Customer__46E78A0C" ,
                table: "Orders" ,
                column: "CustomerID" ,
                principalTable: "Customers" ,
                principalColumn: "Id" ,
                onDelete: ReferentialAction.SetNull);
        }
    }
}
