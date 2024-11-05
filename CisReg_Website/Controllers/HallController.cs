using CisReg_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CisReg_Website.Controllers
{
    public class HallController : Controller
    {
        // Simulação de dados
        private static List<HallModel> _halls = new List<HallModel>
    {
        new HallModel
        {
            CNPJ = "12345678000199",
            CNES = 123456,
            Agreement = 20,
            Address = new Address
            {
                City = "São Paulo",
                Zipcode = "01000-000",
                StateName = "SP",
                StreetName = "Rua Exemplo",
                Number = "100",
                Neighborhood = "Centro"
            }
        },
        new HallModel
        {
            CNPJ = "98765432000199",
            CNES = 654321,
            Agreement = 15,
            Address = new Address
            {
                City = "Rio de Janeiro",
                Zipcode = "20000-000",
                StateName = "RJ",
                StreetName = "Avenida Exemplo",
                Number = "200",
                Neighborhood = "Zona Sul"
            }
        }
    };

        // Exibe a tela de gerenciamento de atribuição
        public IActionResult Index()
        {
            var hallsWithVagas = _halls.Select(hall => new
            {
                hall.Address,
                hall.CNPJ,
                hall.CNES,
                hall.Agreement,
                VagasNaoUtilizadas = GetVagasNaoUtilizadas(hall) // Chama a função para calcular as vagas não utilizadas
            }).ToList();

            return View(hallsWithVagas);
        }

        // Ação para atualizar o número do acordo
        [HttpPost]
        public IActionResult UpdateAgreement(string cnpj, int newAgreement)
        {
            // Encontra o município pelo CNPJ
            var hall = _halls.FirstOrDefault(h => h.CNPJ == cnpj);

            if (hall == null)
            {
                return NotFound("Município não encontrado.");
            }

            // Verifica se o novo acordo é válido
            if (newAgreement < hall.Agreement)
            {
                ModelState.AddModelError("", "O novo acordo não pode ser menor do que o atual.");
                return View("Index", _halls);
            }

            // Atualiza o número do acordo na lista simulada
            hall.Agreement = newAgreement;

            // Redireciona para a tela de gerenciamento após a atualização
            return RedirectToAction(nameof(Index));
        }

        // Método para calcular vagas não utilizadas
        private int GetVagasNaoUtilizadas(HallModel hall)
        {
            // Aqui você deve implementar a lógica real para calcular as vagas não utilizadas
            // Exemplo fictício
            return 5; // Retornar um número de exemplo
        }



        // GET: HallController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HallController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HallController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HallController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HallController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HallController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HallController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
