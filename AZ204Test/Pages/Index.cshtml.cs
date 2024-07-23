using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AZ204Test.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<Courses> Courses = new List<Courses>();
    private IConfiguration _configuration;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public void OnGet()
    {
        string connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING");
        var sqlConnection = new SqlConnection(connectionString);
        sqlConnection.Open();
        var sqlCommand = new SqlCommand("Select * From Courses",sqlConnection);
        using(SqlDataReader sqlDataReader = sqlCommand.ExecuteReader()){
            while(sqlDataReader.Read()){
                Courses.Add(new Courses(){
                    CourseID=Int32.Parse(sqlDataReader["CourseID"].ToString()),
                    CourseName=sqlDataReader["CourseName"].ToString(),
                    Rating=Decimal.Parse(sqlDataReader["Rating"].ToString()),
                    });
            }
        }

        sqlConnection.Close();
    }
}
