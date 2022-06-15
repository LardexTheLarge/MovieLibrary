using Microsoft.AspNetCore.Components;
using MovieLibrary.Services;
using MovieLibrary.Services.AirTable;
using MovieLibrary.Shared;

namespace MovieLibrary.Pages
{
	public partial class MovieOverview
	{
		public IEnumerable<Movie> Movies { get; set; }

		//[Inject]
		//public IMovieDataService MovieDataService { get; set; }

        protected async override Task OnInitializedAsync()
        {
			DataAccess dataAccess = new DataAccess();
			var list = await dataAccess.GetAllMovies();
			Movies = list;
        }
	}
}