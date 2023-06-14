using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Ilogger = Serilog.ILogger;
namespace CRMSongStudio.Controllers
{
    
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly Ilogger _logger;
        public SongsController() {
            _logger = Log.Logger;
        }

        private static List<Song> songs = new List<Song>() {
            new Song(){
                Id = 0,
                Name = "Ex's Hate Me",
                Description = "All my ex's hate me",
                Price = 5000,
                Author = "B Bray"
            }
        };
        [HttpGet]
        [Route("/api/[controller]/get-all-song")]
        public IActionResult GetAllSong() {
            return Ok(songs);
        }
        [HttpPost]
        [Route("/api/[controller]/add-song")]
        public IActionResult AddSong([FromBody]Song song) {
            _logger.Information("Start add new song");
            var existSong = songs.Where(x => x.Id == song.Id).FirstOrDefault();
            if (existSong != null)
            {
                return BadRequest($"Song has Id = {song.Id} is existed ");
            }
            songs.Add(song);
            var jsonSong = JsonConvert.SerializeObject(song);
            _logger.Information(jsonSong);
            return Ok(songs);
        }
        [HttpPut]
        [Route("/api/[controller]/update-song")]
        public IActionResult UpdateSong([FromBody]Song song) {
            _logger.Information("Update Song");
            var existSong = songs.Where(x => x.Id == song.Id).FirstOrDefault();
            if (existSong == null) {
                return BadRequest($"A song is not existed");

            }
            //for (int i = 0; i < songs.Count(); i++) {
            //    if (song.Id == songs[i].Id) { 
            //        songs[i].Name = song.Name;
            //        songs[i].Author = song.Author;
            //        songs[i].Description = song.Description;
            //        songs[i].Price = song.Price;
            //    }
            //}
            var songUpdate = songs.Where(x => x.Id == song.Id).SingleOrDefault();
            if (songUpdate == null) {
                return BadRequest("Song is not found");
            }
            songUpdate.Name = song.Name;
            songUpdate.Description = song.Description;
            songUpdate.Author = song.Author;
            songUpdate.Price = song.Price;
            var jsonSong = JsonConvert.SerializeObject(song);
            _logger.Information(jsonSong);
            return Ok(songs);
        }
        [HttpDelete]
        [Route("/api/[controller]/delete-car/{id}")]
        public IActionResult DeleteSong(int id) {
            var song = songs.Where(x => x.Id == id).SingleOrDefault();
            if (song == null)
            {
                return BadRequest("Song is not found");
            }
            songs.Remove(song);
            return Ok(songs);
        }
        [HttpGet]
        [Route("/api/[controller]/get-song-id/{id}")]
        public IActionResult GetSong(int id)
        {
            return Ok(songs.Select(_ => _.Id == id).FirstOrDefault());
        }
    }
    
}
