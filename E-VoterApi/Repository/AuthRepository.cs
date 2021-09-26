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
        private readonly int salt;
        
        public AuthRepository() : base()
        {
            salt = 12;

        }

        public static async Task<bool> UserExists(Guid userID)
        {
            using SqlConnection con = new SqlConnection(ConnectionString);
            var checkUser = await con.QuerySingleAsync<Guid?>(
                    $@"select userID from VoterUsers where userID = '{userID}'",
                    commandType: CommandType.Text
                    );

            return checkUser != null ? true : false;
        }

        public static async Task<bool> UserExists(string email)
        {
            using SqlConnection con = new SqlConnection(ConnectionString);
            var checkUser = await con.QuerySingleAsync<Guid?>(
                    $@"select email from VoterUsers where email = '{email}'",
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
                    $@"select email, userID, firstName, lastName, contactNo from VoterUsers where userID = '{userID}';",
                    commandType: CommandType.Text);

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
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (false, new UserDetails());
                throw;
            }
        }

        public async Task<(bool verified, Guid userID)> VerifyUserCredentials(LoginModel credentials)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);
                var user = await con.QuerySingleAsync<(string email, string password, Guid userID)?>(
                    $@"select email, password, userID from VoterUsers where email = '{credentials.email}';",
                    commandType: CommandType.Text);

                if (user == null)
                {
                    return (false, new Guid());
                }
                else
                {
                    return (BCrypt.Net.BCrypt.Verify(credentials.password, user.Value.password), user.Value.userID);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                throw;
            }
        }

        public async Task<(int status, Guid userID)> RegisterUser(RegisterUserModel user)
        {
            try
            {
                using SqlConnection con = new SqlConnection(ConnectionString);

                if (!(await UserExists(user.email)))
                    return (0, new Guid());

                var hash = BCrypt.Net.BCrypt.HashPassword(user.password, salt);
                var result = await con.QuerySingleAsync<Guid>(
                        $@"insert into VoterUsers (userID, firstName, lastName, contactNo, email, password) 
                           output inserted.userID
                           values (NEWID(), '{user.firstName}', '{user.lastName}', '{user.contactNo}', '{user.email}', '{hash}');",
                        commandType: CommandType.Text);
                return (1, result);
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}: {e.InnerException}");
                return (-1, new Guid());
                throw;
            }

        }
    }
}
