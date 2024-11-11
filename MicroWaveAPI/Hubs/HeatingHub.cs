using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using MicroWaveAPI.Dtos;

namespace MicroWaveAPI.Hubs;

public class HeatingHub : Hub
{
    // Lista concorrente para armazenar os processos de aquecimento
    private static readonly ConcurrentDictionary<string, HeatingProcessDto> HeatingProcesses = new();

    // Inicia o processo de aquecimento
    public async Task StartHeating(int power, int timeInSeconds, char charIndicator)
    {
        var connectionId = Context.ConnectionId;


        // Verifica se já existe um processo em andamento para esse cliente
        if (HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "Já existe um processo de aquecimento em andamento.");
            return;
        }
        
        // Verifica a potencia informada
        if (power is > 10 or < 1)
        {
            await Clients.Caller.SendAsync("ReceiveError", "A potencia deve ser entre 1 e 10");
            return;
        }
        
        // Verifica o tempo informado
        if (timeInSeconds is > 120 or < 1)
        {
            await Clients.Caller.SendAsync("ReceiveError", "O tempo de aquecimento deve ser maior que 1 segundo e menor que 2 minutos");
            return;
        }
        
        
        var heatingProcess = new HeatingProcessDto
        {
            Power = power,
            TimeRemaining = timeInSeconds,
            IsPaused = false,
            IsCanceled = false,
            StringIndicator = new string(charIndicator, power)
        };

        HeatingProcesses[connectionId] = heatingProcess;

        await ProcessHeating(connectionId, heatingProcess);
    }

  
    // Pausa o processo
    public async Task PauseHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "Nenhum processo de aquecimento em andamento.");
            return;
        }

        var heatingProcess = HeatingProcesses[connectionId];
        heatingProcess.IsPaused = true;

        await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, heatingProcess.TimeRemaining, "Pausado");
    }

    // Cancela o processo de aquecimento
    public async Task CancelHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "Nenhum processo de aquecimento em andamento.");
            return;
        }

        var heatingProcess = HeatingProcesses[connectionId];
        heatingProcess.IsCanceled = true;

        await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, 0, "Cancelado");
        HeatingProcesses.TryRemove(connectionId, out _); // Remove o processo da lista
    }

    //Inicio Rapido e incremento de tempo
    public async Task QuickStartHeating()
    {
        var connectionId = Context.ConnectionId;
        if (HeatingProcesses.ContainsKey(connectionId))
        {
            var heatingProcess = HeatingProcesses[connectionId];
            heatingProcess.TimeRemaining += 30;
            await ProcessHeating(connectionId, heatingProcess);
            return;
        }

        await StartHeating(10, 30, '.');
    }
    
    // Reinicia o processo de aquecimento
    public async Task RestartHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "Nenhum processo de aquecimento foi pausado.");
            return;
        }

        var heatingProcess = HeatingProcesses[connectionId];

        // Se o processo foi pausado, reinicia a partir de onde parou
        if (heatingProcess.IsPaused)
        {
            heatingProcess.IsPaused = false;
            await ProcessHeating(connectionId, heatingProcess);
        }
        else
        {
            await Clients.Caller.SendAsync("ReceiveError", "Não há processo pausado para reiniciar.");
        }
    }

    // Processa o aquecimento (controle de tempo e atualização de estado)
    private async Task ProcessHeating(string connectionId, HeatingProcessDto heatingProcess)
    {
        for (var remainingTime = heatingProcess.TimeRemaining; remainingTime > 0; remainingTime--)
        {
            if (heatingProcess.IsCanceled)
            {
                await Clients.Caller.SendAsync("HeatingFinished", "Aquecimento cancelado.");
                return;
            }

            if (heatingProcess.IsPaused)
            {
                await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, remainingTime, "Pausado");
                return; 
            }
            
            // Envia a atualização de status para o cliente que iniciou o processo
            await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, remainingTime, heatingProcess.StringIndicator, "Aquecendo");

            await Task.Delay(1000); 
        }

  
        await Clients.Caller.SendAsync("HeatingFinished", "Aquecimento concluído!");
        
        HeatingProcesses.TryRemove(connectionId, out _); 
    }
}