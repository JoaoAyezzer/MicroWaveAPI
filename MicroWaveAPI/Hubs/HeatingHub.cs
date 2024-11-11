using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using MicroWaveAPI.Contracts;
using MicroWaveAPI.Dtos;

namespace MicroWaveAPI.Hubs;

public class HeatingHub(IHeatingModeService heatingModeService) : Hub
{
    // Lista concorrente para armazenar os processos de aquecimento
    private static readonly ConcurrentDictionary<string, HeatingProcessDto> HeatingProcesses = new();

    public async Task StartDefault(int power, int timeInSeconds)
    {
        // Verifica o tempo informado
        if (timeInSeconds is > 120 or < 1)
        {
            await Clients.Caller.SendAsync("ReceiveError", "Heating time must be greater than 1 second and less than 2 minutes");
            return;
        }
        var heatingProcess = new HeatingProcessDto
        {
            Power = power,
            TimeRemaining = timeInSeconds,
            IsPaused = false,
            IsCanceled = false,
            StringIndicator = new string('.', power),
            AllowsAdditionalTime = true
        };
        await StartHeating(heatingProcess);
    }

    public async Task StartRegisteredHeatingMode(long modeId)
    {
        var heatingMode = await heatingModeService.GetByIdAsync(modeId);
        var heatingProcess = new HeatingProcessDto
        {
            Power = heatingMode.Power,
            TimeRemaining = heatingMode.Time,
            IsPaused = false,
            IsCanceled = false,
            StringIndicator = new string(heatingMode.CharIndicator, heatingMode.Power),
            AllowsAdditionalTime = false
        };
        await StartHeating(heatingProcess);
    }

    // Inicia o processo de aquecimento
    private async Task StartHeating(HeatingProcessDto heatingProcess)
    {
        var connectionId = Context.ConnectionId;
        
        // Verifica se já existe um processo em andamento para esse cliente
        if (HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "A heating process is already in progress.");
            return;
        }
        
        // Verifica a potencia informada
        if (heatingProcess.Power is > 10 or < 1)
        {
            await Clients.Caller.SendAsync("ReceiveError", "Power must be between 1 and 10");
            return;
        }

        HeatingProcesses[connectionId] = heatingProcess;

        await ProcessHeating(connectionId, heatingProcess);
    }

    // Pausa o processo
    public async Task PauseHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "No heating process in progress.");
            return;
        }

        var heatingProcess = HeatingProcesses[connectionId];
        heatingProcess.IsPaused = true;

        await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, heatingProcess.TimeRemaining, "Paused");
    }

    // Cancela o processo de aquecimento
    public async Task CancelHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "No heating process in progress.");
            return;
        }

        var heatingProcess = HeatingProcesses[connectionId];
        heatingProcess.IsCanceled = true;

        await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, 0, "Canceled");
        HeatingProcesses.TryRemove(connectionId, out _); // Remove o processo da lista
    }

    // Inicio Rápido e incremento de tempo
    public async Task QuickStartHeating()
    {
        var connectionId = Context.ConnectionId;
        if (HeatingProcesses.ContainsKey(connectionId))
        {
            var heatingProcess = HeatingProcesses[connectionId];
            if (!heatingProcess.AllowsAdditionalTime)
            {
                await Clients.Caller.SendAsync("ReceiveError", "This process cannot be modified.");
                return;
            }
            heatingProcess.TimeRemaining += 30;
            await ProcessHeating(connectionId, heatingProcess);
            return;
        }

        await StartDefault(10, 30);
    }
    
    // Reinicia o processo de aquecimento
    public async Task RestartHeating()
    {
        var connectionId = Context.ConnectionId;

        if (!HeatingProcesses.ContainsKey(connectionId))
        {
            await Clients.Caller.SendAsync("ReceiveError", "No paused heating process.");
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
            await Clients.Caller.SendAsync("ReceiveError", "There is no paused process to restart.");
        }
    }

    // Processa o aquecimento (controle de tempo e atualização de estado)
    private async Task ProcessHeating(string connectionId, HeatingProcessDto heatingProcess)
    {
        for (var remainingTime = heatingProcess.TimeRemaining; remainingTime > 0; remainingTime--)
        {
            if (heatingProcess.IsCanceled)
            {
                await Clients.Caller.SendAsync("HeatingFinished", "Heating canceled.");
                return;
            }

            if (heatingProcess.IsPaused)
            {
                await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, remainingTime, "Paused");
                return; 
            }
            
            // Envia a atualização de status para o cliente que iniciou o processo
            await Clients.Caller.SendAsync("ReceiveUpdate", heatingProcess.Power, remainingTime, heatingProcess.StringIndicator, "Heating");

            await Task.Delay(1000); 
        }

        await Clients.Caller.SendAsync("HeatingFinished", "Heating completed!");
        
        HeatingProcesses.TryRemove(connectionId, out _); 
    }
}
