using System;
using System.Linq;
using System.Threading.Tasks;
using AspNet.Security.OAuth.Validation;
using Jahshaka.API.Constants;
using Jahshaka.API.ViewModels.Collection;
using Jahshaka.API.ViewModels.Mappers;
using Jahshaka.API.ViewModels.Shared;
using Jahshaka.Core.Data;
using Jahshaka.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Jahshaka.API.Controllers.Admin
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("admin/collections")]
    public class CollectionController : BaseController
    {
        private readonly ApplicationDbContext _appDbContext;
        protected ILogger _logger;
        
        public CollectionController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
            _logger = loggerFactory.CreateLogger<CollectionController>();
        }

        [HttpGet, Route("")]
        public async Task<IActionResult> Index(int? page = null)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || !user.UserType.Equals(UserType.Admin))
                {
                    return Unauthorized();
                }

                int pageNumber = page ?? 1;
                int pageSize = 20;

                var queryable = _dbContext.Collections
                    .Where(c => c.CollectionId==null)
                    .OrderByDescending(c => c.CreatedAt);  
                    
                var total = queryable.Count();

                var pageResult = queryable.Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var model = new PagedListViewModel<CollectionViewModel>()
                {
                    Items = pageResult.ToViewModel(),

                    Paging = new PagingOptionsViewModel()
                    {
                        CurrentPage = pageNumber,
                        TotalItems = total,
                        PageSize = pageSize
                    }
                };

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { 
                    Error = ErrorCode.ServerError, 
                    ErrorDescription = GetEnvironmentErrorMessage(ex) 
                });
            }
        
        }

        [HttpGet, Route("all")]
        public async Task<IActionResult> All()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || !user.UserType.Equals(UserType.Admin))
                {
                    return Unauthorized();
                }

                var queryable = _dbContext.Collections
                    .Include(c => c.Collections)
                    .AsEnumerable()
                    .Where(c => c.CollectionId==null)
                    .OrderByDescending(c => c.CreatedAt)
                    .ToList();  

                return Ok(queryable.ToViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { 
                    Error = ErrorCode.ServerError, 
                    ErrorDescription = GetEnvironmentErrorMessage(ex) 
                });
            }
        }

        [HttpPost, Route("create")]
        public async Task<IActionResult> Create([FromBody] CreateCollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    if (user == null || !user.UserType.Equals(UserType.Admin))
                    {
                        return Unauthorized();
                    }

                    var collection = new Collection(){
                        Name = model.Name,
                        CreatedAt = DateTime.UtcNow
                    };

                    if(model.CollectionId != null)
                    {
                        var collectionParent = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(model.CollectionId));

                        if(collectionParent == null)
                        {
                            throw new Exception("Parent collection not found");
                        }

                        collection.CollectionId = collectionParent.Id;
                    }
                    
                    _dbContext.Collections.Add(collection);

                    await _dbContext.SaveChangesAsync();

                    return Ok(collection.ToViewModel());
                    
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = ex.Message
                    });
                }

            }
            
            return BadRequest(new ErrorViewModel()
            {
                Error = ErrorCode.ModelError,
                ErrorDescription = ModelState?.GetFirstError()
            });
            
        }

        /*[HttpGet, Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
                    .ToList(); 
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null || !user.UserType.Equals(UserType.Admin))
                {
                    return Unauthorized();
                }

                var queryable = _dbContext.Collections
                    .Include(c => c.Collections)
                    .Where(c => c.CollectionId==null); 

                return Ok(queryable.ToViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel { 
                    Error = ErrorCode.ServerError, 
                    ErrorDescription = GetEnvironmentErrorMessage(ex) 
                });
            }
        }*/


        [HttpPost, Route("{id}/update")]
        public async Task<IActionResult> Update(int id, CreateCollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    if (user == null || !user.UserType.Equals(UserType.Admin))
                    {
                        return Unauthorized();
                    }

                    var collection = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(id));

                    if(collection == null)
                    {
                        return BadRequest(new ErrorViewModel()
                        {
                            Error = ErrorCode.ModelError,
                            ErrorDescription = "Collection not found"
                        });
                    }

                    collection.Name = model.Name;
                    
                    _dbContext.Collections.Add(collection);

                    await _dbContext.SaveChangesAsync();

                    return Ok(collection.ToViewModel());
                    
                }
                catch (Exception ex)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = ex.Message
                    });
                }

            }
            
            return BadRequest(new ErrorViewModel()
            {
                Error = ErrorCode.ModelError,
                ErrorDescription = ModelState?.GetFirstError()
            });
            
        }

        /*[HttpPost, Route("{int:id}/add/{int:collection_id}")]
        public async Task<IActionResult> AddCollection(int id, int collection_id)
        {
           var user = await _userManager.GetUserAsync(User);
                    
            if (user == null || !user.UserType.Equals(UserType.Admin))
            {
                return Unauthorized();
            }

            var collectionParent = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(id));

            if(collectionParent == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "Parent collection not found"
                });
            }

            var collectionChild = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(collection_id));

            if(collectionChild == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "Child collection not found"
                });
            }

            collectionParent.Collections.Add(collectionChild);

            await _dbContext.SaveChangesAsync();

            return Ok(collectionParent.ToViewModel());
            
        }

        [HttpPost, Route("{int:id}/remove")]
        public async Task<IActionResult> RemoveCollection(int id)
        {
           var user = await _userManager.GetUserAsync(User);
                    
            if (user == null || !user.UserType.Equals(UserType.Admin))
            {
                return Unauthorized();
            }

            var collection = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(id));

            if(collection == null)
            {
                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = "Collection not found"
                });
            }

            collection.CollectionId = null;

            await _dbContext.SaveChangesAsync();

            return Ok(collection.ToViewModel());
            
        }*/
    }
}