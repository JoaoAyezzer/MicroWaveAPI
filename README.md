# API de Microondas Digital

Esta é uma API desenvolvida com C# ASP.NET Core 8 que fornece endpoints para operações CRUD de programas de aquecimento e métodos WebSocket (usando SignalR) para controlar o estado do aquecimento (iniciar, pausar, cancelar e reiniciar).

## Tecnologias Utilizadas

- C# ASP.NET Core 8
- Entity Framework Core
- SignalR para WebSocket
- MySQL

## Funcionalidades

### Endpoints RESTful (HTTP)

A API permite operações CRUD nos programas de aquecimento, com os seguintes endpoints:

- **GET /heating**: Retorna todos os programas de aquecimento.
- **GET /heating/{id}**: Retorna um programa específico por ID.
- **POST /heating**: Cria um novo programa de aquecimento.
- **PUT /heating**: Atualiza um programa de aquecimento existente.
- **DELETE /heating/{id}**: Exclui um programa de aquecimento.

### Métodos WebSocket (SignalR)

A API usa SignalR para comunicação em tempo real e controle do estado do aquecimento dos programas. O hub WebSocket permite os seguintes comandos:

- **Iniciar Aquecimento**: Inicia o aquecimento de um programa.
- **Pausar Aquecimento**: Pausa o aquecimento em andamento.
- **Cancelar Aquecimento**: Cancela o aquecimento atual.
- **Reiniciar Aquecimento**: Reinicia o aquecimento interrompido.

Esses comandos são enviados via WebSocket e processados pelo `HeatingModeHub` para atualizar o estado do aquecimento em tempo real.

## Configuração do Ambiente

1. **Pré-requisitos**:
   - [.NET SDK 8](https://dotnet.microsoft.com/download/dotnet/8.0)
   - MySQL para o banco de dados
   
2. **Clone o repositório**:
   ```bash
   git clone https://github.com/JoaoAyezzer/MicroWaveAPI.git
   cd MicroWaveAPI
