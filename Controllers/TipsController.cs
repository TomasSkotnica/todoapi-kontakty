using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Docs.Samples;
using todoapi.Models;
using TodoApi.Models;

namespace todoapi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class TipsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TipsController(TodoContext context)
        {
            _context = context;
        }

        [HttpGet("/q/")]
        public async Task<ActionResult<string>> GetQ([FromQuery] string param1, [FromQuery] string param2)
        {
            // Získání hodnot parametrů z query
            string value1 = HttpContext.Request.Query["param1"];
            if (string.IsNullOrEmpty(value1)) { value1 = string.Empty; }
            string value2 = HttpContext.Request.Query["param2"];
            if (string.IsNullOrEmpty(value2)) { value2 = string.Empty; }
            return $"values are {value1}, {value2}";
        }


        [HttpGet("/getgens/")]
        public async Task<ActionResult<IEnumerable<Generation>>> GetGenerations()
        {
            List<Generation> output = new();
            output.Add(new Generation { Id = 1, Name = "Sales" });
            output.Add(new Generation { Id = 2, Name = "MES" });

            return output;
        }

        [HttpGet("/getrels/")]
        public async Task<ActionResult<IEnumerable<Release>>> GetReleases()
        {
            List<Release> output = new();
            output.Add(new Release { GenId = 1, ReleaseName = "MR39-14.0.0" });
            output.Add(new Release { GenId = 2, ReleaseName = "21.2.0" });
            output.Add(new Release { GenId = 2, ReleaseName = "21.0.0" });

            return output;
        }

        // todo, kdyz nezadam id, tak to musi vratit nejakou smysluplnou odezvu, abz to nekraslo
        [HttpGet("/getrelsofgen/{id}")]
        public async Task<ActionResult<IEnumerable<Release>>> GetReleasesOfGeneration(int id)
        {
            List<Release> all = new();
            all.Add(new Release { GenId = 1, ReleaseName = "MR39-14.0.0" });
            all.Add(new Release { GenId = 2, ReleaseName = "21.2.0" });
            all.Add(new Release { GenId = 2, ReleaseName = "21.0.0" });


            return all.Where(i => i.GenId == id).ToList();
        }

        [HttpGet("Throw")]
        public IActionResult Throw() => throw new Exception("Sample exception.");

        /*
         response bodz is like that"
        System.Exception: Sample exception.
   at todoapi.Controllers.TipsController.Throw() in C:\work\MS\todoapi\Controllers\TipsController.cs:line 54
   at lambda_method2(Closure, Object, Object[])
   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.SyncActionResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.InvokeActionMethodAsync()
   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)
....
         */
        /*
        // Postman https://localhost:7150/error vraci hodnoty z Problem();
        [Route("/error")]
        public IActionResult HandleError() => Problem(detail: "Muj detail", statusCode: 555, title: "Muj titulek");
        */
/*
        [Route("/error-dev")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult HandleErrorDevelopment([FromServices] IHostEnvironment hostEnvironment)
        {
            if (!hostEnvironment.IsDevelopment()) { return NotFound(); }

            var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            // exceptionHandlerFeature je null, nevim proc
            return Problem(
                detail: exceptionHandlerFeature.Error.StackTrace,
                title: exceptionHandlerFeature.Error.Message);
        }
*/
    }

}
