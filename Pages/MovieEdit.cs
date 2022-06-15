using Microsoft.AspNetCore.Components;
using MovieLibrary.Services;
using MovieLibrary.Services.AirTable;
using MovieLibrary.Shared;

namespace MovieLibrary.Pages
{
    public partial class MovieEdit
    {
        //[Inject]
        //public IMovieDataService MovieDataService { get; set; }
        public DataAccess dataAccess { get; set; }

        [Parameter]
        public string MovieId { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public Movie Movie { get; set; } = new Movie();

        protected string Message = string.Empty;
        protected string StatusClass = string.Empty;
        protected bool Saved;

        protected async override Task OnInitializedAsync()
        {
            dataAccess = new DataAccess();

            //Movie = await MovieDataService.GetMovieById(int.Parse(MovieId));

            if (string.IsNullOrEmpty(MovieId))
            {
                Movie = new Movie { DateReleased = DateTime.Now };
            }
            else
            {
                Movie = await dataAccess.GetMovieById(MovieId);
            }
        }

        protected async Task HandleValidSubmit()
        {
            Saved = false;

            if (string.IsNullOrEmpty(Movie.MovieId)) //new
            {
                var Result = await dataAccess.AddMovie(Movie);
                if (Result.Success)
                {
                    StatusClass = "alert-success";
                    Message = "New movie added successfully.";
                    Saved = true;
                }
                else
                {
                    StatusClass = "alert-danger";
                    Message = Result.Error;
                    Saved = false;
                }
            }
            else
            {
                await dataAccess.UpdateMovie(Movie);
                StatusClass = "alert-success";
                Message = "Movie updated successfully.";
                Saved = true;
            }
        }

        protected void HandleInvalidSubmit()
        {
            StatusClass = "alert-danger";
            Message = "There are some validation errors. Please try again.";
        }

        protected async Task DeleteMovie()
        {
            await dataAccess.DeleteMovie(Movie.MovieId);

            StatusClass = "alert-success";
            Message = "Deleted successfully";

            Saved = true;
        }

        protected void NavigateToOverview()
        {
            NavigationManager.NavigateTo("/movieoverview");
        }
    }
}
