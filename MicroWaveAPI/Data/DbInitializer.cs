using MicroWaveAPI.Contracts;
using MicroWaveAPI.Models;

namespace MicroWaveAPI.Data;

public class DbInitializer(ApplicationDbContext context) : IDbInitializer
{
    public void Initialize()
    { 
        context.Database.EnsureCreated();

        if (!context.HeatingModeCollection.Any())
        {
            context.HeatingModeCollection.AddRange(GetHeatingModesDefault());
        }

        if (!context.MicrowaveCollection.Any())
        {
            context.MicrowaveCollection.AddRange(GetMicrowavesDefault());
        }

        context.SaveChanges();
    }
    private IEnumerable<HeatingMode> GetHeatingModesDefault()
    {
        return new List<HeatingMode>
        {
            new HeatingMode
            {
                Id = 1,
                Name = "Pipoca",
                Food = "Pipoca (de micro-ondas)",
                Time = 180,
                Power = 7,
                Instructions =
                    "Observar o barulho de estouros do milho, caso houver um intervalo de mais de 10 segundos entre um estouro e outro, interrompa o aquecimento."
            },
            new HeatingMode
            {
                Id = 2,
                Name = "Leite",
                Food = "Leite",
                Time = 300,
                Power = 5,
                Instructions =
                    "Cuidado com aquecimento de líquidos, o choque térmico aliado ao movimento do recipiente pode\ncausar fervura imediata causando risco de queimaduras."
            },
            new HeatingMode
            {
                Id = 3,
                Name = "Carnes de boi",
                Food = "Carne em pedaço ou fatias",
                Time = 840,
                Power = 4,
                Instructions =
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."
            },
            new HeatingMode
            {
                Id = 4,
                Name = "Frango",
                Food = "Frango (qualquer corte)",
                Time = 480,
                Power = 7,
                Instructions =
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."
            },
            new HeatingMode
            {
                Id = 5,
                Name = "Feijão",
                Food = "Feijão congelado",
                Time = 480,
                Power = 9,
                Instructions =
                    "Interrompa o processo na metade e vire o conteúdo com a parte de baixo para cima para o descongelamento uniforme."
            }

        };

        
    }
    private IEnumerable<Microwave> GetMicrowavesDefault()
    {
        return new List<Microwave>
        {
            new Microwave { Id = 1, Name = "Microondas Teste" }
        };
    }
}