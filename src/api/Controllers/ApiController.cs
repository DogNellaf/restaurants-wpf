using Microsoft.AspNetCore.Mvc;
using RestaurantsClasees.OrderSystem;
using RestaurantsClasses;
using RestaurantsClasses.KontragentsSystem;
using RestaurantsClasses.OnlineSystem;
using RestaurantsClasses.WorkersSystem;

namespace RestaurantsDataApi.Controllers
{
    [Route("[controller]/[action]")]
    public class ApiController : ControllerBase
    {

        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        public IList<Meal> GetMeals()
        {
            return Database.GetObject<Meal>();
        }


        public IEnumerable<Ingredient> GetIngredients()
        {
            return Database.GetObject<Ingredient>();
        }

        public IEnumerable<Ingredient> GetIngredientsByMeal(int meal_id)
        {
            var meal = Database.GetObject<Meal>($"id = {meal_id}").FirstOrDefault();

            if (meal is null)
                return new List<Ingredient>();

            return meal.GetIngredients().Select(x => x.Key);
        }

        public bool Auth(string username, string password, bool isClient = false)
        {
            if (isClient)
            {
                var client = Database.GetObject<Client>($"username = {username}").FirstOrDefault();

                if (client is null)
                    return false;

                return Encoder.CheckHash(password, client.Password);
            }
            else
            {
                var worker = Database.GetObject<Worker>($"username = '{username}'").FirstOrDefault();

                if (worker is null)
                    return false;

                return Encoder.CheckHash(password, worker.Password);
            }
        }
    }
}