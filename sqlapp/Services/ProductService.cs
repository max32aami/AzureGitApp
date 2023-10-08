using sqlapp.Models;
using System.Data.SqlClient;

namespace sqlapp.Services
{

    // This service will interact with our Product data in the SQL database
    public class ProductService : IProductService
    {
        private readonly IConfiguration _configuration;
        public ProductService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private SqlConnection GetConnection()
        {
            var builder = WebApplication.CreateBuilder();
            var app = builder.Build();
            if(app.Environment.IsDevelopment())
            {
                return GetConnectionLocal();
            }
            return new SqlConnection(_configuration.GetConnectionString("SQLConnection"));
        }

        private SqlConnection GetConnectionLocal()
        {

            var _builder = new SqlConnectionStringBuilder();
            _builder.DataSource = "tcp:maxsql.database.windows.net,1433";
            _builder.UserID = "sqladmin";
            _builder.Password = "Netsolpk1!";
            _builder.InitialCatalog = "appdb";
            return new SqlConnection(_builder.ConnectionString);
        }

        public List<Product> GetProducts()
        {
            List<Product> _product_lst = new List<Product>();
            string _statement = "SELECT ProductID,ProductName,Quantity from Products";
            SqlConnection _connection = GetConnection();

            _connection.Open();

            SqlCommand _sqlcommand = new SqlCommand(_statement, _connection);

            using (SqlDataReader _reader = _sqlcommand.ExecuteReader())
            {
                while (_reader.Read())
                {
                    Product _product = new Product()
                    {
                        ProductID = _reader.GetInt32(0),
                        ProductName = _reader.GetString(1),
                        Quantity = _reader.GetInt32(2)
                    };

                    _product_lst.Add(_product);
                }
            }
            _connection.Close();
            return _product_lst;
        }

    }
}

