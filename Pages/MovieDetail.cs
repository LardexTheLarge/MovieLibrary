using Microsoft.AspNetCore.Components;
using MovieLibrary.Services;
using MovieLibrary.Services.AirTable;
using MovieLibrary.Shared;

namespace MovieLibrary.Pages
{
    public partial class MovieDetail
    {
		[Parameter]
        public string MovieId { get; set; }
		public Movie Movie { get; set; } = new Movie();

		//[Inject]
		//public IMovieDataService MovieDataService { get; set; }
		public DataAccess dataAccess { get; set; }
		protected async override Task OnInitializedAsync()
		{
			dataAccess = new DataAccess();
			Movie = await dataAccess.GetMovieById(MovieId);
			
		}
	}
}
