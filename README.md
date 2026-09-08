# CP4 - API de Tarefas

Este projeto consiste em uma API REST em ASP.NET Core para gerenciamento de tarefas, com persistência em arquivo JSON, logs estruturados, health check e instrumentação para observabilidade.

## Integrantes

Equipe responsável pelo desenvolvimento do projeto:

- Arthur Graciani Bezerra RM 561728
- Gustavo Pinheiro de Oliveira RM 566358
- Lucas Hideki P. S. de Souza RM 565355


## O que foi implementado

A aplicação foi estruturada em camadas para manter organização e facilitar manutenção:

- Controller: expõe os endpoints da API.
- Service: valida regras de negócio e mapeia a lógica de criação e listagem.
- Repository: persiste as tarefas em arquivo JSON.
- DTOs: desacoplamento entre entrada/saída e modelo de domínio.
- Infrastructure: configuração de injeção de dependência, OpenTelemetry e health checks.
- Diagnostics: constantes para nome do serviço e métrica customizada.

Os endpoints principais são:

- GET /api/tarefas
- POST /api/tarefas

A API também possui os endpoints de observabilidade e documentação abaixo.

## Versões do que está sendo usado

### .NET e ASP.NET Core

- Target Framework: .NET 10 (`net10.0`)
- ASP.NET Core Web API
- OpenAPI / Minimal API com `AddOpenApi()` e `MapOpenApi()`

### Bibliotecas principais

- Serilog: `4.4.0`
- Serilog.AspNetCore: `10.0.0`
- OpenTelemetry: `1.18.0`
- OpenTelemetry.Api: `1.18.0`
- OpenTelemetry.Extensions.Hosting: `1.18.0`
- OpenTelemetry.Instrumentation.AspNetCore: `1.18.0`
- OpenTelemetry.Instrumentation.Http: `1.18.0`
- Microsoft.AspNetCore.OpenApi: `10.0.5`

### Bibliotecas de teste

- Microsoft.NET.Test.Sdk: `17.12.0`
- xUnit: `2.9.2`
- xUnit Runner VisualStudio: `2.8.2`
- Moq: `4.20.72`
- Coverlet Collector: `6.0.2`
- Microsoft.AspNetCore.Mvc.Testing: `10.0.0`

## Endpoints de observabilidade

### Health Check

- Endpoint: `/health`
- Finalidade: verificar se a aplicação está saudável e pronta para receber requisições.

### Tarefa
- Endpoint: `/api/tarefas`
- Finalidade: criar e listar tarefas.

### Logs estruturados

- A aplicação usa Serilog para registrar logs em console no formato estruturado.
- O nível mínimo de log foi configurado para reduzir ruído e manter registros úteis de execução.

### Telemetria com OpenTelemetry

- Instrumentação de entrada HTTP com ASP.NET Core.
- Instrumentação de chamadas HTTP de saída com HttpClient.
- Métricas e traces customizados para a API de tarefas.

Essas métricas e traces ajudam a monitorar uso, erros e fluxo de requisições em ambientes de produção ou desenvolvimento.

## Fluxos testados

### Testes unitários

Arquivo: `CP4_to_do_api.UnitTests/TarefaServiceTests.cs`

Os fluxos testados foram:

1. Criação de tarefa com título válido
   - Deve criar a tarefa corretamente.
   - Deve persistir via repositório.
   - Deve retornar resposta com Id e Nome esperados.

2. Criação de tarefa com título nulo, vazio ou em branco
   - Deve lançar `ArgumentException`.
   - Não deve chamar o repositório para persistência.

3. Listagem de tarefas existentes
   - Deve buscar todas as tarefas do repositório.
   - Deve mapear corretamente para a resposta da API.

### Testes de integração

Arquivos:

- `CP4_to_do_api.IntegrationTests/TarefaPostEndpointTests.cs`
- `CP4_to_do_api.IntegrationTests/TarefaGetEndpointTests.cs`

Fluxos cobertos:

1. POST `/api/tarefas` com dados válidos
   - Retorna status `201 Created`.
   - Inclui `Location` no cabeçalho.
   - Retorna o objeto da tarefa criada.

2. POST `/api/tarefas` com título inválido
   - Retorna status `400 Bad Request`.
   - Valida a regra de negócio de título obrigatório.

3. GET `/api/tarefas`
   - Retorna status `200 OK`.
   - Lista as tarefas cadastradas.
   - Valida que a tarefa criada anteriormente aparece na resposta.

## Observações importantes

### Persistência

- As tarefas são armazenadas em um arquivo JSON local.
- O caminho do arquivo é configurado pela chave `TarefasFile`.
- Caso o arquivo não exista, a aplicação cria um arquivo vazio (`[]`).

### Isolamento dos testes

- Os testes de integração usam `WebApplicationFactory`.
- Cada execução cria um arquivo JSON temporário para evitar interferência entre rodadas de teste e manter o ambiente limpo.

### Arquitetura de monitoramento

- A aplicação foi preparada para coletar métricas e traces com OpenTelemetry.
- A configuração inclui observabilidade da infraestrutura ASP.NET Core e de requisições HTTP.

### Regras de negócio da tarefa

- O nome da tarefa é obrigatório.
- Caso venha nulo, vazio ou em branco, a API rejeita a operação com erro de validação.

## Como executar o projeto

1. Acesse a pasta raiz do projeto.
2. Execute o comando:

```bash
dotnet restore
```

3. Em seguida:

```bash
dotnet run --project WebApplication1/to_do_api.csproj
```

4. Para rodar os testes:

```bash
dotnet test
```

## Conclusão

O projeto entrega uma API funcional de tarefas com boa organização em camadas, validações de negócio, persistência simples em JSON, logs estruturados e observabilidade via OpenTelemetry e health checks. A estrutura também foi validada com testes unitários e integrados, cobrindo os principais fluxos da aplicação.
