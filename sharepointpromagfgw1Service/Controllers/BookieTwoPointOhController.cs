using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using sharepointpromagfgw1Service.DataObjects;
using sharepointpromagfgw1Service.Models;

namespace sharepointpromagfgw1Service.Controllers
{
    public class BookieTwoPointOhController : TableController<BookieTwoPointOh>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            sharepointpromagfgw1Context context = new sharepointpromagfgw1Context();
            DomainManager = new EntityDomainManager<BookieTwoPointOh>(context, Request, Services);
        }

        // GET tables/BookieTwoPointOh
        public IQueryable<BookieTwoPointOh> GetAllBookieTwoPointOh()
        {
            return Query(); 
        }

        // GET tables/BookieTwoPointOh/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<BookieTwoPointOh> GetBookieTwoPointOh(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/BookieTwoPointOh/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<BookieTwoPointOh> PatchBookieTwoPointOh(string id, Delta<BookieTwoPointOh> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/BookieTwoPointOh
        public async Task<IHttpActionResult> PostBookieTwoPointOh(BookieTwoPointOh item)
        {
            BookieTwoPointOh current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/BookieTwoPointOh/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteBookieTwoPointOh(string id)
        {
             return DeleteAsync(id);
        }

    }
}