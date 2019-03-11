using AutoMapper;
using JSONPlaceholderAPI.Models.Abstract;
using JSONPlaceholderAPI.Models.Infrastructure;
using JSONPlaceholderAPI.Models.Infrastructure.Hateoas;
using JSONPlaceholderAPI.Models.Mappings;
using JSONPlaceholderAPI.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using UserModel = JSONPlaceholderAPI.Models.Mappings.User;

namespace JSONPlaceholderAPI.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly static HttpClient _hc = new HttpClient();
        private readonly IPagination<UserViewModel> _pagination;
        private readonly IPlaceholderUser _externalUser;
        private readonly IPlaceholderAlbum _externalAlbum;

        public UsersController(IPagination<UserViewModel> pagination,
                               IPlaceholderUser placeholderUser,
                               IPlaceholderAlbum placeholderAlbum)
        {
            _pagination = pagination;
            _externalUser = placeholderUser;
            _externalAlbum = placeholderAlbum;
        }
        [HttpGet]
        public async Task<IHttpActionResult> GetUsers()
        {
            var requestResult = await _hc.GetAsync(_externalUser.GetLink());
            if (!requestResult.IsSuccessStatusCode)
            {
                return Conflict();
            }

            var data = await requestResult
                .Content
                .ReadAsAsync<IEnumerable<UserModel>>();

            var mappedUserViewModel = Mapper.Map<IEnumerable<UserModel>, IEnumerable<UserViewModel>>(data);
            foreach (var item in mappedUserViewModel)
            {
                item.Links = new List<LinkHATEOAS>() {
                    CreatorHateoas.CreateHateoas("self", 
                    $"{Url.Link("DefaultApi", new { id = item.Id})}",
                    "GET")
                };
            }
            return Ok(mappedUserViewModel);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetUsersPaging(int? pageSize, int? page = 1)
        {
            if (pageSize == null || pageSize < 0
                || page < 0 || page == null)
            {
                return BadRequest();
            }

            var requestResult = await _hc.GetAsync(_externalUser.GetLink());
            if (!requestResult.IsSuccessStatusCode)
            {
                return Conflict();
            }

            var data = await requestResult
                .Content
                .ReadAsAsync<IEnumerable<UserModel>>();

            var mappedUserViewModel = Mapper.Map<IEnumerable<UserModel>, IEnumerable<UserViewModel>>(data);
            foreach (var item in mappedUserViewModel)
            {
                item.Links = new List<LinkHATEOAS>() {
                    CreatorHateoas.CreateHateoas("self",
                    $"{Url.Link("DefaultApi", new { id = item.Id})}",
                    "GET")
                };
            }

            return Ok(_pagination.GetPaging(mappedUserViewModel, (int)pageSize, (int)page));
        }

        [HttpGet]
        [Route("{id:long:min(1)}")]
        public async Task<IHttpActionResult> GetUsers(long id)
        {
            var requestResult = await _hc.GetAsync(_externalUser.GetLink());
            
            if (!requestResult.IsSuccessStatusCode)
            {
                return Conflict();
            }
            var data = await requestResult
                .Content
                .ReadAsAsync<IEnumerable<UserModel>>();

            var dataById = data.FirstOrDefault(x => x.Id == id);
            if (dataById == null)
            {
                return NotFound();
            }
            var mappedUserViewModel = Mapper.Map<UserModel, UserViewModel>(dataById);
            mappedUserViewModel.Links = new List<LinkHATEOAS>() {
                     CreatorHateoas.CreateHateoas("self",
                     $"{Request.RequestUri.AbsoluteUri}",
                     "GET") };
            return Ok(mappedUserViewModel);
        }

        [HttpGet]
        [Route("{id:long:min(1)}/albums")]
        public async Task<IHttpActionResult> GetUserAlbums(long id)
        {
            var asyncAlbums = await _hc.GetAsync(_externalAlbum.GetLink());

            if (!asyncAlbums.IsSuccessStatusCode)
            {
                return Conflict();
            }

            var allAlbums = await asyncAlbums
                .Content
                .ReadAsAsync<IEnumerable<Album>>();

            var filterById = allAlbums.Where(x => x.UserId == id);
            foreach (var item in filterById)
            {
                item.Links = new List<LinkHATEOAS>() {
                    CreatorHateoas.CreateHateoas("self",
                    $"{Url.Link("DefaultApi",new { id = item.Id})}",
                    "GET")
                };
            }

            var mappedAlbumViewModel = Mapper.Map<IEnumerable<Album>, IEnumerable<AlbumInfoViewModel>>(filterById);
            return Ok(mappedAlbumViewModel);
        }
    }
}
