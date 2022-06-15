using AirtableApiClient;
using MovieLibrary.Services.AirTable.Models;
using MovieLibrary.Shared;

namespace MovieLibrary.Services.AirTable
{
    public class DataAccess
    {
        private const string DataAccessKey = "keylFRAzofJH2sq7W";
        readonly string baseId = "app1rzu0qJJEjiv2n";

        public async Task<List<Movie>> GetAllMovies() 
        {

            string offset = null;
            string errorMessage = null;
            var records = new List<AirtableRecord>();
            var movies = new List<Movie>();

            using (AirtableBase airtableBase = new AirtableBase(DataAccessKey, baseId))
            {
                //
                // Use 'offset' and 'pageSize' to specify the records that you want
                // to retrieve.
                // Only use a 'do while' loop if you want to get multiple pages
                // of records.
                //

                do
                {
                    Task<AirtableListRecordsResponse> task = airtableBase.ListRecords(
                           "Movies");

                    AirtableListRecordsResponse response = await task;

                    if (response.Success)
                    {
                        records.AddRange(response.Records.ToList());
                        offset = response.Offset;
                    }
                    else if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                        break;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                        break;
                    }
                } while (offset != null);
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // Error reporting
            }
            else
            {
                // Do something with the retrieved 'records' and the 'offset'
                // for the next page of the record list.
                foreach(var dto in records)
                {
                    var Movie = new Movie
                    {
                        MovieId = dto.Id,
                        Title = dto.GetField("Title").ToString(),
                        Director = dto.GetField("Director").ToString(),
                        DateReleased = DateTime.Parse(dto.GetField("DateReleased").ToString()),
                    };
                    movies.Add(Movie);
                }
                
            }

            return movies;
        }

        public async Task<ResultInfo> AddMovie(Movie movie)
        {
            var records = new List<AirtableRecord>();
            var fields = new Fields();

            fields.AddField("ID", movie.DateReleased.Year);
            fields.AddField("Title", movie.Title);
            fields.AddField("Director", movie.Director);
            fields.AddField("DateReleased", movie.DateReleased.ToShortDateString());

            using (AirtableBase airtableBase = new AirtableBase(DataAccessKey, baseId))
            {
                var addMovie = await airtableBase.CreateRecord("Movies", fields);
                return new ResultInfo
                {
                    Success = addMovie.Success,
                    Error = (addMovie.Success) ?String.Empty: addMovie.AirtableApiError.ErrorMessage
                };



                
            }

        }

        public async Task<Movie> GetMovieById(String id)
        {

            using (AirtableBase airtableBase = new AirtableBase(DataAccessKey, baseId))
            {
                Task<AirtableRetrieveRecordResponse> task = airtableBase.RetrieveRecord("Movies", id);
                var response = await task;
                if (!response.Success)
                {
                    return null;
                }
                else
                {
                    var Movie = new Movie
                    {
                        MovieId = response.Record.Id,
                        Title = response.Record.GetField("Title").ToString(),
                        Director = response.Record.GetField("Director").ToString(),
                        DateReleased = DateTime.Parse(response.Record.GetField("DateReleased").ToString()),
                    };
                    return Movie;
                }
            }
        }

        public async Task DeleteMovie(String id)
        {
            using (AirtableBase airtableBase = new AirtableBase(DataAccessKey, baseId))
            {
                Task<AirtableDeleteRecordResponse> task = airtableBase.DeleteRecord("Movies", id);
                var response = await task;
                if (!response.Success)
                {
                    string errorMessage = null;
                    if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                    }
                }
            }
        }

        public async Task UpdateMovie(Movie movie)
        {
            using (AirtableBase airtableBase = new AirtableBase(DataAccessKey, baseId))
            {
                var fields = new Fields();
                fields.AddField("ID", movie.DateReleased.Year);
                fields.AddField("Title", movie.Title);
                fields.AddField("Director", movie.Director);
                fields.AddField("DateReleased", movie.DateReleased.ToShortDateString());

                Task<AirtableCreateUpdateReplaceRecordResponse> task = airtableBase.UpdateRecord("Movies", fields, movie.MovieId);
                var response = await task;

                if (!response.Success)
                {
                    string errorMessage = null;
                    if (response.AirtableApiError is AirtableApiException)
                    {
                        errorMessage = response.AirtableApiError.ErrorMessage;
                    }
                    else
                    {
                        errorMessage = "Unknown error";
                    }
                    // Report errorMessage
                }
                else
                {
                    var record = response.Record;
                    
                    // Do something with your updated record.
                }
            }
        }
    }
}
