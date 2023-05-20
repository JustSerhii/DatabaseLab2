using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BDLab2.Models;


namespace BDLab2.Controllers
{
    public class QueriesController : Controller
    {

        private const string C1_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\C1.sql";
        private const string C2_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\C2.sql";
        private const string C3_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\C3.sql";
        private const string S1_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S1.sql";
        private const string S2_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S2.sql";
        private const string S3_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S3.sql";
        private const string S4_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S4.sql";
        private const string S5_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S5.sql";



        private const string CONN_STR = "Server=DESKTOP-R04J9T9\\SQLEXPRESS;Database=MusicDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate = true";

        private const string ERR_AVG = "No Albums";
        private const string ERR_LABEL = "No such Labels";
        private const string ERR_ALB = "No such Albums";
        private const string ERR_ART = "No such Artists";
        private const string ERR_GEN = "No such Genres";


        private readonly MusicDbContext _context;
        public QueriesController(MusicDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int errorCode)
        {
            var customers = _context.Genres.Select(c => c.Name).Distinct().ToList();
            if (errorCode == 1)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.PriceError = "Введіть коректну вартість";
            }
            if (errorCode == 2)
            {
                ViewBag.ErrorFlag = 2;
                ViewBag.ProdNameError = "Поле необхідно заповнити";
            }

            var empty = new SelectList(new List<string> { "--Пусто--" });
            var anyGenres = _context.Genres.Any();
            var anyArtists = _context.Artists.Any();
            var anyAlbums = _context.Albums.Any();

            ViewBag.ArtistIds = anyArtists ? new SelectList(_context.Artists, "Id", "Id") : empty;
            ViewBag.ArtistNames = anyArtists ? new SelectList(_context.Artists, "Name", "Name") : empty;
            ViewBag.GenreNames = anyGenres ? new SelectList(customers) : empty;
            ViewBag.GenreDescriptions = anyGenres ? new SelectList(_context.Genres, "Description", "Description") : empty;
            ViewBag.Labels = _context.Labels.Any() ? new SelectList(_context.Labels, "Name", "Name") : empty;

            ViewBag.AlbumTitles = anyAlbums ? new SelectList(_context.Albums, "Title", "Title") : empty;
            return View();
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery1(Models.Query queryModel)
        {
            string query = System.IO.File.ReadAllText(C1_PATH);
            query = query.Replace("Z", queryModel.ArtistId.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "C1";
            queryModel.LabelNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.LabelNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_LABEL;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery2(Models.Query queryModel)
        {
            string query = System.IO.File.ReadAllText(C2_PATH);
            query = query.Replace("Z", "N\'" + queryModel.GenreName.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "C2";
            queryModel.GenreDescriptions = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.GenreDescriptions.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_GEN;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery3(Models.Query queryModel)
        {
            
            string query = System.IO.File.ReadAllText(C3_PATH);
            query = query.Replace("Y", "N\'" + queryModel.GenreDescription.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "C3";
            queryModel.GenreNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.GenreNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_GEN;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery1(Models.Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S1_PATH);
            query = query.Replace("Z", "N\'" + queryModel.ArtistName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S1";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        queryModel.AvgPrice = Convert.ToDecimal(result);
                    }
                    else
                    {
                        queryModel.ErrorFlag = 1;
                        queryModel.Error = ERR_AVG;
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery2(Models.Query queryModel) //1. Знайти альбоми артиста Z
        {
            string query = System.IO.File.ReadAllText(S2_PATH);
            query = query.Replace("Z", "N\'" + queryModel.ArtistName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S2";
            queryModel.AlbumTitles = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.AlbumTitles.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_ALB;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery3(Models.Query queryModel)
        {
            if (ModelState.IsValid)
            {
                string query = System.IO.File.ReadAllText(S3_PATH);
                query = query.Replace("X", "N\'" + queryModel.AlbumTitle + "\'");
                query = query.Replace("\r\n", " ");
                query = query.Replace('\t', ' ');
                queryModel.QueryId = "S3";
                queryModel.ArtistNames = new List<string>();

                using (var connection = new SqlConnection(CONN_STR))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.ArtistNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.Error = ERR_ART;
                            }
                        }
                    }
                    connection.Close();
                }
                return RedirectToAction("Result", queryModel);
            }

            return RedirectToAction("Index", new { errorCode = 2 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery4(Models.Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S4_PATH);
            query = query.Replace("Z", "N\'" + queryModel.LabelName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S4";
            queryModel.AlbumTitles = new List<string>();
            queryModel.AlbumPrices = new List<decimal>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.AlbumTitles.Add(reader.GetString(0));
                            queryModel.AlbumPrices.Add(reader.GetDecimal(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_ALB;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery5(Models.Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S5_PATH);
            query = query.Replace("X", "N\'" + queryModel.ArtistName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S5";
            queryModel.GenreNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.GenreNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_GEN;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult Result(Models.Query queryResult)
        {
            return View(queryResult);
        }

    }
}
