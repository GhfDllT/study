#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspnetcoreapp.Data;
using aspnetcoreapp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace aspnetcoreapp.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly aspnetcoreapp.Data.aspnetcoreappContext _context;

        public IndexModel(aspnetcoreapp.Data.aspnetcoreappContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get;set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        public SelectList Genres { get; set; }

        [BindProperty(SupportsGet = true)]
        public string MovieGenre { get; set; }

        public async Task OnGetAsync()
        {
            var genres = _context
                .Movie
                .OrderBy(x => x.Genre)
                .Select(x => x.Genre);

            var movies = _context.Movie.Select(x => x);

            if (!string.IsNullOrEmpty(SearchString))
            {
                movies = movies.Where(x => x.Title.Contains(SearchString));
            }

            if (!string.IsNullOrEmpty(MovieGenre))
            {
                movies = movies.Where(x => x.Genre == MovieGenre);
            }
            Genres = new SelectList(await genres.Distinct().ToListAsync());
            Movie = await movies.ToListAsync();
        }
    }
}
