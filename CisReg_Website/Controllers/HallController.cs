using CisReg_Website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using static CisReg_Website.Models.HallModel;
using MongoDB.Driver;

namespace CisReg_Website.Controllers
{
    public class HallController : Controller
    {
        private readonly IMongoCollection<HallModel> _Halls;

        public HallController(IMongoDatabase database)
        {
            _Halls = database.GetCollection<HallModel>("Halls"); // Aqui acessamos a coleção "Halls"
        }
        public async Task<IActionResult> UpdateAgreement(string cnpj, int newAgreement)
        {
            try
            {
                // Busca o hall (município) pelo CNPJ
                var filter = Builders<HallModel>.Filter.Eq(h => h.CNPJ, cnpj);
                var update = Builders<HallModel>.Update.Set(h => h.Agreement, newAgreement);

                // Executa a atualização
                var result = await _Halls.UpdateOneAsync(filter, update);

                if (result.ModifiedCount > 0)
                {
                    TempData["AlertMessage"] = "Acordo atualizado com sucesso!";
                }
                else
                {
                    TempData["AlertMessage"] = "Nenhuma alteração foi feita. Verifique o CNPJ.";
                }

                return RedirectToAction("Index"); // Redireciona para a página principal
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe uma mensagem
                TempData["AlertMessage"] = $"Erro ao atualizar o acordo: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
            public async Task<IActionResult> IndexAsync()
        {
            try
            {
                // Consulta para pegar todos os halls (municípios)
                var listaHalls = await _Halls.Find(_ => true).ToListAsync();

                // Verifica se encontrou algum município
                if (listaHalls.Count == 0)
                {
                    TempData["AlertMessage"] = "Nenhum município encontrado.";
                }
                else
                {
                    TempData["AlertMessage"] = $"Carregados {listaHalls.Count} municípios com sucesso!";
                }

                return View(listaHalls);
            }
            catch (Exception ex)
            {
                // Em caso de erro, mostra uma mensagem de erro
                TempData["AlertMessage"] = $"Erro ao buscar municípios: {ex.Message}";
                return View(new List<HallModel>());
            }
        }


        // Exibe a tela de gerenciamento de atribuição
        private static List<HallModel> _hall = new List<HallModel>
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
        public IActionResult Index1()
        {
            var hallsWithVagas = _hall.Select(hall => new
            {
                hall.Address,
                hall.CNPJ,
                hall.CNES,
                hall.Agreement,
                 // Chama a função para calcular as vagas não utilizadas
            }).ToList();

            return View(hallsWithVagas);
        }


    }
}
