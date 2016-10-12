using System.Linq;
using System.Web.Mvc;
using CarRental.Data.App_Data;
using CarRental.Data.Infastructure;
using CarRental.Data.Repositories;
using CarRental.Web.Models.ViewModel;

namespace CarRental.Web.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IBaseRepository<Car> _carRepository;
        private readonly IBaseRepository<Stock> _stockRepository;

        public HomeController(IBaseRepository<Car> carRepository, IBaseRepository<Stock> stockRepository,
            IBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _carRepository = carRepository;
            _stockRepository = stockRepository;
        }

        public ActionResult Index()
        {
            /*Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Car, CarViewModel>();
            });
            */

            var car = _carRepository.GetAll()
                .OrderByDescending(m => m.CreatedDate)
                .Select(c => new CarViewModel
                {
                    Id = c.ID,
                    Title = c.Title,
                    Description = c.Description,
                    City = c.City,
                    County = c.County,
                    Location = c.Location,
                    ImageUrl = c.Image,
                    GetStock = c.Stocks,
                    Rating = c.Rating
                }).ToList();

            // _stockRepository.FindBy(s => s.CarId == c.ID).Select(s => (int)s.Quantity).AsEnumerable().Count()
            /*var config = new MapperConfiguration(x => x.CreateMap<Car, CarViewModel>());
            var mapper = config.CreateMapper();
            CarViewModel model = mapper.Map<CarViewModel>(car);*/
            /**/

            return View(car);
        }

        public ActionResult Detail(int Id)
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}