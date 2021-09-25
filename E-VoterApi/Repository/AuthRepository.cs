using E_VoterApi.Models;
using System.Data.SqlClient;
using Dapper;
using System.Data;
using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace E_VoterApi.Repository
{
    public class AuthRepository : BaseRepository
    {
        private readonly string salt;
        
        public AuthRepository() : base()
        {
            salt = "saltyInnit";

        }

        public static async Task<bool> UserExists(Guid userID)
        {
            using SqlConnection con = new SqlConnection(ConnectionString);
            var checkUser = await con.QuerySingleAsync<Guid?>(
                    $@"select userID from VoterUsers where userID = {userID}",
                    commandType: CommandType.Text
                    );

            return checkUser != null ? true : false;
        }

        public async Task GenerateToken()
        {

        }

        public async Task VerifyToken()
        {

        }

        public async Task<(bool success, UserDetails user)> GetUserDetails(Guid userID)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var user = await con.QuerySingleAsync<UserDetails>(
                    "sp_GetUserDetails",
                    new
                    {
                        userID = userID
                    },
                    commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    return (false, new UserDetails());
                }
                else
                {
                    return (true, user);
                }
            }
            catch (Exception e)
            {
                return(false, new UserDetails());
                throw;
            }
        }

        public async Task<(bool verified, Guid userID)> VerifyUserCredentials(LoginModel credentials)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var user = await con.QuerySingleAsync<LoginModel>(
                    "sp_GetUserLoginDetails",
                    new
                    {
                        email = credentials.email
                    },
                    commandType: CommandType.StoredProcedure);

                if (user == null)
                {
                    return (false, new Guid());
                }
                else
                {
                    return (BCrypt.Net.BCrypt.Verify(credentials.password, user.password), user.userID.Value);
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<(bool success, Guid userID)> RegisterUser(UserDetails user)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var hash = BCrypt.Net.BCrypt.HashPassword(user.password, salt);

                var result = await con.QuerySingleAsync<Guid>(
                        $@"insert into VoterUsers (userID, firstName, lastName, contactNo, email, password) 
                           output inserted.userID
                           values (NEWID(), {user.firstName}, {user.lastName}, {user.contactNo}, {user.email}, {hash});",
                        commandType: CommandType.Text);
                return (true, result);
            }
            catch (Exception e)
            {
                return (false, new Guid());
                Console.WriteLine(e.Message);
                throw;
            }

        }
    }
}
