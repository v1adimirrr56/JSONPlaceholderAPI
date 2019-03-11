using AutoMapper;
using JSONPlaceholderAPI.Models.Abstract;
using JSONPlaceholderAPI.Models.Infrastructure;
using JSONPlaceholderAPI.Models.Infrastructure.Hateoas;
using JSONPlaceholderAPI.Models.Mappings;
using JSONPlaceholderAPI.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UserModel = JSONPlaceholderAPI.Models.Mappings.User;

namespace JSONPlaceholderAPI.Controllers
{
    [RoutePrefix("api/albums")]
    public class AlbumsController : ApiController
    {
        private readonly HttpClient _hc = new HttpClient();
        private readonly IPlaceholderUser _externalUser;
        private readonly IPlaceholderAlbum _externalAlbum;
        private readonly IPagination<AlbumViewModel> _pagination;

        public AlbumsController(IPagination<AlbumViewModel> pagination,
                       IPlaceholderUser placeholderUser,
                       IPlaceholderAlbum placeholderAlbum)
        {
            _pagination = pagination;
            _externalUser = placeholderUser;
            _externalAlbum = placeholderAlbum;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAlbums()
       {
            var asyncAlbums = await _hc.GetAsync(_externalAlbum.GetLink());
            var asyncUsers = await _hc.GetAsync(_externalUser.GetLink());

            if (!asyncAlbums.IsSuccessStatusCode
                || !asyncAlbums.IsSuccessStatusCode)
            {
                return Conflict();
            }

            var allAlbums = await asyncAlbums
                .Content
                .ReadAsAsync<IEnumerable<Album>>();
            var allUsers = await asyncUsers
                .Content
                .ReadAsAsync<IEnumerable<UserModel>>();

            var mappedUserViewModel = Mapper.Map<IEnumerable<UserModel>, IEnumerable<UserViewModel>>(allUsers);
            var mappedAlbumViewModel = Mapper.Map<IEnumerable<Album>, IEnumerable<AlbumViewModel>>(allAlbums);
            foreach (var item in mappedAlbumViewModel)
            {
                item.User = mappedUserViewModel
                    .FirstOrDefault(x => x.Id == item.UserId);
            }
            
            foreach (var item in mappedAlbumViewModel)
            {
                item.Links = new List<LinkHATEOAS>() {
                    CreatorHateoas.CreateHateoas("self",
                    $"{Url.Link("DefaultApi",new { id = item.Id})}",
                    "GET")
                };
            }
            return Ok(mappedAlbumViewModel);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAlbumsPaging(int? pageSize, int? page = 1)
        {
            if (pageSize == null || pageSize < 0
                || page < 0 || page == null)
            {
                return BadRequest();
            }
            var asyncRequset = await GetAlbums();
            var asyncAlbums = await asyncRequset
                .ExecuteAsync(CancellationToken.None);
            if (!asyncAlbums.IsSuccessStatusCode)
            {
                return Conflict();
            }
            var allAlbums = await asyncAlbums
                .Content
                .ReadAsAsync<IEnumerable<AlbumViewModel>>();

            return Ok(_pagination.GetPaging(allAlbums, (int)pageSize, (int)page));
        }

        [Route("{id:long:min(1)}")]
        public async Task<IHttpActionResult> GetAlbum(long id)
        {
            var requestResult = await _hc.GetAsync(_externalAlbum.GetLink());

            if (!requestResult.IsSuccessStatusCode)
            {
                return Conflict();
            }
            var data = await requestResult
                .Content
                .ReadAsAsync<IEnumerable<Album>>();

            var dataById = data.FirstOrDefault(x => x.Id == id);
            if (dataById == null)
            {
                return NotFound();
            }
            var mappedAlbumViewModel = Mapper.Map<Album, AlbumViewModel>(dataById);
            mappedAlbumViewModel.Links = new List<LinkHATEOAS>() {
                     CreatorHateoas.CreateHateoas("self",
                     $"{Request.RequestUri.AbsoluteUri}",
                     "GET") };
            return Ok(mappedAlbumViewModel);
        }
    }
}
