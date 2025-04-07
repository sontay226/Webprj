﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webprj.Migrations
{
    public partial class AddIdentityTablesOnly : Migration
    {
        protected override void Up( MigrationBuilder migrationBuilder )
        {
            // Tạo bảng AspNetRoles
            migrationBuilder.CreateTable(
                name: "AspNetRoles" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    Name = table.Column<string>(type: "nvarchar(256)" , maxLength: 256 , nullable: true) ,
                    NormalizedName = table.Column<string>(type: "nvarchar(256)" , maxLength: 256 , nullable: true) ,
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)" , nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles" , x => x.Id);
                });

            // Tạo bảng AspNetUsers với cột khóa chính là Id
            migrationBuilder.CreateTable(
                name: "AspNetUsers" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    CustomerName = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: false) ,
                    ShippingAddress = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    BillingAddress = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    CreatedAt = table.Column<DateTime>(type: "datetime" , nullable: false , defaultValueSql: "(getdate())") ,
                    UserName = table.Column<string>(type: "nvarchar(256)" , maxLength: 256 , nullable: true) ,
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)" , maxLength: 256 , nullable: true) ,
                    Email = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)" , maxLength: 256 , nullable: true) ,
                    EmailConfirmed = table.Column<bool>(type: "bit" , nullable: false) ,
                    PasswordHash = table.Column<string>(type: "nvarchar(255)" , maxLength: 255 , nullable: true) ,
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)" , nullable: true) ,
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)" , nullable: true) ,
                    PhoneNumber = table.Column<string>(type: "varchar(20)" , unicode: false , maxLength: 20 , nullable: true) ,
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit" , nullable: false) ,
                    TwoFactorEnabled = table.Column<bool>(type: "bit" , nullable: false) ,
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset" , nullable: true) ,
                    LockoutEnabled = table.Column<bool>(type: "bit" , nullable: false) ,
                    AccessFailedCount = table.Column<int>(type: "int" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers" , x => x.Id);
                });

            // Tạo bảng AspNetRoleClaims
            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    RoleId = table.Column<int>(type: "int" , nullable: false) ,
                    ClaimType = table.Column<string>(type: "nvarchar(max)" , nullable: true) ,
                    ClaimValue = table.Column<string>(type: "nvarchar(max)" , nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId" ,
                        column: x => x.RoleId ,
                        principalTable: "AspNetRoles" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            // Tạo bảng AspNetUserClaims với FK tham chiếu cột Id của AspNetUsers
            migrationBuilder.CreateTable(
                name: "AspNetUserClaims" ,
                columns: table => new
                {
                    Id = table.Column<int>(type: "int" , nullable: false)
                        .Annotation("SqlServer:Identity" , "1, 1") ,
                    UserId = table.Column<int>(type: "int" , nullable: false) ,
                    ClaimType = table.Column<string>(type: "nvarchar(max)" , nullable: true) ,
                    ClaimValue = table.Column<string>(type: "nvarchar(max)" , nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims" , x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            // Tạo bảng AspNetUserLogins với FK tham chiếu cột Id của AspNetUsers
            migrationBuilder.CreateTable(
                name: "AspNetUserLogins" ,
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)" , nullable: false) ,
                    ProviderKey = table.Column<string>(type: "nvarchar(450)" , nullable: false) ,
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)" , nullable: true) ,
                    UserId = table.Column<int>(type: "int" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins" , x => new { x.LoginProvider , x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            // Tạo bảng AspNetUserRoles với FK tham chiếu cột Id của AspNetUsers
            migrationBuilder.CreateTable(
                name: "AspNetUserRoles" ,
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int" , nullable: false) ,
                    RoleId = table.Column<int>(type: "int" , nullable: false)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles" , x => new { x.UserId , x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId" ,
                        column: x => x.RoleId ,
                        principalTable: "AspNetRoles" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            // Tạo bảng AspNetUserTokens với FK tham chiếu cột Id của AspNetUsers
            migrationBuilder.CreateTable(
                name: "AspNetUserTokens" ,
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int" , nullable: false) ,
                    LoginProvider = table.Column<string>(type: "nvarchar(450)" , nullable: false) ,
                    Name = table.Column<string>(type: "nvarchar(450)" , nullable: false) ,
                    Value = table.Column<string>(type: "nvarchar(max)" , nullable: true)
                } ,
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens" , x => new { x.UserId , x.LoginProvider , x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId" ,
                        column: x => x.UserId ,
                        principalTable: "AspNetUsers" ,
                        principalColumn: "Id" ,
                        onDelete: ReferentialAction.Cascade);
                });

            // Tạo các index cho ASP.NET Identity
            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId" ,
                table: "AspNetRoleClaims" ,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex" ,
                table: "AspNetRoles" ,
                column: "NormalizedName" ,
                unique: true ,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId" ,
                table: "AspNetUserClaims" ,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId" ,
                table: "AspNetUserLogins" ,
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId" ,
                table: "AspNetUserRoles" ,
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex" ,
                table: "AspNetUsers" ,
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UQ__Customer__A9D1053485F331D6" ,
                table: "AspNetUsers" ,
                column: "Email" ,
                unique: true ,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex" ,
                table: "AspNetUsers" ,
                column: "NormalizedUserName" ,
                unique: true ,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down( MigrationBuilder migrationBuilder )
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
