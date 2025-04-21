using MVCSP.Interface;
using MVCSP.Models;
using System.Data;
using System.Data.SqlClient;

namespace MVCSP.Service
{
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        private readonly string connectionString;

        public ProductService(IConfiguration configuration) {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("DefaultConnection"); // or inject from settings
        }


        public async Task<(IEnumerable<Product> data, int totalCount)> GetPaginatedData(int page, int pagesize, string search)
        {
            var data = new List<Product>();
            int totalCount = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            using (SqlCommand cmd = new SqlCommand("GetPaginatedProducts", conn, transaction))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@Page", page);
                                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                                cmd.Parameters.AddWithValue("@Search", (object?)search ?? DBNull.Value);

                                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                                {
                                    // First result: total count
                                    if (await reader.ReadAsync())
                                    {
                                        totalCount = reader.GetInt32(0);
                                    }

                                    // Move to second result set
                                    await reader.NextResultAsync();

                                    while (await reader.ReadAsync())
                                    {
                                        data.Add(new Product
                                        {
                                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                            Name = reader.GetString(reader.GetOrdinal("Name")),
                                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                            IsSold = reader.GetBoolean(reader.GetOrdinal("IsSold"))
                                        });
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        catch (SqlException sqlEx)
                        {
                            transaction.Rollback();
                            await LogSqlError(conn, sqlEx);
                            throw; // Re-throw for upstream handling
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Optionally log this to a file or other logger
                throw;
            }

            return (data, totalCount);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            var data = new List<Product>();

            return data;
        }

        public async Task<Product> GetByIdAsync(int Id)
        {
            var data = new Product();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("GetByIdProduct", conn, transaction);

                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", Id);

                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                            {
                                if (await reader.ReadAsync())
                                {
                                    data = new Product
                                    {
                                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                        Name = reader.GetString(reader.GetOrdinal("Name")),
                                        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString(reader.GetOrdinal("Description")),
                                        IsSold = reader.GetBoolean(reader.GetOrdinal("IsSold"))
                                    };
                                }
                            }

                            transaction.Commit();
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            await LogSqlError(conn, ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return data;
        }

        public async Task AddAsync(Product data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            if (data.Description.Length > 100)
                                throw new Exception("Description exceeds allowed length.");

                            SqlCommand cmd = new SqlCommand("AddProduct", conn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Name", data.Name);
                            cmd.Parameters.AddWithValue("@Price", data.Price);
                            cmd.Parameters.AddWithValue("@Description", data.Description ?? "");
                            cmd.Parameters.AddWithValue("@IsSold", data.IsSold);

                            await cmd.ExecuteNonQueryAsync();

                            transaction.Commit();
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            await LogSqlError(conn, ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task UpdateAsync(Product data)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {

                            if (data.Description.Length > 100)
                                throw new Exception("Description exceeds allowed length.");

                            SqlCommand cmd = new SqlCommand("UpdateProduct", conn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", data.Id);
                            cmd.Parameters.AddWithValue("@Name", data.Name);
                            cmd.Parameters.AddWithValue("@Price", data.Price);
                            cmd.Parameters.AddWithValue("@Description", data.Description ?? "");
                            cmd.Parameters.AddWithValue("@IsSold", data.IsSold);

                            await cmd.ExecuteNonQueryAsync();

                            transaction.Commit();
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            await LogSqlError(conn, ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (SqlTransaction transaction = conn.BeginTransaction())
                    {
                        try
                        {
                            SqlCommand cmd = new SqlCommand("DeleteProduct", conn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@Id", id);

                            await cmd.ExecuteNonQueryAsync();

                            transaction.Commit();
                        }
                        catch (SqlException ex)
                        {
                            transaction.Rollback();
                            await LogSqlError(conn, ex);
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        private async Task LogSqlError(SqlConnection conn, SqlException ex)
        {
            try
            {
                using (SqlCommand logCmd = new SqlCommand("INSERT INTO ErrorLog (ErrorMessage, StackTrace) VALUES (@msg, @trace)", conn))
                {
                    logCmd.Parameters.AddWithValue("@msg", ex.Message);
                    logCmd.Parameters.AddWithValue("@trace", ex.StackTrace ?? "");
                    await logCmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
