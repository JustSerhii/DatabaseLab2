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
        private const string S1_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S1.sql";
        private const string S2_PATH = @"C:\Users\ks\Desktop\projects visual\BDLab2\Requests\S2.sql";
        private const string CONN_STR = "Server=DESKTOP-R04J9T9\\SQLEXPRESS;Database=MusicDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate = true";

        private const string ERR_AVG = "No products";
        private const string ERR_LABEL = "No Labels";
        private const string ERR_PROD = "No such albums";

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

            ViewBag.ArtistIds = anyArtists ? new SelectList(_context.Artists, "Id", "Id") : empty;
            ViewBag.ArtistNames = anyArtists ? new SelectList(_context.Artists, "Name", "Name") : empty;
            ViewBag.GenreNames = anyGenres ? new SelectList(customers) : empty;
            ViewBag.GenreDescriptions = anyGenres ? new SelectList(_context.Genres, "GenreDescription", "GenreDescription") : empty;
            ViewBag.Labels = _context.Labels.Any() ? new SelectList(_context.Labels, "Name", "Name") : empty;
            return View();
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
                            queryModel.Error = ERR_PROD;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
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


        public IActionResult Result(Models.Query queryResult)
        {
            return View(queryResult);
        }

    }
}
