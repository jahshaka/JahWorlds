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

namespace Jahshaka.API.Controllers
{
    [Authorize(AuthenticationSchemes = OAuthValidationDefaults.AuthenticationScheme)]
    [Route("collections")]
    public class CollectionController : BaseController
    {
        private readonly ApplicationDbContext _appDbContext;
        
        public CollectionController(
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext appDbContext,
            IHostingEnvironment environment,
            ILoggerFactory loggerFactory
        ) : base(appDbContext, loggerFactory, environment, userManager)
        {
            _logger = loggerFactory.CreateLogger<CollectionController>();
        }

        [HttpGet, Route("all")]
        public async Task<IActionResult> All()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized();
                }

                var queryable = _dbContext.Collections
                    .Include(c => c.Collections)
                    .AsEnumerable()
                    .Where(c => c.CollectionId==null && c.UserId.Equals(user.Id))
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
                    
                    if (user == null)
                    {
                        return Unauthorized();
                    }

                    var collection = new Collection(){
                        Name = model.Name,
                        CreatedAt = DateTime.UtcNow,
                        UserId = user.Id
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

        [HttpPost, Route("{id}/rename")]
        public async Task<IActionResult> Rename(int id, [FromBody] RenameCollectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    
                    if (user == null)
                    {
                        return Unauthorized();
                    }

                    var collection = _dbContext.Collections.FirstOrDefault(c => c.Id.Equals(id) && c.UserId.Equals(user.Id));

                    if(collection == null)
                    {
                        return BadRequest(new ErrorViewModel()
                        {
                            Error = ErrorCode.ModelError,
                            ErrorDescription = "Collection not found"
                        });
                    }
                    
                    collection.Name = model.Name;
                    
                    _dbContext.Update(collection);

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

        [HttpDelete, Route("{id}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            
            try 
            {

                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return Unauthorized();
                }

                var collection = _dbContext.Collections
                    .Include(c => c.Collections)
                    .Include(c => c.Assets)
                    .AsEnumerable()
                    .FirstOrDefault(a => a.Id == id && a.UserId.Equals(user.Id));

                if (collection == null)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Collection not found."
                    });
                }

                if(collection.Assets.Count() > 0 || collection.Collections.Count() > 0)
                {
                    return BadRequest(new ErrorViewModel()
                    {
                        Error = ErrorCode.ModelError,
                        ErrorDescription = "Cannot delete collection with assets/collections."
                    });
                }

                _dbContext.Remove(collection);

                await _dbContext.SaveChangesAsync();

                return Ok();

            }
            catch (Exception ex)
            {

                _logger.LogCritical(ex.Message);

                return BadRequest(new ErrorViewModel()
                {
                    Error = ErrorCode.ModelError,
                    ErrorDescription = ex.Message
                });
            }     

        }

    }
}

        